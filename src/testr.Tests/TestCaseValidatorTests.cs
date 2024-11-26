using tomware.TestR;

namespace testr.Tests;

public class TestCaseValidatorTests
{
  [Fact]
  public async Task ValidateSteps_With4TestSteps_ShouldReturnNoValidationErrors()
  {
    // Arrange
    var file = Path.Combine(Environment.CurrentDirectory, "TestData", "TC-001-Login.md");
    var testCase = await TestCase.FromTestCaseFileAsync(file, default);

    var validator = new TestCaseValidator(testCase);

    // Act
    var result = validator.ValidateSteps(testCase.Steps);

    // Assert
    Assert.NotNull(result);
    Assert.True(result.IsValid);
  }

  [Fact]
  public async Task ValidateSteps_WithWrongTestData_ShouldReturnOneValidationErrors()
  {
    // Arrange
    var file = Path.Combine(Environment.CurrentDirectory, "TestData", "TC-003-Login-Validation-Errors.md");
    var testCase = await TestCase.FromTestCaseFileAsync(file, default);

    var validator = new TestCaseValidator(testCase);

    // Act
    var result = validator.ValidateSteps(testCase.Steps);

    // Assert
    Assert.NotNull(result);
    Assert.False(result.IsValid);
    Assert.True(result.Errors.Count() == 1);
    Assert.Equal("Requested value 'GetRole' was not found.", result.Errors.First().ErrorMessage);
  }
}