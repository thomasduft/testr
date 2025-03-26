namespace tomware.TestR;

internal record TestCaseExecutionParam(
  string Route,
  IEnumerable<TestStepInstruction> Instructions
);