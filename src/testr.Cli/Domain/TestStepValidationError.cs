namespace tomware.TestR;

internal record TestStepValidationError(int StepId, string ErrorMessage)
{
  public override string ToString()
  {
    return $"Step {StepId}: {ErrorMessage}";
  }
}