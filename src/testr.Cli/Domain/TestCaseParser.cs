using HtmlAgilityPack;

using Markdig;

namespace tomware.TestR;

internal class TestCaseParser
{
  private readonly string _file;

  public TestCaseParser(string file)
  {
    _file = file;
  }

  internal async Task<TestCase> ToTestCaseAsync(CancellationToken cancellationToken)
  {
    var lines = await File.ReadAllLinesAsync(_file, cancellationToken);

    var (testCaseId, testCaseTitle) = GetTestCaseIdAndTitle(lines);
    var type = FindTag(lines, "Type");
    var status = FindTag(lines, "Status");
    var route = FindTag(lines, "Route");

    var markdownContent = string.Join(Environment.NewLine, lines);
    var steps = GetTestSteps(markdownContent);

    return new TestCase
    {
      Id = testCaseId,
      Title = testCaseTitle,
      Type = type,
      Status = status,
      Route = route,
      Steps = steps,
      File = _file
    };
  }

  private IEnumerable<TestStep> GetTestSteps(string markdownContent)
  {
    var testSteps = new List<TestStep>();

    var pipeline = new MarkdownPipelineBuilder()
        .UseAdvancedExtensions()
        .Build();

    var html = Markdown.ToHtml(markdownContent, pipeline);
    var hap = new HtmlDocument();
    hap.LoadHtml(html);

    // Extract the table content
    var tableNodes = hap.DocumentNode
      .Descendants("table")
      .ToList();
    foreach (var tableNode in tableNodes)
    {
      // Extract rows from the table
      var rowNodes = tableNode.Descendants("tr").ToList();

      foreach (var rowNode in rowNodes.Skip(1))
      {
        // Extract cells from the row
        var testStep = new TestStep();
        var cellNodes = rowNode.Descendants("td").ToList();

        // List each cell's content
        for (var i = 0; i < cellNodes.Count; i++)
        {
          var cellNode = cellNodes[i];
          var cellContent = cellNode.InnerText.Trim();

          switch (i)
          {
            case 0:
              testStep.Id = int.Parse(cellContent);
              break;
            case 1:
              testStep.Description = cellContent;
              break;
            case 2:
              testStep.TestData = cellContent;
              break;
            case 3:
              testStep.ExpectedResult = cellContent;
              break;
          }
        }

        testSteps.Add(testStep);
      }
    }

    return testSteps.OrderBy(ts => ts.Id);
  }

  private (string TestCaseId, string TestCaseTitle) GetTestCaseIdAndTitle(string[] lines)
  {
    // we are just reading the first line
    var line = lines.FirstOrDefault();

    var splittedItems = line!.Split(':');
    var testCaseId = splittedItems[0].Trim()
      .Replace(" ", string.Empty)
      .Replace("#", string.Empty)
      .Replace(":", string.Empty);
    var testCaseTitle = splittedItems[1].Trim();

    return (testCaseId, testCaseTitle);
  }

  private string FindTag(string[] lines, string tag)
  {
    var line = lines.FirstOrDefault(l => l.StartsWith($"- **{tag}**:"));

    var splittedItems = line!.Split(':');

    return splittedItems[1].Trim();
  }
}