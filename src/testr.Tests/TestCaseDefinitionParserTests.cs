using tomware.TestR;

namespace testr.Tests;

public class TestCaseDefinitionParserTests
{
  [Fact]
  public async Task GetTestCaseIdAsync_ReadsExecutableTestCase_ShouldReturnTestCaseId()
  {
    // Arrange
    var file = Path.Combine(Environment.CurrentDirectory, "TestData", "ExecutableTestCase.md");
    var parser = new TestCaseDefinitionParser(file);

    // Act
    var testCaseId = await parser.GetTestCaseIdAsync(default);

    // Assert
    Assert.Equal("TC-001-Login", testCaseId);
  }

  [Fact]
  public async Task GetTestCaseTitleAsync_ReadsExecutableTestCase_ShouldReturnTestCaseTitle()
  {
    // Arrange
    var file = Path.Combine(Environment.CurrentDirectory, "TestData", "ExecutableTestCase.md");
    var parser = new TestCaseDefinitionParser(file);

    // Act
    var title = await parser.GetTestCaseTitleAsync(default);

    // Assert
    Assert.Equal("Login", title);
  }

  [Fact]
  public async Task GetRouteAsync_ReadsExecutableTestCase_ShouldReturnTestCaseRoute()
  {
    // Arrange
    var file = Path.Combine(Environment.CurrentDirectory, "TestData", "ExecutableTestCase.md");
    var parser = new TestCaseDefinitionParser(file);

    // Act
    var route = await parser.GetRouteAsync(default);

    // Assert
    Assert.Equal("Login", route);
  }

  [Fact]
  public async Task GetTestStepsAsync_ReadsExecutableTestCase_ShouldExtract4TestSteps()
  {
    // Arrange
    var file = Path.Combine(Environment.CurrentDirectory, "TestData", "ExecutableTestCase.md");
    var parser = new TestCaseDefinitionParser(file);

    // Act
    IEnumerable<TestStep> steps = await parser.GetTestStepsAsync(default);

    // Assert
    Assert.Equal(4, steps.Count());

    AssertTestStep(steps.First(), 1, "enter username", "Locator=GetByLabel Text=Username Action=Fill Value=admin", "username is entered");
    AssertTestStep(steps.Skip(1).First(), 2, "enter password", "Locator=GetByLabel Text=Password Action=Fill Value=password", "password is entered");
    AssertTestStep(steps.Skip(2).First(), 3, "click login button", "Locator=GetByRole AriaRole=Button Text=Login Action=Click", "system validates the user credentials and redirects to dashboard");
    AssertTestStep(steps.Last(), 4, "displays the dashboard", "Locator=GetByRole AriaRole=Button Text=Logout Action=IsVisible", "Logout button visible in the main navigation");
  }

  private void AssertTestStep(
    TestStep step,
    int id,
    string description,
    string testData,
    string expectedResult
  )
  {
    Assert.Equal(id, step.Id);
    Assert.Equal(description, step.Description);
    Assert.Equal(testData, step.TestData);
    Assert.Equal(expectedResult, step.ExpectedResult);
  }
}
