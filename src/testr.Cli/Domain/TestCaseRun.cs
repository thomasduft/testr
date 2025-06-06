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
    string inputDirectory,
    string outputDirectory,
    CancellationToken cancellationToken
  )
  {
    var lines = await File.ReadAllLinesAsync(_testCase.File, cancellationToken);

    SetProperties(lines, _results.All(r => r.IsSuccess));
    if (_testCase.HasDomain) lines = AppendDomainProperty(lines, _testCase.Domain);
    UpdateTestSteps(lines, _results);

    // Ensure directory structure based on the input directory
    var relativePath = Path.GetRelativePath(inputDirectory, _testCase.File);
    var outputDir = Path.Combine(outputDirectory, Path.GetDirectoryName(relativePath)!);
    if (!Directory.Exists(outputDir))
    {
      Directory.CreateDirectory(outputDir);
    }

    await File.WriteAllLinesAsync(
      $"{outputDir}/{_testCase.Id}.md",
      lines,
      cancellationToken
    );
  }

  private void SetProperties(string[] lines, bool result)
  {
    ReplacePropertyValue(lines, "Date", DateStringProvider.GetDateString());
    ReplacePropertyValue(lines, "Type", Constants.TestCaseType.Run);
    ReplacePropertyValue(lines, "Status", result
      ? Constants.TestCaseStatus.Passed
      : Constants.TestCaseStatus.Failed);
  }

  private void ReplacePropertyValue(string[] lines, string property, string value)
  {
    var line = lines.FirstOrDefault(l => l.StartsWith($"- **{property}**:"));
    var splittedItems = line!.Split(':');
    lines[Array.IndexOf(lines, line)] = line.Replace(splittedItems[1].Trim(), value);
  }

  private string[] AppendDomainProperty(string[] lines, string domain)
  {
    // find Status property
    var line = lines.FirstOrDefault(l => l.StartsWith($"- **Status**:"));
    // add Domain property after Status property
    var index = Array.IndexOf(lines, line) + 1;
    var lineToInsert = $"- **Domain**: {domain}";

    // insert new line at index
    return lines
      .Take(index)
      .Concat([lineToInsert])
      .Concat(lines.Skip(index))
      .ToArray();
  }

  private void UpdateTestSteps(string[] lines, IEnumerable<TestStepResult> testResults)
  {
    // <!-- STEPS:BEGIN -->
    // | Step ID | Description | Test Data | Expected Result | Actual Result |
    // | ------- | ----------- | --------- | --------------- | ------------- |
    // | 1       | tbd         | tbd       | tbd             |  ✅           |
    // | 2       | tbd         | tbd       | tbd             |  ❌ error txt |
    // <!-- STEPS:END -->

    var stepsBegin = Array.IndexOf(lines, "<!-- STEPS:BEGIN -->") + 3;
    var stepsEnd = Array.IndexOf(lines, "<!-- STEPS:END -->");
    var failed = false;

    for (var i = stepsBegin; i < stepsEnd; i++)
    {
      var splittedItems = lines[i].Split('|');
      var stepId = splittedItems[1].Trim();
      var testResult = testResults
        .FirstOrDefault(r => r.Step.Id.ToString().ToLowerInvariant() == stepId.ToLowerInvariant()
          && r.IsSuccess == false);
      failed = testResult != null;

      splittedItems[5] = !failed
        ? " ✅ "
        : $" ❌ {testResult?.Error} ";
      lines[i] = string.Join('|', splittedItems);
    }
  }
}