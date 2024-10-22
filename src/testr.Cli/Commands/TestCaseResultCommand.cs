using McMaster.Extensions.CommandLineUtils;

namespace tomware.TestR;

public class TestCaseResultCommand : CommandLineApplication
{
  private readonly CommandArgument<string> _inputDirectory;
  private readonly CommandArgument<string> _outputDirectory;
  private readonly CommandArgument<string> _testCaseId;
  private readonly CommandArgument<string> _executionResult;

  public TestCaseResultCommand()
  {
    Name = "test-case";
    Description = "Copies an existing Test Case definition and creates an execution result (i.e. test-case-result <my-test-case-definition-directory> <my-output-directory> TC-001 <my-execution-status>).";

    _inputDirectory = Argument<string>(
      "input-directory",
      "The input directory where the Test Case definition is located.",
      cfg => cfg.IsRequired(),
      true
    );

    _outputDirectory = Argument<string>(
      "output-directory",
      "The output directory where the Test Case result will be stored.",
      cfg => cfg.IsRequired(),
      true
    );

    _testCaseId = Argument<string>(
      "test-case-id",
      "The Test Case ID (e.g. TC-001).",
      cfg => cfg.IsRequired(),
      true
    );

    _executionResult = Argument<string>(
      "execution-result",
      "The execution result (e.g. PASSED, FAILED).",
      cfg => cfg.IsRequired(),
      true
    );

    OnExecuteAsync(ExecuteAsync);
  }

  private async Task<int> ExecuteAsync(CancellationToken cancellationToken)
  {
    Console.WriteLine("Hi, I am just a sample command.");

    return await Task.FromResult(0);
  }
}