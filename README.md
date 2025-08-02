[![build](https://github.com/thomasduft/testr/actions/workflows/build.yml/badge.svg)](https://github.com/thomasduft/testr/actions/workflows/build.yml) [![NuGet](https://img.shields.io/nuget/vpre/tomware.testr.svg)](https://www.nuget.org/packages/tomware.testr)

# testr

A command-line tool for managing and executing automated test cases with browser support.

## Introduction

`testr` is a powerful CLI tool designed to streamline test case management and execution. It provides comprehensive support for running automated end-to-end tests across multiple browsers, validating test case definitions, and creating new test cases with a file-based approach.

### Key Features

- 🌐 **Multi-browser support** - Run tests on Chrome, Firefox, and WebKit
- 📝 **File-based test definitions** - Maintain test cases as markdown files
- 🔄 **Test execution history** - Track and store test run results
- 🧪 **Dynamic variables** - Support for parameterized test data
- 📊 **OpenTelemetry integration** - Built-in observability and monitoring
- ✅ **Test validation** - Validate test case definitions before execution

## Vision & Workflow

The vision of `testr` is to provide a tool-agnostic, file-based approach to test case management. The typical workflow follows these steps:

1. **Define Use Cases** - Teams create high-level use case documentation
2. **Create Test Case Definitions** - Convert use cases into executable test case definitions
   📄 [Example: TC-Login-001 Definition](samples/Definitions/localhost/TC-Login-001.md)
3. **Execute Tests** - Run automated end-to-end tests against target environments
   ![Sample Test Run](TC-001-Login-Run.gif)
4. **Store Results** - Each execution creates a historical test run record
   📄 [Example: TC-Login-001 Run](samples/Runs/localhost/TC-Login-001.md)

> **Note:** When escaping strings in test data, use a backslash `\` followed by a double quote `"`. For example:
> `Locator=GetByText Text=\\"Invalid login attempt for user 'Albert'\\" Action=IsVisible`

## Installation

### Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) or later

### Option 1: Install from NuGet (Recommended)

Install `testr` globally using the .NET tool command:

```bash
dotnet tool install -g tomware.testr
```

### Option 2: Build from Source

Clone the repository and build the project:

```bash
git clone https://github.com/thomasduft/testr.git
cd testr
dotnet build
```

For local development and testing, run the installation script:

```bash
./install.sh
```

## Usage

### Quick Start

```bash
# Create a new test case
testr test-case TC-Login-001 "User Login Test"

# Validate a test case definition
testr validate TC-Login-001

# Run a test case
testr run https://localhost:5001 --test-case-id TC-Login-001
```

### Command Overview

```bash
A cli tool to manage and run executable test cases.

Usage: testr [command] [options]

Options:
  -?|-h|--help  Show help information.

Commands:
  man           Display syntax help for writing test data within test cases
  playwright    Playwright-specific commands and utilities
  run           Execute test case definitions
  test-case     Create new test case definitions
  validate      Validate existing test case definitions

Run 'testr [command] -?|-h|--help' for more information about a command.
```

## Detailed Command Reference

### `run` - Execute Test Cases

Execute test case definitions against target environments.

```bash
testr run [domain] [options]
```

**Options:**
- `-tc|--test-case-id` - Test Case ID to execute (e.g., `TC-Audit-001`)
- `-i|--input-directory` - Directory containing test case definitions (default: `.`)
- `-o|--output-directory` - Directory for storing test results
- `--headless` - Run browser in headless mode (default: `false`)
- `--continue-on-failure` - Continue execution on test failures (default: `false`)
- `-s|--slow` - Slow down execution by specified milliseconds (default: `500`)
- `-t|--timeout` - Playwright locator timeout in milliseconds (default: `10000`)
- `-bt|--browser-type` - Browser to use: `Chrome`, `Firefox`, `Webkit` (default: `Chrome`)
- `-rvd|--record-video-dir` - Directory for recording test execution videos
- `-v|--variable` - Define variables for test data replacement
- `--otlp` - OpenTelemetry endpoint for metrics collection

**Example:**
```bash
testr run https://localhost:5001 \
  --test-case-id TC-Login-001 \
  --browser-type Chrome \
  --record-video-dir ./videos \
  --variable Username=testuser --variable Password=secret123
```

### `test-case` - Create Test Cases

Create new test case definition files.

```bash
testr test-case [test-case-id] [title]
```

**Example:**
```bash
testr test-case TC-Registration-001 "User Registration Flow"
```

### `validate` - Validate Test Cases

Validate the syntax and structure of test case definitions.

```bash
testr validate [test-case-id] [options]
```

**Options:**
- `-i|--input-directory` - Directory containing test case definitions (default: `.`)

**Example:**
```bash
testr validate TC-Login-001 --input-directory ./test-definitions
```

## Advanced Features

### Variable Support

Use variables to inject dynamic or sensitive data into test cases. Variables are defined in test steps using the `@` prefix:

```markdown
| 1 | enter password | Locator=GetByLabel Text=Password Action=Fill Value=@Password | password is entered | - |
```

Pass variable values at runtime:

```bash
testr run https://localhost:5001 \
  --test-case-id TC-Login-001 \
  --variable Password=mysecretpassword \
  --variable Username=testuser
```

> ⚠️ **Important:** Variable keys are case-sensitive!

### OpenTelemetry Integration

Enable comprehensive observability and monitoring for your test executions:

```bash
testr run https://localhost:5001 \
  --test-case-id TC-Login-001 \
  --otlp "http://localhost:9090/api/v1/otlp/v1/metrics"
```

This allows you to:
- Track test execution metrics
- Monitor performance trends
- Integrate with observability platforms like Prometheus and Grafana

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

---

**Happy Testing!** 🚀
