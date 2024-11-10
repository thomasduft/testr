namespace tomware.TestR;

public class TestCase
{
  public string Id { get; set; } = string.Empty;
  public string Title { get; set; } = string.Empty;
  public string Route { get; set; } = string.Empty;
  public IEnumerable<TestStep> Steps { get; set; } = [];

  public static async Task<TestCase> FromTestCaseDefinitionAsync(
    string file,
    CancellationToken cancellationToken
  )
  {
    var extractor = new TestCaseDefinitionParser(file);

    var testCase = new TestCase
    {
      Id = await extractor.GetTestCaseIdAsync(cancellationToken),
      Title = await extractor.GetTestCaseTitleAsync(cancellationToken),
      Route = await extractor.GetRouteAsync(cancellationToken),
      Steps = await extractor.GetTestStepsAsync(cancellationToken)
    };

    return testCase;
  }
}