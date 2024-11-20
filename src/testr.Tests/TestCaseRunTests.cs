using tomware.TestR;

namespace testr.Tests;

public class TestCaseRunTests
{
  [Fact]
  public async Task SaveAsync_WithTestCaseDefinition_ShouldCreatePassedTestCaseRun()
  {
    // Arrange
    var file = Path.Combine(Environment.CurrentDirectory, "TestData", "TC-001-Login.md");
    var testCase = await TestCase.FromTestCaseFileAsync(file, default);

    var testCaseRun = new TestCaseRun(testCase, []);
    var outputDirectory = Path.Combine(Environment.CurrentDirectory, "TestRuns");

    // Act
    await testCaseRun.SaveAsync(outputDirectory, default);

    // Assert
    var testCaseRunFile = Path.Combine(outputDirectory, $"{testCase.Id}.md");
    Assert.True(File.Exists(testCaseRunFile));

    var testCaseRunExecution = await TestCase.FromTestCaseFileAsync(testCaseRunFile, default);
    Assert.Equal(testCase.Id, testCaseRunExecution.Id);
    Assert.Equal(testCase.Title, testCaseRunExecution.Title);
    Assert.Equal("Execution", testCaseRunExecution.Type);
    Assert.Equal("Passed", testCaseRunExecution.Status);
  }

  [Fact]
  public async Task SaveAsync_WithTestCaseDefinition_ShouldCreateFailedTestCaseRun()
  {
    // Arrange
    var file = Path.Combine(Environment.CurrentDirectory, "TestData", "TC-002-Login-Fails.md");
    var testCase = await TestCase.FromTestCaseFileAsync(file, default);

    var results = new List<TestStepResult>{
      TestStepResult.Success(new TestStep { Id = 1, Description = "enter username" }),
      TestStepResult.Success(new TestStep { Id = 2, Description = "enter password" }),
      TestStepResult.Failed(new TestStep { Id = 3, Description = "click login button" }, "Login failed"),
      TestStepResult.Failed(new TestStep { Id = 4, Description = "displays the dashboard" }, "No access to dashboard")
    };
    var testCaseRun = new TestCaseRun(testCase, results);
    var outputDirectory = Path.Combine(Environment.CurrentDirectory, "TestRuns");

    // Act
    await testCaseRun.SaveAsync(outputDirectory, default);

    // Assert
    var testCaseRunFile = Path.Combine(outputDirectory, $"{testCase.Id}.md");
    Assert.True(File.Exists(testCaseRunFile));

    var testCaseRunExecution = await TestCase.FromTestCaseFileAsync(testCaseRunFile, default);
    Assert.Equal(testCase.Id, testCaseRunExecution.Id);
    Assert.Equal(testCase.Title, testCaseRunExecution.Title);
    Assert.Equal("Execution", testCaseRunExecution.Type);
    Assert.Equal("Failed", testCaseRunExecution.Status);
  }
}