# WebSpark.Bootswatch.Tests

Multi-framework test project for WebSpark.Bootswatch library.

## Overview

This test project validates the WebSpark.Bootswatch library across multiple .NET frameworks:
- **.NET 8.0** (LTS)
- **.NET 9.0** (STS)
- **.NET 10.0** (Current)

## Running Tests

### All Frameworks

```powershell
# Run tests for all frameworks
dotnet test
```

### Specific Framework

```powershell
# Test .NET 8.0
dotnet test --framework net8.0

# Test .NET 9.0
dotnet test --framework net9.0

# Test .NET 10.0
dotnet test --framework net10.0
```

### Using PowerShell Script

```powershell
# Test all frameworks with detailed output
.\run-multi-framework-tests.ps1

# Test specific framework
.\run-multi-framework-tests.ps1 -Framework net8.0

# Test with Release configuration
.\run-multi-framework-tests.ps1 -Configuration Release
```

## Test Categories

### Framework Compatibility Tests
Verify that the library initializes and functions correctly across all target frameworks.

### StyleModel Tests
Test the StyleModel class functionality for storing theme information.

### StyleCache Tests
Validate the StyleCache service initialization and theme retrieval across frameworks.

## CI/CD Integration

The `.github/workflows/multi-framework-tests.yml` workflow runs tests separately for each framework:
- Each framework has its own job
- Test results are uploaded as artifacts
- Summary matrix shows pass/fail status for each framework

## Test Results

Test results are stored in the `TestResults` directory with framework-specific filenames:
- `test-results-net8.0.trx`
- `test-results-net9.0.trx`
- `test-results-net10.0.trx`
