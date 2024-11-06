using System.Security.Cryptography;

using Microsoft.Playwright;

namespace tomware.TestR;

public class TestStepsValidator
{
  public string TestCaseId { get; }
  public string TestCaseTitle { get; }

  public TestStepsValidator(string testCaseId, string testCaseTitle)
  {
    TestCaseId = testCaseId;
    TestCaseTitle = testCaseTitle;
  }

  public ValidationResult ValidateSteps(
    IEnumerable<TestStep> steps
  )
  {
    var result = new ValidationResult(TestCaseId, TestCaseTitle);

    foreach (var step in steps)
    {
      if (string.IsNullOrWhiteSpace(step.TestData))
        continue;

      try
      {
        _ = TestStepInstructionItem.FromTestStep(step);
      }
      catch (Exception ex)
      {
        result.AddError(step.Id, ex.Message);
      }
    }

    return result;
  }
}
