# TC-100-Dashboard: Dashboard as administrator

- **Date**: 2024-11-29
- **Test Priority**: High
- **Module Name**: Dashboard
- **Type**: Execution
- **Status**: Passed
- **Route**: home

## Description

When authenticated as administrator I see the dashboard with two available workflows.

## Preconditions

- The user administrator must be registered in the system.
- **Link**: [The administrator must be authenticated](TC-001-Login.md).

## Steps

<!-- STEPS:BEGIN -->
| Step ID | Description            | Test Data                                                      | Expected Result              | Actual Result |
| -------:| ---------------------- | -------------------------------------------------------------- | -----------------------------| ------------- |
| 1       | nav Holiday            | Locator=GetByRole AriaRole=Link Text=HOLIDAY Action=IsVisible  | Holiday nav item is visible  | ✅ |
| 2       | nav Issue              | Locator=GetByRole AriaRole=Link Text=ISSUE Action=IsVisible    | Issue nav item is visible    | ✅ |
<!-- STEPS:END -->

## Postcondition

\-
