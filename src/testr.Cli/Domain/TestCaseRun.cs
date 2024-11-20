namespace tomware.TestR;

internal class TestCaseRun
{
  private readonly TestCase _testCase;
  private readonly IEnumerable<TestStepResult> _results;

  public TestCaseRun(
    TestCase testCase,
    IEnumerable<TestStepResult> results
  )
  {
    _testCase = testCase;
    _results = results;
  }

  internal async Task SaveAsync(
    string outputDirectory,
    bool result,
    CancellationToken cancellationToken
  )
  {
    var lines = await File.ReadAllLinesAsync(_testCase.File, cancellationToken);

    SetProperties(lines, result);
    UpdateTestSteps(lines, result);

    await File.WriteAllLinesAsync(
      $"{outputDirectory}/{_testCase.Id}.md",
      lines,
      cancellationToken
    );
  }

  private void SetProperties(string[] lines, bool result)
  {
    ReplacePropertyValue(lines, "Date", DateStringProvider.GetDateString());
    ReplacePropertyValue(lines, "Type", "Execution");
    ReplacePropertyValue(lines, "Status", result ? "Passed" : "Failed");
  }

  private void ReplacePropertyValue(string[] lines, string property, string value)
  {
    var line = lines.FirstOrDefault(l => l.StartsWith($"- **{property}**:"));
    var splittedItems = line!.Split(':');
    lines[Array.IndexOf(lines, line)] = line.Replace(splittedItems[1].Trim(), value);
  }

  private void UpdateTestSteps(string[] lines, bool result)
  {
    // TODO: Set ticks for each test step or add test step error for each failed test step
  }
}