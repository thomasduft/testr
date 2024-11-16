namespace tomware.TestR;

public class TestCaseValidator
{
  private readonly TestCase _testCase;

  public string TestCaseId => _testCase.Id;
  public string TestCaseTitle => _testCase.Title;

  public TestCaseValidator(TestCase testCase)
  {
    _testCase = testCase;
  }

  public TestCaseValidationResult ValidateSteps(
    IEnumerable<TestStep> steps
  )
  {
    var result = new TestCaseValidationResult(TestCaseId, TestCaseTitle);

    foreach (var step in steps)
    {
      if (string.IsNullOrWhiteSpace(step.TestData))
        continue;

      try
      {
        _ = TestStepInstruction.FromTestStep(step);
      }
      catch (Exception ex)
      {
        result.AddError(step.Id, ex.Message);
      }
    }

    return result;
  }
}