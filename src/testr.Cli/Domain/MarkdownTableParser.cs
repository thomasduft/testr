using System.Text.RegularExpressions;

namespace tomware.TestR;

/// <summary>
/// A lightweight markdown table parser specifically designed for parsing test steps
/// from markdown tables in the format used by TestR test cases.
/// </summary>
internal class MarkdownTableParser
{
  private readonly string _content;

  public MarkdownTableParser(string content)
  {
    _content = content;
  }

  /// <summary>
  /// Parses test steps from markdown tables found between STEPS:BEGIN and STEPS:END comments.
  /// Expected table format:
  /// | Step ID | Description | Test Data | Expected Result | Actual Result |
  /// | -------:| ----------- | --------- | --------------- | ------------- |
  /// | 1       | step desc   | test data | expected result | actual result |
  /// </summary>
  /// <returns>Collection of parsed test steps</returns>
  public IEnumerable<TestStep> ParseTestSteps()
  {
    var testSteps = new List<TestStep>();

    // Find the test steps section between comments
    var stepsSection = ExtractStepsSection();
    if (string.IsNullOrEmpty(stepsSection))
    {
      return testSteps;
    }

    // Parse the table rows
    var tableRows = ExtractTableRows(stepsSection);
    foreach (var row in tableRows)
    {
      var testStep = ParseTableRow(row);
      if (testStep != null)
      {
        testSteps.Add(testStep);
      }
    }

    return testSteps.OrderBy(ts => ts.Id);
  }

  /// <summary>
  /// Extracts the content between <!-- STEPS:BEGIN --> and <!-- STEPS:END --> comments
  /// </summary>
  private string ExtractStepsSection()
  {
    var beginPattern = @"<!--\s*STEPS:BEGIN\s*-->";
    var endPattern = @"<!--\s*STEPS:END\s*-->";

    var beginMatch = Regex.Match(_content, beginPattern, RegexOptions.IgnoreCase);
    if (!beginMatch.Success)
    {
      return string.Empty;
    }

    var endMatch = Regex.Match(_content, endPattern, RegexOptions.IgnoreCase);
    if (!endMatch.Success || endMatch.Index <= beginMatch.Index)
    {
      return string.Empty;
    }

    var startIndex = beginMatch.Index + beginMatch.Length;
    var length = endMatch.Index - startIndex;

    return _content.Substring(startIndex, length);
  }

  /// <summary>
  /// Extracts table rows from the steps section, excluding the header and separator rows
  /// </summary>
  private List<string> ExtractTableRows(string stepsSection)
  {
    var lines = stepsSection.Split('\n', StringSplitOptions.RemoveEmptyEntries)
      .Select(line => line.Trim())
      .Where(line => !string.IsNullOrEmpty(line))
      .ToList();

    var tableRows = new List<string>();
    var foundHeader = false;
    var foundSeparator = false;

    foreach (var line in lines)
    {
      // Check if this is a table row (starts and ends with |)
      if (!line.StartsWith("|") || !line.EndsWith("|"))
      {
        continue;
      }

      // Skip the header row (first table row we encounter)
      if (!foundHeader)
      {
        foundHeader = true;
        continue;
      }

      // Skip the separator row (contains only |, -, :, and spaces)
      if (!foundSeparator && IsSeparatorRow(line))
      {
        foundSeparator = true;
        continue;
      }

      // This is a data row
      if (foundSeparator)
      {
        tableRows.Add(line);
      }
    }

    return tableRows;
  }

  /// <summary>
  /// Checks if a line is a markdown table separator row
  /// </summary>
  private static bool IsSeparatorRow(string line)
  {
    // Remove outer pipes and check if content only contains allowed separator characters
    var content = line.Trim('|', ' ');
    return Regex.IsMatch(content, @"^[\s\-:|]+$");
  }

  /// <summary>
  /// Parses a single table row into a TestStep object
  /// </summary>
  private TestStep? ParseTableRow(string row)
  {
    try
    {
      var cells = ParseTableCells(row);

      // Ensure we have at least 4 cells (Step ID, Description, Test Data, Expected Result)
      // The 5th cell (Actual Result) is optional
      if (cells.Count < 4)
      {
        return null;
      }

      var testStep = new TestStep();

      // Parse Step ID (first cell)
      if (int.TryParse(cells[0].Trim(), out var stepId))
      {
        testStep.Id = stepId;
      }
      else
      {
        return null; // Invalid step ID
      }

      // Parse Description (second cell)
      testStep.Description = UnescapeMarkdown(cells[1].Trim());

      // Parse Test Data (third cell)
      testStep.TestData = UnescapeMarkdown(cells[2].Trim());

      // Parse Expected Result (fourth cell)
      testStep.ExpectedResult = UnescapeMarkdown(cells[3].Trim());

      // Parse Actual Result (fifth cell, optional)
      if (cells.Count > 4)
      {
        var actualResult = cells[4].Trim();
        // Check for success/failure indicators
        testStep.IsSuccess = actualResult.Contains("✅");
      }

      return testStep;
    }
    catch
    {
      // If parsing fails for any reason, return null
      return null;
    }
  }

  /// <summary>
  /// Parses table cells from a row, handling escaped pipes within cell content
  /// </summary>
  private List<string> ParseTableCells(string row)
  {
    var cells = new List<string>();
    var currentCell = string.Empty;

    // Remove leading and trailing pipes
    var content = row.Trim();
    if (content.StartsWith("|"))
    {
      content = content.Substring(1);
    }
    if (content.EndsWith("|"))
    {
      content = content.Substring(0, content.Length - 1);
    }

    for (int i = 0; i < content.Length; i++)
    {
      var c = content[i];

      if (c == '\\' && i + 1 < content.Length)
      {
        // Handle escaped characters
        var nextChar = content[i + 1];
        if (nextChar == '|' || nextChar == '\\')
        {
          currentCell += nextChar;
          i++; // Skip the next character
          continue;
        }
      }

      if (c == '|')
      {
        // Cell separator
        cells.Add(currentCell);
        currentCell = string.Empty;
      }
      else
      {
        currentCell += c;
      }
    }

    // Add the last cell
    cells.Add(currentCell);

    return cells;
  }

  /// <summary>
  /// Unescapes markdown content, particularly handling escaped quotes and special characters
  /// </summary>
  private static string UnescapeMarkdown(string content)
  {
    if (string.IsNullOrEmpty(content))
    {
      return content;
    }

    // Handle HTML entities that might be present
    content = content
      .Replace("&quot;", "\"")
      .Replace("&amp;", "&")
      .Replace("&lt;", "<")
      .Replace("&gt;", ">")
      .Replace("&nbsp;", " ");

    return content;
  }
}
