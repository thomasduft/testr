using tomware.TestR;

namespace testr.Tests;

public class MarkdownTableParserTests
{
  [Fact]
  public void ParseTestSteps_WithValidTable_ShouldReturnTestSteps()
  {
    // Arrange
    var markdown = @"
# Test Case

## Steps

<!-- STEPS:BEGIN -->
| Step ID | Description            | Test Data                                                      | Expected Result                                                  | Actual Result |
| -------:| ---------------------- | -------------------------------------------------------------- | ---------------------------------------------------------------- | ------------- |
| 1       | enter username         | Locator=GetByLabel Text=Username Action=Fill Value=admin       | username is entered                                              | -             |
| 2       | enter password         | Locator=GetByLabel Text=Password Action=Fill Value=@Password   | password is entered                                              | -             |
| 3       | click login button     | Locator=GetByRole AriaRole=Button Text=Login Action=Click      | system validates the user credentials and redirects to dashboard | -             |
| 4       | displays the dashboard | Locator=GetByRole AriaRole=Button Text=Logout Action=IsVisible | Logout button visible in the main navigation                     | -             |
<!-- STEPS:END -->

## Postcondition
";

    var parser = new MarkdownTableParser(markdown);

    // Act
    var testSteps = parser.ParseTestSteps().ToList();

    // Assert
    Assert.Equal(4, testSteps.Count);

    var firstStep = testSteps[0];
    Assert.Equal(1, firstStep.Id);
    Assert.Equal("enter username", firstStep.Description);
    Assert.Equal("Locator=GetByLabel Text=Username Action=Fill Value=admin", firstStep.TestData);
    Assert.Equal("username is entered", firstStep.ExpectedResult);
  }

  [Fact]
  public void ParseTestSteps_WithEscapedQuotes_ShouldHandleCorrectly()
  {
    // Arrange
    var markdown = @"
<!-- STEPS:BEGIN -->
| Step ID | Description | Test Data | Expected Result | Actual Result |
| -------:| ----------- | --------- | --------------- | ------------- |
| 1       | test escaped | Locator=GetByText Text=""Invalid login attempt for user 'Albert'"" Action=IsVisible | shows error message | - |
<!-- STEPS:END -->
";

    var parser = new MarkdownTableParser(markdown);

    // Act
    var testSteps = parser.ParseTestSteps().ToList();

    // Assert
    Assert.Single(testSteps);
    Assert.Equal("Locator=GetByText Text=\"Invalid login attempt for user 'Albert'\" Action=IsVisible", testSteps[0].TestData);
  }

  [Fact]
  public void ParseTestSteps_WithSuccessFailureIndicators_ShouldSetIsSuccess()
  {
    // Arrange
    var markdown = @"
<!-- STEPS:BEGIN -->
| Step ID | Description | Test Data | Expected Result | Actual Result |
| -------:| ----------- | --------- | --------------- | ------------- |
| 1       | success step | test data | expected | ✅ |
| 2       | failed step  | test data | expected | ❌ error message |
| 3       | pending step | test data | expected | - |
<!-- STEPS:END -->
";

    var parser = new MarkdownTableParser(markdown);

    // Act
    var testSteps = parser.ParseTestSteps().ToList();

    // Assert
    Assert.Equal(3, testSteps.Count);
    Assert.True(testSteps[0].IsSuccess);   // ✅ indicates success
    Assert.False(testSteps[1].IsSuccess);  // ❌ indicates failure
    Assert.False(testSteps[2].IsSuccess);  // - indicates not run yet (considered failure)
  }

  [Fact]
  public void ParseTestSteps_WithNoStepsSection_ShouldReturnEmpty()
  {
    // Arrange
    var markdown = @"
# Test Case

## Description
Some content without steps section.
";

    var parser = new MarkdownTableParser(markdown);

    // Act
    var testSteps = parser.ParseTestSteps().ToList();

    // Assert
    Assert.Empty(testSteps);
  }

