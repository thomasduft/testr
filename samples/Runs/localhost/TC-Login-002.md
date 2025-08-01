# TC-Login-002: Login with wrong or not existing Username

- **Date**: 2025-08-02
- **Test Priority**: High
- **Module**: Identity
- **Type**: Run
- **Status**: Passed
- **Domain**: https://localhost:5001
- **Route**: Login

## Description

The purpose of this test-case is to validate the expected behavior when a user tries to login with a wrong or not existing username.

## Preconditions

- The user must not be registered in the system.

## Steps

| Step ID | Description            | Test Data                                                                           | Expected Result                                                                                             | Actual Result |
| -------:| ---------------------- | ----------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------- | ------------- |
| 1 | enter username | Locator=GetByLabel Text=Username Action=Fill Value=Albert | username Albert is entered | ✅ |
| 2 | enter password | Locator=GetByLabel Text=Password Action=Fill Value=@Password | password is entered | ✅ |
| 3 | click login button | Locator=GetByRole AriaRole=Button Text=Login Action=Click | system validates the user credentials and displays an error message that the tried login attempt is invalid | ✅ |
| 4 | displays error message | Locator=GetByText Text=\"Invalid login attempt for user 'Albert'\" Action=IsVisible | Validation result of invalid login attempt is visible | ✅ |

## Postcondition

\-

