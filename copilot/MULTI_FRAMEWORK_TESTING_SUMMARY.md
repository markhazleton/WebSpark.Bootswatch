# Multi-Framework Testing & Multi-Targeting - Complete Guide

## Overview

WebSpark.Bootswatch now supports multiple .NET frameworks through multi-targeting, allowing the library to run on .NET 8.0 (LTS), 9.0 (STS), and 10.0 (Current). This document explains the implementation, testing strategy, and benefits of this approach.

## Multi-Targeting Implementation

### Package Structure

The NuGet package contains separate assemblies for each target framework:

```
WebSpark.Bootswatch.1.31.0.nupkg
??? lib/
?   ??? net8.0/
?   ?   ??? WebSpark.Bootswatch.dll
?   ?   ??? WebSpark.Bootswatch.xml
?   ??? net9.0/
?   ?   ??? WebSpark.Bootswatch.dll
?   ?   ??? WebSpark.Bootswatch.xml
?   ??? net10.0/
?       ??? WebSpark.Bootswatch.dll
?       ??? WebSpark.Bootswatch.xml
```

### Project Configuration

**WebSpark.Bootswatch.csproj:**
```xml
<PropertyGroup>
  <TargetFrameworks>net8.0;net9.0;net10.0</TargetFrameworks>
</PropertyGroup>

<!-- Framework-specific package references -->
<ItemGroup Condition="'$(TargetFramework)' == 'net8.0'">
  <PackageReference Include="Microsoft.Extensions.FileProviders.Embedded" Version="8.0.11" />
</ItemGroup>

<ItemGroup Condition="'$(TargetFramework)' == 'net9.0'">
  <PackageReference Include="Microsoft.Extensions.FileProviders.Embedded" Version="9.0.9" />
</ItemGroup>

<ItemGroup Condition="'$(TargetFramework)' == 'net10.0'">
  <PackageReference Include="Microsoft.Extensions.FileProviders.Embedded" Version="10.0.0" />
</ItemGroup>
```

## Testing Infrastructure

### Test Project Setup

**WebSpark.Bootswatch.Tests.csproj:**
```xml
<PropertyGroup>
  <TargetFrameworks>net8.0;net9.0;net10.0</TargetFrameworks>
  <IsTestProject>true</IsTestProject>
</PropertyGroup>

<ItemGroup>
  <PackageReference Include="xunit" Version="2.9.2" />
  <PackageReference Include="xunit.runner.visualstudio" Version="2.8.2" />
  <PackageReference Include="FluentAssertions" Version="7.0.0" />
  <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.11.1" />
</ItemGroup>

<!-- Framework-specific test dependencies -->
<ItemGroup Condition="'$(TargetFramework)' == 'net8.0'">
  <PackageReference Include="Microsoft.AspNetCore.Mvc.Testing" Version="8.0.11" />
</ItemGroup>
<!-- Similar for net9.0 and net10.0 -->
```

### Test Classes

#### 1. Framework Compatibility Tests
Verifies runtime framework version and core functionality:

```csharp
[Fact]
public void TargetFramework_ShouldBeCorrect()
{
    var runtimeVersion = RuntimeInformation.FrameworkDescription;
    var version = Environment.Version;
    version.Major.Should().BeOneOf(8, 9, 10);
}
```

#### 2. StyleModel Tests
Tests data model functionality across frameworks:

```csharp
[Theory]
[InlineData("cerulean", "Cerulean")]
[InlineData("cosmo", "Cosmo")]
public void StyleModel_ShouldStore_ThemeNames(string themeName, string displayName)
{
    var styleModel = new StyleModel
    {
        name = themeName,
        description = displayName
    };
    
    styleModel.name.Should().Be(themeName);
}
```

#### 3. StyleCache Tests
Validates service initialization and caching across frameworks:

```csharp
[Fact]
public void StyleCache_ShouldInitialize()
{
    var serviceProvider = CreateServiceProvider();
    var styleCache = new StyleCache(serviceProvider);
    styleCache.Should().NotBeNull();
}
```

## Running Tests

### Command Line

#### Test All Frameworks
```bash
dotnet test
```

