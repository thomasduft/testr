# TC-Login-003: Login with wrong Password

- **Date**: 2024-10-17
- **Test Priority**: High
- **Module**: Identity
- **Type**: Definition
- **Status**: Unknown
- **Route**: Login

## Description

The purpose of this test-case is to validate the expected behavior when a user tries to login with a wrong password.

## Preconditions

- The user must be registered in the system.

## Steps

| Step ID | Description            | Test Data                                                                          | Expected Result                                                                                             | Actual Result |
| -------:| ---------------------- | ---------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------- | ------------- |
| 1       | enter username         | Locator=GetByLabel Text=Username Action=Fill Value=admin                           | username admin is entered                                                                                   | -             |
| 2       | enter wrong password   | Locator=GetByLabel Text=Password Action=Fill Value=test                            | password test is entered                                                                                    | -             |
| 3       | click login button     | Locator=GetByRole AriaRole=Button Text=Login Action=Click                          | system validates the user credentials and displays an error message that the tried login attempt is invalid | -             |
| 4       | displays error message | Locator=GetByText Text=\"Invalid login attempt for user 'admin'\" Action=IsVisible | Validation result of invalid login attempt is visible                                                       | -             |

## Postcondition

\-
