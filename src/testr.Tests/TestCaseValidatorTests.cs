using tomware.TestR;

namespace testr.Tests;

public class TestCaseValidatorTests
{
  [Fact]
  public async Task Validate_With4TestSteps_ShouldReturnNoValidationErrors()
  {
    // Arrange
    var file = Path.Combine(Environment.CurrentDirectory, "TestData", "TC-001-Login.md");
    var testCase = await TestCase
      .FromTestCaseFileAsync(file, default);
    testCase.WithVariables(new Dictionary<string, string>
    {
      {"Password", "my-super-secret"}
    });

    var validator = new TestCaseValidator(testCase);

    // Act
    var result = validator.Validate();

    // Assert
    Assert.NotNull(result);
    Assert.True(result.IsValid);
  }

  [Fact]
  public async Task Validate_WithNotAllowedType_ShouldReturnOneValidationError()
  {
    // Arrange
    var file = Path.Combine(Environment.CurrentDirectory, "TestData", "TC-003-Login-with-wrong-type.md");
    var testCase = await TestCase.FromTestCaseFileAsync(file, default);

    var validator = new TestCaseValidator(testCase);

    // Act
    var result = validator.Validate();

    // Assert
    Assert.NotNull(result);
    Assert.False(result.IsValid);
    Assert.True(result.Errors.Count() == 1);
    Assert.Equal("Type must be 'Definition' or 'Run'.", result.Errors.First());
  }

  [Fact]
  public async Task Validate_WithNotAllowedStatus_ShouldReturnOneValidationError()
  {
    // Arrange
    var file = Path.Combine(Environment.CurrentDirectory, "TestData", "TC-003-Login-with-wrong-status.md");
    var testCase = await TestCase.FromTestCaseFileAsync(file, default);

    var validator = new TestCaseValidator(testCase);

    // Act
    var result = validator.Validate();

    // Assert
    Assert.NotNull(result);
    Assert.False(result.IsValid);
    Assert.True(result.Errors.Count() == 1);
    Assert.Equal("Status must be either 'Passed', 'Failed' or 'Unknown'.", result.Errors.First());
  }

  [Fact]
  public async Task Validate_WithWrongLinkedNotExistingFile_ShouldReturnOneValidationError()
  {
    // Arrange
    var file = Path.Combine(Environment.CurrentDirectory, "TestData", "TC-003-Login-with-not-existing-linked-file.md");
    var testCase = await TestCase.FromTestCaseFileAsync(file, default);

    var validator = new TestCaseValidator(testCase);

    // Act
    var result = validator.Validate();

    // Assert
    Assert.NotNull(result);
    Assert.False(result.IsValid);
    Assert.True(result.Errors.Count() == 1);
    Assert.StartsWith("Linked file ", result.Errors.First());
  }

  [Fact]
  public async Task Validate_WithWrongTestData_ShouldReturnOneValidationErrors()
  {
    // Arrange
    var file = Path.Combine(Environment.CurrentDirectory, "TestData", "TC-003-Login-Validation-Errors.md");
    var testCase = await TestCase.FromTestCaseFileAsync(file, default);

    var validator = new TestCaseValidator(testCase);

    // Act
    var result = validator.Validate();

    // Assert
    Assert.NotNull(result);
    Assert.False(result.IsValid);
    Assert.True(result.Errors.Count() == 1);
    Assert.Equal("Step 4: Requested value 'GetRole' was not found.", result.Errors.First());
  }

  [Fact]
  public async Task Validate_WithMissingVariables_ShouldReturnValidationErrors()
  {
    // Arrange
    var file = Path.Combine(Environment.CurrentDirectory, "TestData", "TC-001-Login.md");
    var testCase = await TestCase
      .FromTestCaseFileAsync(file, default);

    var validator = new TestCaseValidator(testCase);

    // Act
    var result = validator.Validate();

    // Assert
    Assert.NotNull(result);
    Assert.False(result.IsValid);
  }
}
