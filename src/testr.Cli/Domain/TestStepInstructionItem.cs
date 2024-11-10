using Microsoft.Playwright;

namespace tomware.TestR;

public class TestStepInstructionItem
{
  private readonly TestStep _step;

  public LocatorType Locator { get; set; }
  public AriaRole AriaRole { get; set; }
  public string Text { get; set; } = string.Empty;
  public string Value { get; set; } = string.Empty;
  public ActionType Action { get; set; }

  public TestStep TestStep => _step;

  public TestStepInstructionItem(TestStep step)
  {
    _step = step;
  }

  public static TestStepInstructionItem FromTestStep(
    TestStep step
  )
  {
    var testDataParameters = ParseTestData(step.TestData, ' ');
    if (testDataParameters.Count == 0)
      throw new InvalidDataException($"No TestData found for Test Step {step.Id}");

    TestStepInstructionItem instructionItem = new(step);

    foreach (var parameter in testDataParameters)
    {
      switch (parameter.Key)
      {
        case nameof(TestStepInstructionItem.Locator):
          instructionItem.Locator = Enum.Parse<LocatorType>(parameter.Value);
          break;
        case nameof(TestStepInstructionItem.AriaRole):
          instructionItem.AriaRole = Enum.Parse<AriaRole>(parameter.Value);
          break;
        case nameof(TestStepInstructionItem.Text):
          instructionItem.Text = parameter.Value;
          break;
        case nameof(TestStepInstructionItem.Value):
          instructionItem.Value = parameter.Value;
          break;
        case nameof(TestStepInstructionItem.Action):
          instructionItem.Action = Enum.Parse<ActionType>(parameter.Value);
          break;
        default:
          throw new InvalidDataException($"Unsupported parameter '{parameter.Key}' found in TestData of Test Step {step.Id}");
      }
    }

    return instructionItem;
  }

  private static Dictionary<string, string> ParseTestData(
    string testData,
    char separator
  )
  {
    var result = new Dictionary<string, string>();
    if (string.IsNullOrEmpty(testData))
      return result;

    foreach (var pair in testData.Split(separator))
    {
      var parts = pair.Split('=');
      if (parts.Length == 2)
      {
        result[parts[0].Trim()] = parts[1].Trim();
      }
    }

    return result;
  }
}