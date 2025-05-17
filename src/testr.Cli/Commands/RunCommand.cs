using System.Diagnostics;
using System.Diagnostics.Metrics;

using McMaster.Extensions.CommandLineUtils;

namespace tomware.TestR;

public class RunCommand : CommandLineApplication
{
  private static readonly Meter TestCaseMeter
    = new("tomware.TestR.Cli.Metrics");
  private static readonly Gauge<int> TestCaseGauge
    = TestCaseMeter.CreateGauge<int>("testr_test_case");

  private readonly Stopwatch _stopwatch = new();

  private readonly CommandArgument<string> _domain;
  private readonly CommandOption<string> _testCaseId;
  private readonly CommandOption<string> _inputDirectory;
  private readonly CommandOption<string> _outputDirectory;
  private readonly CommandOption<bool> _headless;
  private readonly CommandOption<bool> _continueOnFailure;
  private readonly CommandOption<int> _slow;
  private readonly CommandOption<int> _timeout;
  private readonly CommandOption<BrowserType> _browserType;
  private readonly CommandOption<string> _recordVideoDir;

  public RunCommand()
  {
    Name = "run";
    Description = "Runs Test Case definitions (i.e. \"https://localhost:5001\" -tc TC-Audit-001).";

    _domain = Argument<string>(
      "domain",
      "The domain to run the Test Case against.",
      cfg => cfg.IsRequired()
    );

    _testCaseId = Option<string>(
      "-tc|--test-case-id",
      "The Test Case ID (e.g. TC-Audit-001).",
      CommandOptionType.SingleValue,
      cfg => cfg.DefaultValue = null,
      true
    );

    _inputDirectory = Option<string>(
      "-i|--input-directory",
      "The input directory where the Test Case definition is located.",
      CommandOptionType.SingleValue,
      cfg => cfg.DefaultValue = ".",
      true
    );

    _outputDirectory = Option<string>(
      "-o|--output-directory",
      "The output directory where the Test Case result will be stored.",
      CommandOptionType.SingleValue,
      cfg => cfg.DefaultValue = null,
      true
    );

    _headless = Option<bool>(
      "--headless",
      "Runs the browser in headless mode.",
      CommandOptionType.NoValue,
      cfg => cfg.DefaultValue = false,
      true
    );

    _continueOnFailure = Option<bool>(
      "--continue-on-failure",
      "Continues the Test Case execution even if the Test Case fails.",
      CommandOptionType.NoValue,
      cfg => cfg.DefaultValue = false,
      true
    );

    _slow = Option<int>(
      "-s|--slow",
      "Slows down the execution by the specified amount of milliseconds.",
      CommandOptionType.SingleValue,
      cfg => cfg.DefaultValue = 500,
      true
    );

    _timeout = Option<int>(
      "-t|--timeout",
      "Sets the timeout for awaiting the Playwright Locator in milliseconds.",
      CommandOptionType.SingleValue,
      cfg => cfg.DefaultValue = 10000,
      true
    );

    _browserType = Option<BrowserType>(
      "-bt|--browser-type",
      "Sets the browser type to run the Test Case against (currently supported Browsers: Chrome, Firefox, Webkit).",
      CommandOptionType.SingleValue,
      cfg => cfg.DefaultValue = BrowserType.Chrome,
      true
    );

    _recordVideoDir = Option<string>(
      "-rvd|--record-video-dir",
      "Records a video of the Test Case execution to the specified directory.",
      CommandOptionType.SingleValue,
      cfg => cfg.DefaultValue = null,
      true
    );

    OnExecuteAsync(ExecuteAsync);
  }

  private async Task<int> ExecuteAsync(CancellationToken cancellationToken)
  {
    // Locate the Test Case definition files
    var files = TestCaseFileLocator.FindFiles(
      _inputDirectory.ParsedValue,
      _testCaseId.ParsedValue
    );

    var outputDirectory = _outputDirectory.HasValue()
      ? _outputDirectory.ParsedValue
      : null;

    foreach (var file in files)
    {
      var result = await RunTestCaseAsync(
        _inputDirectory.ParsedValue,
        outputDirectory,
        file,
        cancellationToken
      );
      if (result != 0)
      {
        return result;
      }
    }

    return 0;
  }

  private async Task<int> RunTestCaseAsync(
    string inputDirectory,
    string? outputDirectory,
    string file,
    CancellationToken cancellationToken
  )
  {
    _stopwatch.Reset();
    var domain = _domain.ParsedValue;

    // Read the Test Case definition
    var testCase = await TestCase.FromTestCaseFileAsync(file, cancellationToken);
    testCase.SetDomain(domain);

    ConsoleHelper.WriteLineYellow($"Running Test Case: {testCase.Id}");

    // Validate the Test Case definition
    var testCaseValidator = new TestCaseValidator(testCase);
    var validationResult = testCaseValidator.Validate();
    if (!validationResult.IsValid)
    {
      foreach (var error in validationResult.Errors)
      {
        ConsoleHelper.WriteLineError(error);
      }

      return await Task.FromResult(1);
    }

    // Run the Test Case steps
    var executionParam = new TestCaseExecutionParam(
      testCase.Route,
      testCase.Steps.Select(TestStepInstruction.FromTestStep)
    );
    var preconditionExecutionParam = await GetPreconditionTestCaseExecutionParam(
      testCase,
      cancellationToken
    );

    ExecutorConfig executorConfig = GetExecutorConfiguration();
    var executor = new TestCaseExecutor(executorConfig);
    _stopwatch.Start();
    var testStepResults = await executor.ExecuteAsync(
      domain,
      executionParam,
      preconditionExecutionParam,
      cancellationToken
    );
    _stopwatch.Stop();
    var success = testStepResults.All(r => r.IsSuccess);

    TestCaseGauge.Record(1, [
      new("test_case_id", testCase.Id),
      new("module", testCase.Module),
      new("status", success ? Constants.TestCaseStatus.Passed : Constants.TestCaseStatus.Failed),
      new("duration", _stopwatch.ElapsedMilliseconds),
    ]);

    if (!success)
    {
      foreach (var result in testStepResults.Where(r => !r.IsSuccess))
      {
        ConsoleHelper.WriteLineError($"Test Case Step failed: {result.Error}");
      }

      if (!_continueOnFailure.ParsedValue)
      {
        return await Task.FromResult(1);
      }
    }

    if (!string.IsNullOrWhiteSpace(outputDirectory))
    {
      // Store the Test Case run
      var run = new TestCaseRun(testCase, testStepResults);
      await run.SaveAsync(
        inputDirectory,
        outputDirectory,
        cancellationToken
      );
    }

    ConsoleHelper.WriteLineSuccess($"Test Case {testCase.Id} executed successfully.");

    return await Task.FromResult(0);
  }

  private async Task<TestCaseExecutionParam?> GetPreconditionTestCaseExecutionParam(
    TestCase testCase,
    CancellationToken cancellationToken
  )
  {
    if (!testCase.HasLinkedFile) return null;

    var linkedTestCase = await TestCase.FromTestCaseFileAsync(
      testCase.LinkedFile,
      cancellationToken
    );
    return new TestCaseExecutionParam(
      linkedTestCase.Route,
      linkedTestCase.Steps.Select(TestStepInstruction.FromTestStep)
    );
  }

  private ExecutorConfig GetExecutorConfiguration()
  {
    return new ExecutorConfig(
      _headless.ParsedValue,
      _slow.ParsedValue,
      _timeout.ParsedValue,
      _browserType.ParsedValue,
      _recordVideoDir.ParsedValue
    );
  }
}
