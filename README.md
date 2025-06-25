[![build](https://github.com/thomasduft/testr/actions/workflows/build.yml/badge.svg)](https://github.com/thomasduft/testr/actions/workflows/build.yml) [![NuGet](https://img.shields.io/nuget/vpre/tomware.TestR.svg)](https://www.nuget.org/packages/tomware.TestR)


# testR

A cli tool to manage executable test cases.

## Introduction

`testR` is a command-line tool designed to manage and execute test cases. It supports running test cases against different browsers, validating test case definitions, and creating new test case definitions.

### Vision

The vision of this tool is to have a tool agnostic file based approach to maintain test cases. A common workflow looks like the following:

1. Team creates Use-Cases
2. out of the Use-Cases, Test-Cases will be defined (Test-Case type: Definition) - see [TC-Login-001 Definition sample](samples/Definitions/localhost/TC-Login-001.md)
3. once a feature has been implemented the appropriate Test-Cases can be executed against an environment in an E2E automated manner
  ![Sample Run](TC-001-Login-Run.gif)
4. each run will be historied with Test-Cases known as Runs (Test-Case type: Run) - see [TC-Login-001 Run sample](samples/Runs/localhost/TC-Login-001.md)

> Note: In case of escaping strings please use a backslash `\` followed by a double quote `"` (e.g. Locator=GetByText Text=\\"Invalid login attempt for user 'Albert'\\" Action=IsVisible)

## Installation

To install `testR`, clone the repository and build the project using the .NET CLI:

```sh
git clone https://github.com/thomasduft/testr.git
cd testr
dotnet build
```

For local testing purpose run the `install.sh` script.

In case of installing the tool in an official way use the following command:

> `dotnet tool install -g tomware.TestR`

## Usage

```bash
A cli tool to manage and run executable test cases.

Usage: testR [command] [options]

Options:
  -?|-h|--help  Show help information.

Commands:
  man           Displays a man page for helping with writing the Test-Data syntax within a Test Case.
  playwright    Offers Playwright specific commands.
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
  -tc|--test-case-id       The Test Case ID (e.g. TC-Audit-001).
  -i|--input-directory     The input directory where the Test Case definition is located.
                           Default value is: ..
  -o|--output-directory    The output directory where the Test Case result will be stored.
  --headless               Runs the browser in headless mode.
                           Default value is: False.
  --continue-on-failure    Continues the Test Case execution even if the Test Case fails.
                           Default value is: False.
  -s|--slow                Slows down the execution by the specified amount of milliseconds.
                           Default value is: 500.
  -t|--timeout             Sets the timeout for awaiting the Playwright Locator in milliseconds.
                           Default value is: 10000.
  -bt|--browser-type       Sets the browser type to run the Test Case against (currently supported Browsers: Chrome, Firefox, Webkit).
                           Allowed values are: Chrome, Firefox, Webkit.
                           Default value is: Chrome.
  -rvd|--record-video-dir  Records a video of the Test Case execution to the specified directory.
  -v|--variable            Key-Value based variable used for replacing property values in a Test Step data configuration.
  -?|-h|--help             Show help information.
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

#### Variables Support for Test Data

In order to pass in dynamic or confidential data `testR` supports so called `Variables` in a Test Case Step definition.

A variable is a `Value`-property value starting with a `@`-sign (see below the `@Password`-variable).

```markdown
| 1 | enter password | Locator=GetByLabel Text=Password Action=Fill Value=@Password | password is entered | - |
```

In order to look it up and replace it during execution of the Test Case you need to pass in the appropriate value via the `-v|--variable`-command line argument option.

For the above sample the command line argument option looks like the following:

```bash
  .... -v Password=password ...
```

> Caution: The `Key`-value is case sensitive!

#### OpenTelemetry Support

`testR` includes built-in support for OpenTelemetry (OTLP) to enable observability and monitoring of test case executions. This feature allows you to collect metrics from your test runs and send them to compatible observability platforms.

To enable OpenTelemetry support, use the `--otlp` option when running test cases:

```bash
testR run https://localhost:5001 --test-case-id TC-Login-001 --otlp "http://localhost:9090/api/v1/otlp/v1/metrics"
```

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