  [Fact]
  public void ParseTestSteps_WithInvalidTableRow_ShouldSkipInvalidRows()
  {
    // Arrange
    var markdown = @"
<!-- STEPS:BEGIN -->
| Step ID | Description   | Test Data | Expected Result | Actual Result |
| -------:| ------------- | --------- | --------------- | ------------- |
| 1       | valid step    | test data | expected        | -             |
| invalid | invalid id    | test data | expected        | -             |
| 2       | another valid | test data | expected        | -             |
<!-- STEPS:END -->
";

    var parser = new MarkdownTableParser(markdown);

    // Act
    var testSteps = parser.ParseTestSteps().ToList();

    // Assert
    Assert.Equal(2, testSteps.Count);
    Assert.Equal(1, testSteps[0].Id);
    Assert.Equal(2, testSteps[1].Id);
  }

  [Fact]
  public void ParseTestSteps_WithMissingColumns_ShouldIgnoreMissingColumns()
  {
    // Arrange
    var markdown = @"
<!-- STEPS:BEGIN -->
| Step ID | Description      | Test Data | Expected Result | Actual Result |
| -------:| ---------------- | --------- | --------------- | ------------- |
| 1       | complete step    | test data | expected        | -             |
| 2       | incomplete       |           |                 |
| 3       | another complete | test data | expected        | -             |
<!-- STEPS:END -->
";

    var parser = new MarkdownTableParser(markdown);

    // Act
    var testSteps = parser.ParseTestSteps().ToList();

    // Assert
    Assert.Equal(3, testSteps.Count);
    Assert.Equal(1, testSteps[0].Id);
    Assert.Equal(2, testSteps[1].Id);
    Assert.Equal(3, testSteps[2].Id);
  }

  [Fact]
  public void ParseTestSteps_WithDifferentCommentFormatting_ShouldWork()
  {
    // Arrange
    var markdown = @"
<!--STEPS:BEGIN-->
| Step ID | Description | Test Data | Expected Result | Actual Result |
| -------:| ----------- | --------- | --------------- | ------------- |
| 1       | test step   | test data | expected        | -             |
<!--   STEPS:END   -->
";

    var parser = new MarkdownTableParser(markdown);

    // Act
    var testSteps = parser.ParseTestSteps().ToList();

    // Assert
    Assert.Single(testSteps);
    Assert.Equal(1, testSteps[0].Id);
  }

  [Fact]
  public void ParseTestSteps_WithNewlineBeforeTable_ShouldWork()
  {
    // Arrange
    var markdown = @"
<!-- STEPS:BEGIN -->

| Step ID | Description | Test Data | Expected Result | Actual Result |
| -------:| ----------- | --------- | --------------- | ------------- |
| 1       | test step 1 | test data 1 | expected 1    | -             |
| 2       | test step 2 | test data 2 | expected 2    | -             |
<!-- STEPS:END -->
";

    var parser = new MarkdownTableParser(markdown);

    // Act
    var testSteps = parser.ParseTestSteps().ToList();

    // Assert
    Assert.Equal(2, testSteps.Count);
    Assert.Equal(1, testSteps[0].Id);
    Assert.Equal("test step 1", testSteps[0].Description);
    Assert.Equal(2, testSteps[1].Id);
    Assert.Equal("test step 2", testSteps[1].Description);
  }

  [Fact]
  public void ParseTestSteps_WithNewlineAfterTable_ShouldWork()
  {
    // Arrange
    var markdown = @"
<!-- STEPS:BEGIN -->
| Step ID | Description | Test Data | Expected Result | Actual Result |
| -------:| ----------- | --------- | --------------- | ------------- |
| 1       | test step 1 | test data 1 | expected 1    | -             |
| 2       | test step 2 | test data 2 | expected 2    | -             |

<!-- STEPS:END -->
";

    var parser = new MarkdownTableParser(markdown);

    // Act
    var testSteps = parser.ParseTestSteps().ToList();

    // Assert
    Assert.Equal(2, testSteps.Count);
    Assert.Equal(1, testSteps[0].Id);
    Assert.Equal("test step 1", testSteps[0].Description);
    Assert.Equal(2, testSteps[1].Id);
    Assert.Equal("test step 2", testSteps[1].Description);
  }
}
