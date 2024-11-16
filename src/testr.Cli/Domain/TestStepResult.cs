using tomware.TestR;

public record TestStepResult
{
  private readonly TestStep _step;

  public string TestStep { get; set; } = string.Empty;
  public bool IsSuccess { get; set; } = false;
  public string Error { get; set; } = string.Empty;

  public TestStep Step => _step;

  public TestStepResult(TestStep step)
  {
    _step = step;
  }

  public static TestStepResult Success(TestStep step)
  {
    return new TestStepResult(step)
    {
      IsSuccess = true
    };
  }

  public static TestStepResult Failed(TestStep step, string error)
  {
    return new TestStepResult(step)
    {
      IsSuccess = false,
      Error = error
    };
  }
}