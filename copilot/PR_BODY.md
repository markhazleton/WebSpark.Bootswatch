# WebSpark.Bootswatch v1.32.0 - Multi-Framework Upgrade

## ?? Overview

This PR upgrades WebSpark.Bootswatch to support .NET 8.0 (LTS), 9.0 (STS), and 10.0 (Current) through multi-targeting, along with comprehensive testing infrastructure and automated CI/CD pipelines.

## ?? Version Update

- **From**: 1.30.0 (Single framework: .NET 9.0)
- **To**: 1.32.0 (Multi-framework: .NET 8.0, 9.0, 10.0)

## ? Key Features

### Multi-Framework Support
- ? .NET 8.0 (LTS) - Supported until November 2026
- ? .NET 9.0 (STS) - Supported until May 2026
- ? .NET 10.0 (Current) - Latest features
- ? Framework-specific package versions
- ? Optimized builds for each framework

### Testing Infrastructure
- ? Comprehensive test suite (WebSpark.Bootswatch.Tests)
- ? 11 unit tests across 3 test classes
- ? 33 total test executions (11 tests × 3 frameworks)
- ? Framework compatibility tests
- ? PowerShell script for local testing
- ? GitHub Actions workflow for CI/CD

### CI/CD Automation
- ? Automated builds for all frameworks
- ? Matrix strategy testing
- ? Multi-framework package validation
- ? Automated NuGet publishing (tag-triggered)
- ? Automatic GitHub Release creation
- ? Test result summaries

## ?? Changes Summary

### Project Files
- **WebSpark.Bootswatch.csproj**: Multi-targeting configuration
- **WebSpark.Bootswatch.Tests.csproj**: New test project

### Dependencies Updated
- Microsoft.Extensions.FileProviders.Embedded: Framework-specific (8.0.11, 9.0.9, 10.0.0)
- WebSpark.HttpClientUtility: 1.2.0 ? 2.1.1
- Removed: System.Text.RegularExpressions (now in BCL)

### Workflows
- **.github/workflows/dotnet.yml**: Multi-framework builds + automated publishing
- **.github/workflows/multi-framework-tests.yml**: Dedicated testing workflow

### Documentation
- README.md, CHANGELOG.md updated
- New: MULTI_FRAMEWORK_TESTING_SUMMARY.md
- New: GITHUB_ACTIONS_PUBLISHING.md
- New: PUBLISHING_GUIDE.md, READY_TO_PUBLISH.md

## ?? Testing

| Framework | Tests | Status |
|-----------|-------|--------|
| .NET 8.0  | 11/11 | ? PASSED |
| .NET 9.0  | 11/11 | ? PASSED |
| .NET 10.0 | 11/11 | ? PASSED |

## ?? Backward Compatibility

? **100% Backward Compatible** - No breaking changes

## ?? Publishing

After merge, tag to trigger automated publishing:
```bash
git tag -a v1.32.0 -m "Release version 1.32.0"
git push origin v1.32.0
```

GitHub Actions will automatically build, test, and publish to NuGet.org!
