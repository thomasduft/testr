# TC-Documents-001: Upload File

- **Date**: 2024-11-21
- **Author**: Thomas Duft
- **Test Priority**: High
- **Module**: Documents
- **Type**: Definition
- **Status**: Unknown
- **Route**: practice-file-upload

## Description

The purpose of this test-case is to test the upload of a file to the system.

## Preconditions

- no preconditions

## Steps

| Step ID | Description                   | Test Data                                                                       | Expected Result                           | Actual Result |
| -------:| ----------------------------- | ------------------------------------------------------------------------------- | ----------------------------------------- | ------------- |
| 1       | File selector visible         | Locator=GetByTestId Text=file-input Action=IsVisible                            | File selector is visible                  |               |
| 2       | Select file                   | Locator=GetByTestId Text=file-input Action=PickFiles Value=../../samples/Definitions/commitquality/TC-Documents-001.md | File selected           |               |
| 3       | Upload file                   | Locator=GetByRole AriaRole=Button Text=Submit Action=Click                      | Submit button clicked and file uploaded   |               |

## Postcondition

- no postcondition
