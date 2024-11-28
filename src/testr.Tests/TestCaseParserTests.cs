using tomware.TestR;

namespace testr.Tests;

public class TestCaseParserTests
{
  [Fact]
  public async Task ToTestCaseAsync_ReadsExecutableTestCase_ShouldReturnTestCase()
  {
    // Arrange
    var file = Path.Combine(Environment.CurrentDirectory, "TestData", "TC-001-Login.md");
    var parser = new TestCaseParser(file);

    // Act
    var testCase = await parser.ToTestCaseAsync(default);

    // Assert
    Assert.NotNull(testCase);
    Assert.Equal("TC-001-Login", testCase.Id);
    Assert.Equal("Login", testCase.Title);
    Assert.Equal("Definition", testCase.Type);
    Assert.Equal("Unknown", testCase.Status);
    Assert.Equal("Login", testCase.Route);
    Assert.Equal(file, testCase.File);
    Assert.True(testCase.IsDefinition);

    // Steps
    Assert.Equal(4, testCase.Steps.Count());

    AssertTestStep(testCase.Steps.First(), 1, "enter username", "Locator=GetByLabel Text=Username Action=Fill Value=admin", "username is entered");
    AssertTestStep(testCase.Steps.Skip(1).First(), 2, "enter password", "Locator=GetByLabel Text=Password Action=Fill Value=password", "password is entered");
    AssertTestStep(testCase.Steps.Skip(2).First(), 3, "click login button", "Locator=GetByRole AriaRole=Button Text=Login Action=Click", "system validates the user credentials and redirects to dashboard");
    AssertTestStep(testCase.Steps.Last(), 4, "displays the dashboard", "Locator=GetByRole AriaRole=Button Text=Logout Action=IsVisible", "Logout button visible in the main navigation");
  }

  [Fact]
  public async Task ToTestCaseAsync_ReadsExecutableTestCaseWithLink_ShouldReturnTestCase()
  {
    // Arrange
    var file = Path.Combine(Environment.CurrentDirectory, "TestData", "TC-010-Authenticated.md");
    var parser = new TestCaseParser(file);

    // Act
    var testCase = await parser.ToTestCaseAsync(default);

    // Assert
    Assert.NotNull(testCase);
    Assert.Equal("TC-010-Authenticated", testCase.Id);
    Assert.Equal("Authenticated", testCase.Title);
    Assert.Equal("Definition", testCase.Type);
    Assert.Equal("Unknown", testCase.Status);
    Assert.Equal(file, testCase.File);
    Assert.True(testCase.IsDefinition);
    Assert.Equal("Home", testCase.Route);
    Assert.EndsWith("TC-001-Login.md", testCase.LinkedFile);
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