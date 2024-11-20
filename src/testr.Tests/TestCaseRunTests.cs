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
    await testCaseRun.SaveAsync(outputDirectory, true, default);

    // Assert
    var testCaseRunFile = Path.Combine(outputDirectory, $"{testCase.Id}.md");
    Assert.True(File.Exists(testCaseRunFile));
    
    var testCaseRunExecution = await TestCase.FromTestCaseFileAsync(testCaseRunFile, default);
    Assert.Equal(testCase.Id, testCaseRunExecution.Id);
    Assert.Equal(testCase.Title, testCaseRunExecution.Title);
    Assert.Equal("Execution", testCaseRunExecution.Type);
    Assert.Equal("Passed", testCaseRunExecution.Status);
  }
}