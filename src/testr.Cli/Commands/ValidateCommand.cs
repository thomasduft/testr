using McMaster.Extensions.CommandLineUtils;

namespace tomware.TestR;

public class ValidateCommand : CommandLineApplication
{
  private readonly CommandArgument<string> _testCaseId;
  private readonly CommandArgument<string> _title;

  public ValidateCommand()
  {
    Name = "validate";
    Description = "Validates a Test Case definition (i.e. TC-Audit-001).";

    _testCaseId = Argument<string>(
      "test-case-id",
      "The Test Case ID (e.g. TC-Audit-001).",
      cfg => cfg.IsRequired()
    );

    _title = Argument<string>(
      "title",
      "The Test Case title.",
      cfg => cfg.DefaultValue = "A TestCase Title"
    );

    OnExecuteAsync(ExecuteAsync);
  }

  private async Task<int> ExecuteAsync(CancellationToken cancellationToken)
  {
    Console.WriteLine("Validating Test Case...");

    return await Task.FromResult(0);
  }
}