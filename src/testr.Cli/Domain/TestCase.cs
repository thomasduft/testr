namespace tomware.TestR;

internal class TestCase
{
  private string _domain = string.Empty;

  public string Id { get; set; } = string.Empty;
  public string Title { get; set; } = string.Empty;
  public string? Module {get; set; } = "Unkown";
  public string Type { get; set; } = string.Empty;
  public string Status { get; set; } = string.Empty;
  public string Route { get; set; } = string.Empty;
  public IEnumerable<TestStep> Steps { get; set; } = [];
  public string File { get; set; } = string.Empty;
  public bool IsDefinition => !string.IsNullOrEmpty(Type) && Type == "Definition";
  public string LinkedFile { get; set; } = string.Empty;
  public bool HasLinkedFile => !string.IsNullOrEmpty(LinkedFile);
  public string Domain => _domain;
  public bool HasDomain => !string.IsNullOrEmpty(_domain);
  public Dictionary<string, string> Variables { get; set; } = [];
  public bool HasVariables => Variables.Count > 0;

  public TestCase WithDomain(string domain)
  {
    _domain = domain;

    return this;
  }

  public TestCase WithVariables(Dictionary<string, string> variables)
  {
    Variables = variables;

    return this;
  }

  public static async Task<TestCase> FromTestCaseFileAsync(
    string file,
    CancellationToken cancellationToken
  )
  {
    var parser = new TestCaseParser(file);

    return await parser.ToTestCaseAsync(cancellationToken);
  }
}
