using McMaster.Extensions.CommandLineUtils;

namespace tomware.TestR;

public class TestCaseCommand : CommandLineApplication
{
  private readonly CommandArgument<string> _testCaseId;
  private readonly CommandArgument<string> _title;

  public TestCaseCommand()
  {
    Name = "test-case";
    Description = "Creates a new Test Case definition (i.e. test-case TC-001 \"My TestCase Title\").";

    _testCaseId = Argument<string>(
      "test-case-id",
      "The Test Case ID (e.g. TC-001).",
      cfg => cfg.IsRequired(),
      true
    );

    _title = Argument<string>(
      "title",
      "The Test Case title.",
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