#### Test Specific Framework
```bash
# .NET 8.0
dotnet test --framework net8.0

# .NET 9.0
dotnet test --framework net9.0

# .NET 10.0
dotnet test --framework net10.0
```

#### Test with Configuration
```bash
dotnet test --framework net8.0 --configuration Release --logger "console;verbosity=detailed"
```

### PowerShell Script

The `run-multi-framework-tests.ps1` script provides enhanced testing capabilities:

```powershell
# Test all frameworks with detailed output
.\run-multi-framework-tests.ps1

# Test specific framework
.\run-multi-framework-tests.ps1 -Framework net8.0

# Test with Release configuration
.\run-multi-framework-tests.ps1 -Configuration Release
```

**Script Features:**
- ? Colored console output (success, error, info)
- ? Framework-specific test results
- ? Summary matrix showing pass/fail per framework
- ? Individual `.trx` files per framework
- ? Build verification before testing
- ? Detailed error reporting

## CI/CD Integration

### GitHub Actions Workflow

The `.github/workflows/multi-framework-tests.yml` workflow provides comprehensive CI testing:

```yaml
jobs:
  test-net8:
    name: Test .NET 8.0
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '8.0.x'
      - run: dotnet test --framework net8.0
      - uses: actions/upload-artifact@v4
        with:
          name: test-results-net8
          path: '**/test-results-net8.trx'
```

**Workflow Features:**
- ? Separate job for each framework
- ? Parallel execution across frameworks
- ? Test result artifacts per framework
- ? Summary matrix in GitHub Actions UI
- ? Automatic triggering on push/PR
- ? Runs on `main` and feature branches

### Test Result Matrix

GitHub Actions displays a matrix showing results:

| Framework | Build | Tests | Status |
|-----------|-------|-------|--------|
| .NET 8.0  | ?    | 11/11 | ? PASSED |
| .NET 9.0  | ?    | 11/11 | ? PASSED |
| .NET 10.0 | ?    | 11/11 | ? PASSED |

## Test Results

### Current Status

All tests pass on all three target frameworks:

```
Framework  | Tests | Passed | Failed | Duration
-----------|-------|--------|--------|----------
.NET 8.0   | 11    | 11     | 0      | 0.8s
.NET 9.0   | 11    | 11     | 0      | 1.0s
.NET 10.0  | 11    | 11     | 0      | 0.9s
```

### Test Coverage

- **Framework Compatibility**: 3 tests
- **StyleModel**: 5 tests
- **StyleCache**: 3 tests

**Total**: 11 tests × 3 frameworks = 33 test executions per run

## Benefits of Multi-Framework Support

### For Library Maintainers

? **Early Issue Detection**: Catch framework-specific bugs before release  
? **API Compatibility**: Verify APIs work consistently across frameworks  
? **Performance Comparison**: Compare performance across .NET versions  
? **Future-Proofing**: Ready for new framework releases  
? **Quality Assurance**: Comprehensive testing coverage  

### For Library Consumers

? **Flexibility**: Choose the .NET version that fits your needs  
? **LTS Support**: .NET 8.0 support until November 2026  
? **Latest Features**: Access .NET 10.0 improvements  
? **No Breaking Changes**: Smooth upgrades between versions  
? **Optimized Binaries**: Framework-specific optimizations  

### For Applications

? **No Lock-In**: Upgrade .NET version independently  
? **Backward Compatible**: Existing code continues to work  
? **Performance**: Get framework-specific optimizations  
? **Security**: Access latest security patches  
? **Support Windows**: Extended support lifecycle  

## Framework-Specific Considerations

### .NET 8.0 (LTS)
- **Support Until**: November 2026
- **Best For**: Production applications requiring long-term stability
- **Package Version**: Microsoft.Extensions.FileProviders.Embedded 8.0.11

### .NET 9.0 (STS)
- **Support Until**: May 2026
- **Best For**: Applications wanting recent features with standard support
- **Package Version**: Microsoft.Extensions.FileProviders.Embedded 9.0.9

### .NET 10.0 (Current)
- **Support Until**: TBD (STS expected)
- **Best For**: New applications wanting latest features
- **Package Version**: Microsoft.Extensions.FileProviders.Embedded 10.0.0

## Adding New Tests

### Framework-Agnostic Tests

Tests that should behave identically across frameworks:

