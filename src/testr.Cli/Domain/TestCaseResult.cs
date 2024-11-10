using tomware.TestR;

public record TestCaseResult
{
  private readonly TestStep _step;

  public string TestStep { get; set; } = string.Empty;
  public bool IsSuccess { get; set; } = false;
  public string Error { get; set; } = string.Empty;

  public TestStep Step => _step;

  public TestCaseResult(TestStep step)
  {
    _step = step;
  }

  public static TestCaseResult Success(TestStep step)
  {
    return new TestCaseResult(step)
    {
      IsSuccess = true
    };
  }

  public static TestCaseResult Failed(TestStep step, string error)
  {
    return new TestCaseResult(step)
    {
      IsSuccess = false,
      Error = error
    };
  }
}