# TC-001-Login: Login

- **Date**: 2024-11-20
- **Test Priority**: High
- **Module Name**: Identity
- **Type**: Execution
- **Status**: Passed
- **Route**: Login

## Description

The purpose of this test-case is to satisfy the use-case UC-001-Login which is to authenticate any user of the system.

## Preconditions

- The user must be registered in the system.

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

- The user is authenticated and has an active session.