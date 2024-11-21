
using McMaster.Extensions.CommandLineUtils;

namespace tomware.TestR;

public class RunCommand : CommandLineApplication
{
  private readonly CommandArgument<string> _testCaseId;
  private readonly CommandArgument<string> _domain;
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
    Description = "Runs a Test Case definition (i.e. TC-Audit-001 \"https://localhost:5001\").";

    _testCaseId = Argument<string>(
      "test-case-id",
      "The Test Case ID (e.g. TC-Audit-001).",
      cfg => cfg.IsRequired()
    );

    _domain = Argument<string>(
      "domain",
      "The domain to run the Test Case against.",
      cfg => cfg.IsRequired()
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
      cfg => cfg.DefaultValue = ".",
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
      "-c|--continue-on-failure",
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
      cfg => cfg.DefaultValue = 30000,
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
    //1. Locate the Test Case definition file
    var file = TestCaseFileLocator.FindFile(_inputDirectory.ParsedValue, _testCaseId.ParsedValue);

    // 2. Read the Test Case definition
    var testCase = await TestCase.FromTestCaseFileAsync(file, cancellationToken);

    // 3. Validate the Test Case definition
    var testCaseValidator = new TestCaseValidator(testCase);
    var validationResult = testCaseValidator.ValidateSteps(testCase.Steps);
    if (!validationResult.IsValid)
    {
      foreach (var error in validationResult.Errors)
      {
        Console.WriteLine($"Step {error.StepId}: {error.ErrorMessage}");
      }

      return await Task.FromResult(1);
    }

    // 4. Run the Test Case steps
    ExecutorConfig executorConfig = GetExecutorConfiguration();
    var executor = new TestCaseExecutor(executorConfig);
    var testStepResults = await executor.ExecuteAsync(
      _domain.ParsedValue,
      testCase.Route,
      testCase.Steps.Select(step => TestStepInstruction.FromTestStep(step))
    );
    var success = testStepResults.All(r => r.IsSuccess);
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

    // 5. Store the Test Case run
    var run = new TestCaseRun(testCase, testStepResults);
    await run.SaveAsync(
      _outputDirectory.ParsedValue,
      cancellationToken
    );

    return await Task.FromResult(0);
  }

  private ExecutorConfig GetExecutorConfiguration()
  {
    return new ExecutorConfig(
      _headless.ParsedValue,
      _slow.DefaultValue,
      _timeout.ParsedValue,
      _browserType.ParsedValue,
      _recordVideoDir.ParsedValue
    );
  }
}
