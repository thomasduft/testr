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
    var testStepInstructionItem = TestStepInstruction.FromTestStep(step);

    // Assert
    Assert.NotNull(testStepInstructionItem);
    Assert.Equal(LocatorType.GetByLabel, testStepInstructionItem.Locator);
    Assert.Equal(AriaRole.Button, testStepInstructionItem.AriaRole);
    Assert.Equal("ClickMe", testStepInstructionItem.Text);
    Assert.Equal("", testStepInstructionItem.Value);
    Assert.Equal(ActionType.Click, testStepInstructionItem.Action);
  }

  [Fact]
  public void FromTestStep_WithUnsupportedTestParameter_ShouldThrowInvalidDataException()
  {
    // Arrange
    var step = new TestStep
    {
      Id = 1,
      Description = "Test Description",
      TestData = "MyParameter=GetByLabel AriaRole=Button Text=ClickMe Value= Action=Click",
      ExpectedResult = "Expected Result",
      IsSuccess = false
    };

    // Act
    // Assert
    Assert.Throws<InvalidDataException>(() => TestStepInstruction.FromTestStep(step));
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
    Assert.Throws<InvalidDataException>(() => TestStepInstruction.FromTestStep(step));
  }
}