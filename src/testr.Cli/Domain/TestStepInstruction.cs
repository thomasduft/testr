using System.Text.RegularExpressions;

using Microsoft.Playwright;

namespace tomware.TestR;

public class TestStepInstruction
{
  private readonly TestStep _step;

  public LocatorType Locator { get; set; }
  public AriaRole AriaRole { get; set; }
  public string Text { get; set; } = string.Empty;
  public string Value { get; set; } = string.Empty;
  public ActionType Action { get; set; }

  public TestStep TestStep => _step;

  public TestStepInstruction(TestStep step)
  {
    _step = step;
  }

  public static TestStepInstruction FromTestStep(
    TestStep step
  )
  {
    var testDataParameters = ParseTestData(step.TestData);
    if (testDataParameters.Count == 0)
      throw new InvalidDataException($"No TestData found for Test Step {step.Id}");

    TestStepInstruction testStepInstruction = new(step);

    foreach (var parameter in testDataParameters)
    {
      switch (parameter.Key)
      {
        case nameof(Locator):
          testStepInstruction.Locator = Enum.Parse<LocatorType>(parameter.Value);
          break;
        case nameof(AriaRole):
          testStepInstruction.AriaRole = Enum.Parse<AriaRole>(parameter.Value);
          break;
        case nameof(Text):
          testStepInstruction.Text = parameter.Value;
          break;
        case nameof(Value):
          testStepInstruction.Value = parameter.Value;
          break;
        case nameof(Action):
          testStepInstruction.Action = Enum.Parse<ActionType>(parameter.Value);
          break;
        default:
          throw new InvalidDataException($"Unsupported parameter '{parameter.Key}' found in TestData of Test Step {step.Id}");
      }
    }

    return testStepInstruction;
  }

  private static Dictionary<string, string> ParseTestData(
    string testData
  )
  {
    var result = new Dictionary<string, string>();
    if (string.IsNullOrEmpty(testData))
      return result;

    var pattern = @"(\w+)=(""[^""]*""|\S+)";
    var matches = Regex.Matches(testData, pattern);

    foreach (Match match in matches)
    {
      var key = match.Groups[1].Value;
      var value = match.Groups[2].Value;
      result[key] = value;
    }

    return result;
  }
}