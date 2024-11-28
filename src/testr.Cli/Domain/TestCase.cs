namespace tomware.TestR;

internal class TestCase
{
  public string Id { get; set; } = string.Empty;
  public string Title { get; set; } = string.Empty;
  public string Type { get; set; } = string.Empty;
  public string Status { get; set; } = string.Empty;
  public string Route { get; set; } = string.Empty;
  public IEnumerable<TestStep> Steps { get; set; } = []; 
  public string File { get; set; } = string.Empty;
  public bool IsDefinition => !string.IsNullOrEmpty(Type) && Type == "Definition";
  public string LinkedFile { get; set; } = string.Empty;
  public bool HasLinkedFile => !string.IsNullOrEmpty(LinkedFile);

  public static async Task<TestCase> FromTestCaseFileAsync(
    string file,
    CancellationToken cancellationToken
  )
  {
    var parser = new TestCaseParser(file);

    return await parser.ToTestCaseAsync(cancellationToken);
  }
}