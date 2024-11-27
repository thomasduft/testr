[![build](https://github.com/thomasduft/testr/actions/workflows/build.yml/badge.svg)](https://github.com/thomasduft/testr/actions/workflows/build.yml)

# testR

A cli tool to manage executable test cases.

## Introduction

`testR` is a command-line tool designed to manage and execute test cases. It supports running test cases against different browsers, validating test case definitions, and creating new test case definitions.

### Vision

The vision of this tool is to have a tool agnostic file based approach to maintain test cases. A common workflow looks like the following:

1. Team creates Use-Cases
2. out of the Use-Cases Test-Cases will be defined (Test-Case type: Definition) - see [TC-001-Login Definition sample](samples/Definitions/TC-001-Login.md)
3. once a feature has been implemented the appropriate Test-Cases can be executed against an environment in an E2E automated manner
  ![Sample Run](TC-001-Login-Run.gif)
4. each run will be historied with Test-Cases known as Runs (Test-Case type: Execution) - see [TC-001-Login Run sample](samples/Runs/TC-001-Login.md)

> Note: In case of escaping strings please use a backslash `\` followed by a double quote `"` (e.g. Locator=GetByText Text=\\"Invalid login attempt for user 'Albert'\\" Action=IsVisible)

## Installation

To install `testR`, clone the repository and build the project using the .NET CLI:

```sh
git clone https://github.com/thomasduft/testr.git
cd testr
dotnet build
```

For local testing purpose run the `install.sh` script.

> Note: Will eventually be available as a dotnet tool on [NuGet.org](https://www.nuget.org/).

## Usage

```bash
A cli tool to manage and run executable test cases.

Usage: testR [command] [options]

Options:
  -?|-h|--help  Show help information.

Commands:
  run           Runs Test Case definitions (i.e. "https://localhost:5001" -tc TC-Audit-001).
  test-case     Creates a new Test Case definition (i.e. test-case TC-Audit-001 "My TestCase Title").
  validate      Validates a Test Case definition (i.e. TC-Audit-001).

Run 'testR [command] -?|-h|--help' for more information about a command.
```

### Commands

#### run

Runs Test Case definitions.

```sh
testR run [domain] [options]

Options:
  -tc|--test-case-id            A specific Test Case ID to run.
  -i|--input-directory <DIR>    The input directory where the Test Case definition is located. (default: .)
  -o|--output-directory <DIR>   The output directory where the Test Case result will be stored. (default: .)
  --headless                    Runs the browser in headless mode.
  --continue-on-failure         Continues execution even if a test step fails.
  -s|--slow <MS>                Sets the slow motion delay in milliseconds.
  -t|--timeout <MS>             Sets the timeout for awaiting the Playwright Locator in milliseconds. (default: 30000)
  -bt|--browser-type <TYPE>     Sets the browser type to run the Test Case against (Chrome, Firefox, Webkit). (default: Chrome)
  -rvd|--record-video-dir <DIR> Records a video of the Test Case execution to the specified directory.
```

#### test-case

Creates a new Test Case definition.

```sh
testR test-case [test-case-id] [title]
```

#### validate

Validates a Test Case definition.

```sh
testR validate [test-case-id] [options]

Options:
  -i|--input-directory    The input directory where the Test Case definition is located. (default: .)
```

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.