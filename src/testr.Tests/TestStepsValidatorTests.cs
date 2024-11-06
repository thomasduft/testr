using tomware.TestR;

namespace testr.Tests;

public class TestStepsValidatorTests
{
  [Fact]
  public async Task ValidateSteps_With4TestSteps_ShouldReturnNoValidationErrors()
  {
    // Arrange
    var file = Path.Combine(Environment.CurrentDirectory, "TestData", "ExecutableTestCase.md");
    var testCase = await TestCase.FromTestCaseDefinitionAsync(file, default);

    var validator = new TestStepsValidator(testCase.Id, testCase.Title);

    // Act
    var result = validator.ValidateSteps(testCase.Steps);

    // Assert
    Assert.NotNull(result);
    Assert.True(result.IsValid);
  }
}