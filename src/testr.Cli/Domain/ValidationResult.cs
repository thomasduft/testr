namespace tomware.TestR;

public record ValidationResult
{
  private readonly List<TestStepValidationError> _errors = [];

  public string TestCaseId { get; private set; } = string.Empty;

  public string TestCaseTitle { get; private set; } = string.Empty;

  public bool IsValid
  {
    get { return _errors.Count == 0; }
  }

  public IEnumerable<TestStepValidationError> Errors
  {
    get { return _errors; }
  }

  public ValidationResult(string testCaseId, string testCaseTitle)
  {
    TestCaseId = testCaseId;
    TestCaseTitle = testCaseTitle;
  }

  public void AddError(int stepId, string errorMessage)
  {
    _errors.Add(new TestStepValidationError(stepId, errorMessage));
  }
}