```csharp
[Fact]
public void MyFeature_ShouldWork_OnAllFrameworks()
{
    // Arrange
    var feature = new MyFeature();
    
    // Act
    var result = feature.DoSomething();
    
    // Assert
    result.Should().NotBeNull();
}
```

### Framework-Specific Tests

Tests that verify framework-specific behavior:

```csharp
[Fact]
public void MyFeature_ShouldUse_FrameworkSpecificOptimization()
{
    // Arrange
    _output.WriteLine($"Testing on {RuntimeInformation.FrameworkDescription}");
    
    // Act & Assert based on framework version
    var version = Environment.Version.Major;
    if (version >= 10)
    {
        // Verify .NET 10+ specific behavior
    }
}
```

## Troubleshooting

### Common Issues

#### Package Version Conflicts
**Problem**: NuGet reports package downgrade warnings

**Solution**: Use framework-specific package references:
```xml
<ItemGroup Condition="'$(TargetFramework)' == 'net8.0'">
  <PackageReference Include="PackageName" Version="8.0.x" />
</ItemGroup>
```

#### Test Discovery Failures
**Problem**: Tests not discovered for specific framework

**Solution**: Ensure test project targets the framework:
```xml
<TargetFrameworks>net8.0;net9.0;net10.0</TargetFrameworks>
```

#### Build Failures
**Problem**: Build fails for one framework but not others

**Solution**: 
1. Build each framework separately to isolate issue
2. Check framework-specific package compatibility
3. Review conditional compilation symbols if used

## Best Practices

### 1. Test Isolation
- Each framework runs in its own environment
- No shared state between framework test runs
- Independent package resolution

### 2. Consistent Test Coverage
- All tests should run on all frameworks
- Use `[Theory]` for data-driven tests
- Avoid framework-specific conditionals unless necessary

### 3. CI/CD Strategy
- Run separate jobs for each framework
- Upload framework-specific artifacts
- Display results in summary matrix

### 4. Version Management
- Keep framework-specific packages aligned
- Update all frameworks together
- Test after package updates

### 5. Documentation
- Document framework-specific behaviors
- Provide migration guides
- Update changelog with framework changes

## Performance Considerations

### Build Times
- Multi-targeting increases build time (~3x)
- CI parallelization mitigates impact
- Local builds can target specific framework

### Test Execution
- Tests run sequentially per framework
- Total test time = (test time) × (framework count)
- Parallel CI jobs reduce wall-clock time

### Package Size
- NuGet package contains all framework assemblies
- Size increase: ~2-3x single framework
- NuGet client downloads only needed version

## Future Plans

### Planned Enhancements
- [ ] Add benchmarking across frameworks
- [ ] Performance comparison reports
- [ ] Framework feature detection utilities
- [ ] Automated migration guides

### Framework Support Policy
- Support current and previous 2 LTS versions
- Add new frameworks within 30 days of release
- Drop support 6 months after EOL

## Resources

### Documentation
- [Test Project README](WebSpark.Bootswatch.Tests/README.md)
- [Main README](README.md)
- [Changelog](CHANGELOG.md)

### Related Files
- `WebSpark.Bootswatch/WebSpark.Bootswatch.csproj` - Multi-targeting configuration
- `WebSpark.Bootswatch.Tests/WebSpark.Bootswatch.Tests.csproj` - Test project configuration
- `.github/workflows/multi-framework-tests.yml` - CI workflow
- `run-multi-framework-tests.ps1` - Local testing script

### External References
- [.NET Multi-Targeting](https://docs.microsoft.com/en-us/dotnet/standard/frameworks)
- [.NET Support Policy](https://dotnet.microsoft.com/platform/support/policy)
- [xUnit Multi-Targeting](https://xunit.net/docs/multi-targeting)

---

## Summary

Multi-framework support provides:

? **Compatibility** - Works with .NET 8.0, 9.0, and 10.0  
? **Quality** - Comprehensive testing across all frameworks  
? **Flexibility** - Consumers choose their .NET version  
? **Performance** - Framework-specific optimizations  
? **Future-Ready** - Easy to add new framework versions  

For questions or issues, please visit our [GitHub repository](https://github.com/MarkHazleton/WebSpark.Bootswatch).
