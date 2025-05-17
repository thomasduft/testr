# Changelog

## [0.6.0] - 2025-05-17

### Added

- Added support for OpenTelemetry.

## [0.5.1] - 2025-03-29

### Fixed

- Updated error messages for test case validation.

## [0.5.0] - 2025-03-26

### Added

- [#1](https://github.com/thomasduft/testr/issues/1): Added `Playwright`-command.

### Changed

- Changed TestCaseType Execution to Run.
- Changed the run command to have the output directory for runs to be optional.

## [0.4.0] - 2025-02-05

### Added

- Added support for .NET 9.0.

## [0.3.1] - 2025-01-23

### Added

- Added Domain property to test run result to indicate against which environment / domain the test case had been executed.

## [0.3.0] - 2025-01-21

### Added

- Added support for picking a file.

## [0.2.1] - 2025-01-18

### Changed

- Changed behavior of how to route to pages before running test steps.

## [0.2.0] - 2024-12-10

### Added

- Added support to align test case runs with same directory structure as test case definitions are maintained. This allows proper markdown links working in test runs as well.

## [0.1.0] - 2024-12-07

### Added

- Added TestCommand and TestResultCommand
- Added github based build workflow (build.yml)
- Added validate command.
- Added test-case command.
- Added run command.
- Added man command.
- Added support to link to Test-Case that needs to be executed as a precondition step.

