
using McMaster.Extensions.CommandLineUtils;

namespace tomware.TestR;

public class RunCommand : CommandLineApplication
{
  private readonly CommandArgument<string> _testCaseId;
  private readonly CommandArgument<string> _domain;
  private readonly CommandArgument<string> _inputDirectory;
  private readonly CommandArgument<string> _outputDirectory;

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

    _inputDirectory = Argument<string>(
      "input-directory",
      "The input directory where the Test Case definition is located.",
      cfg => cfg.DefaultValue = "."
    );

    _outputDirectory = Argument<string>(
      "output-directory",
      "The output directory where the Test Case result will be stored.",
      cfg => cfg.DefaultValue = "."
    );

    OnExecuteAsync(ExecuteAsync);
  }

  private async Task<int> ExecuteAsync(CancellationToken cancellationToken)
  {
    Console.WriteLine("Running Test Case...");

    return await Task.FromResult(0);
  }
}
