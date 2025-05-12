# TC-010-Authenticated: Authenticated

- **Date**: 2024-10-17
- **Test Priority**: High
- **Module**: Dashboard
- **Type**: Definition
- **Status**: Unknown
- **Route**: Home

## Description

When authenticated as administrator I see the dashboard with two available workflows.

## Preconditions

- The user administrator must be registered in the system.
- **Link**: [The user must be authenticated](TC-001-Login.md).

## Steps

<!-- STEPS:BEGIN -->
| Step ID | Description            | Test Data                                                      | Expected Result                                                  | Actual Result |
| -------:| ---------------------- | -------------------------------------------------------------- | ---------------------------------------------------------------- | ------------- |
| 1       | enter username         | Locator=GetByLabel Text=Username Action=Fill Value=admin       | username is entered                                              | -             |
| 2       | enter password         | Locator=GetByLabel Text=Password Action=Fill Value=password    | password is entered                                              | -             |
| 3       | click login button     | Locator=GetByRole AriaRole=Button Text=Login Action=Click      | system validates the user credentials and redirects to dashboard | -             |
| 4       | displays the dashboard | Locator=GetByRole AriaRole=Button Text=Logout Action=IsVisible | Logout button visible in the main navigation                     | -             |
<!-- STEPS:END -->

## Postcondition

\-
