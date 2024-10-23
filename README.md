# testR

A cli tool to manage executable test cases.

## Usage

```bash
A cli tool to manage executable test cases.

Usage: testR [command] [options]

Options:
  -?|-h|--help      Show help information.

Commands:
  run               Runs a Test Case definition (i.e. TC-Audit-001 "https://localhost:5001").
  test-case         Creates a new Test Case definition (i.e. test-case TC-Audit-001 "My TestCase Title").
  test-case-result  Copies an existing Test Case definition and creates an execution result (i.e. test-case-result <my-test-case-definition-directory>
                    <my-output-directory> TC-001 <my-execution-status>).
  validate          Validates a Test Case definition (i.e. TC-Audit-001).

Run 'testR [command] -?|-h|--help' for more information about a command.
```

