# TC-Dashboard-100: Dashboard as administrator

- **Date**: 2024-10-17
- **Test Priority**: High
- **Module**: Dashboard
- **Type**: Definition
- **Status**: Unknown
- **Route**: home

## Description

When authenticated as administrator I see the dashboard with two available workflows.

## Preconditions

- The user administrator must be registered in the system.
- **Link**: [The administrator must be authenticated](../TC-Login-001.md).

## Steps

| Step ID | Description            | Test Data                                                      | Expected Result              | Actual Result |
| -------:| ---------------------- | -------------------------------------------------------------- | -----------------------------| ------------- |
| 1       | nav Holiday            | Locator=GetByRole AriaRole=Link Text=HOLIDAY Action=IsVisible  | Holiday nav item is visible  | -             |
| 2       | nav Issue              | Locator=GetByRole AriaRole=Link Text=ISSUE Action=IsVisible    | Issue nav item is visible    | -             |

## Postcondition

\-
