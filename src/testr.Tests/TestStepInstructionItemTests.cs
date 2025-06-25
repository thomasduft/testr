using Microsoft.Playwright;

using tomware.TestR;

namespace testr.Tests;

public class TestStepInstructionItemTests
{
  [Fact]
  public void FromTestStep_WithGetByLabelAndNoValueTestData_ShouldReturnInstance()
  {
    // Arrange
    var step = new TestStep
    {
      Id = 1,
      Description = "Test Description",
      TestData = "Locator=GetByLabel AriaRole=Button Text=ClickMe Value= Action=Click",
      ExpectedResult = "Expected Result",
      IsSuccess = false
    };

    // Act
    var testStepInstructionItem = TestStepInstruction.FromTestStep(step, []);

    // Assert
    Assert.NotNull(testStepInstructionItem);
    Assert.Equal(LocatorType.GetByLabel, testStepInstructionItem.Locator);
    Assert.Equal(AriaRole.Button, testStepInstructionItem.AriaRole);
    Assert.Equal("ClickMe", testStepInstructionItem.Text);
    Assert.Equal(string.Empty, testStepInstructionItem.Value);
    Assert.Equal(ActionType.Click, testStepInstructionItem.Action);
  }

  [Fact]
  public void FromTestStep_WithEscapedValueStringTestData_ShouldReturnInstance()
  {
    // Arrange
    var step = new TestStep
    {
      Id = 1,
      Description = "Test Description",
      TestData = "Locator=GetByText Text=\"Invalid login attempt for user 'Albert'\" Action=IsVisible",
      ExpectedResult = "-",
      IsSuccess = false
    };

    // Act
    var testStepInstructionItem = TestStepInstruction.FromTestStep(step, []);

    // Assert
    Assert.NotNull(testStepInstructionItem);
    Assert.Equal(LocatorType.GetByText, testStepInstructionItem.Locator);
    Assert.Equal("\"Invalid login attempt for user 'Albert'\"", testStepInstructionItem.Text);
    Assert.Equal(ActionType.IsVisible, testStepInstructionItem.Action);
  }

  [Fact]
  public void FromTestStep_WithValueAsVariable_ShouldReturnInstance()
  {
    // Arrange
    Dictionary<string, string> variables = new Dictionary<string, string>
    {
      {"@Password", "my-super-secret"}
    };
    var step = new TestStep
    {
      Id = 1,
      Description = "Test Description",
      TestData = "Locator=GetByLabel Text=Password Action=Fill Value=@Password",
      ExpectedResult = "-",
      IsSuccess = false
    };

    // Act
    var testStepInstructionItem = TestStepInstruction.FromTestStep(step, variables);

    // Assert
    Assert.NotNull(testStepInstructionItem);
    Assert.Equal(LocatorType.GetByLabel, testStepInstructionItem.Locator);
    Assert.Equal(ActionType.Fill, testStepInstructionItem.Action);
    Assert.Equal("Password", testStepInstructionItem.Text);
    Assert.Equal("my-super-secret", testStepInstructionItem.Value);
  }

  [Fact]
  public void FromTestStep_WithUnsupportedTestParameter_ShouldThrowInvalidDataException()
  {
    // Arrange
    var step = new TestStep
    {
      Id = 1,
      Description = "Test Description",
      TestData = "MyParameter=GetByLabel AriaRole=Button Text=ClickMe Action=Click",
      ExpectedResult = "Expected Result",
      IsSuccess = false
    };

    // Act
    // Assert
    Assert.Throws<InvalidDataException>(() => TestStepInstruction.FromTestStep(step, []));
  }

  [Fact]
  public void FromTestStep_WithNoTestParameter_ShouldThrowInvalidDataException()
  {
    // Arrange
    var step = new TestStep
    {
      Id = 1,
      Description = "Test Description",
      ExpectedResult = "Expected Result",
      IsSuccess = false
    };

    // Act
    // Assert
    Assert.Throws<InvalidDataException>(() => TestStepInstruction.FromTestStep(step, []));
  }
}
