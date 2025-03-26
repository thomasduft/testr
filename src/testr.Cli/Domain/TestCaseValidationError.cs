namespace tomware.TestR;

internal record TestCaseValidationError(string Property, string ErrorMessage)
{
  public override string ToString()
  {
    return $"{ErrorMessage}";
  }
}