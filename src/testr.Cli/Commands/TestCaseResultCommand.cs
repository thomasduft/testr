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
    Name = "test-case-result";
    Description = "Copies an existing Test Case definition and creates an execution result (i.e. test-case-result <my-test-case-definition-directory> <my-output-directory> TC-001 <my-execution-status>).";

    _inputDirectory = Argument<string>(
      "input-directory",
      "The input directory where the Test Case definition is located.",
      cfg => cfg.IsRequired()
    );

    _outputDirectory = Argument<string>(
      "output-directory",
      "The output directory where the Test Case result will be stored.",
      cfg => cfg.IsRequired()
    );

    _testCaseId = Argument<string>(
      "test-case-id",
      "The Test Case ID (e.g. TC-001).",
      cfg => cfg.IsRequired()
    );

    _executionResult = Argument<string>(
      "execution-result",
      "The execution result (e.g. PASSED, FAILED).",
      cfg => cfg.IsRequired()
    );

    OnExecuteAsync(ExecuteAsync);
  }

  private async Task<int> ExecuteAsync(CancellationToken cancellationToken)
  {
    bool result = bool.Parse(_executionResult.Value!);

    CopyAsTestCaseResult(
      _inputDirectory.Value!,
      _outputDirectory.Value!,
      _testCaseId.Value!,
      result
    );

    return await Task.FromResult(0);
  }

  private void CopyAsTestCaseResult(
    string inputDirectory,
    string outputDirectory,
    string testCaseId,
    bool result
  )
  {
    // 1. Find TestCase definition
    var file = FindFile(inputDirectory, testCaseId);

    // 2. Read lines and find Tag and change from Definition to Execution
    var lines = File.ReadAllLines(file);
    ReplacePropertyValue(lines, "Date", DateStringProvider.GetDateString());
    ReplacePropertyValue(lines, "Type", "Execution");
    ReplacePropertyValue(lines, "Status", result ? "Passed" : "Failed");

    // 3. Write TestCase execution to output directory.
    File.WriteAllLines(
      $"{outputDirectory}/{testCaseId}.md",
      lines
    );
  }

  private string FindFile(string inputDirectory, string testCaseId)
  {
    foreach (var file in Directory.GetFiles(inputDirectory, "*.md", SearchOption.TopDirectoryOnly))
    {
      var splittedItems = File
        .ReadAllLines(file!)
        .FirstOrDefault()!
        .Split(":");
      if (splittedItems[0].Trim().ToLower().Contains(testCaseId.ToLower()))
      {
        return file;
      }
    }

    throw new FileNotFoundException($"TestCase definition for '{testCaseId}' not found!");
  }

  private void ReplacePropertyValue(string[] lines, string property, string value)
  {
    var line = lines.FirstOrDefault(l => l.StartsWith($"- **{property}**:"));
    var splittedItems = line!.Split(':');
    lines[Array.IndexOf(lines, line)] = line.Replace(splittedItems[1].Trim(), value);
  }
}