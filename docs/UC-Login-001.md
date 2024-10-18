### UC-001-Login

- **Date**: 2024-10-17
- **Author**: Thomas Duft
- **Scope**: Identity
- **Type**: Primary
- **Actor**: User

#### Description

The purpose of this use case is to authenticate any user of the system.

#### Pre-Conditions

The user must be registered in the system.

#### Basic Flow

1. The user selects to login to the system.
2. THe system presents the login form.
3. The user enters the username and password.
4. The user submits the form.
5. The system validates the user credentials.
6. The systems initiates an active session for the user.
7. The system presents a login confirmation.

#### Alternative / Exception Flows

- 2a - Remember Me
    - The system detect that the user's computer has a saved username and password.
    - The system bypasses step 4 and 4.
    - Continue with Basic Flow, step 5.
- 5a - Username does not exist
    - In step2, system determines that the username does not match an active user profile.
    - The system present an error message.
    - Use case ends.
- 5b - Invalid password
    - In step 2, system determines that the password does not match the username provided.
    - The system present an error message.
    - Use case ends.

#### Post-Conditions

The user is authenticated and has an active session.
