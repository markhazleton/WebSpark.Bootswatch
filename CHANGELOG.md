# Changelog

All notable changes to WebSpark.Bootswatch will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.31.0] - 2025-01-13

### Added
- Multi-framework targeting support for .NET 8.0, 9.0, and 10.0
- Comprehensive multi-framework test suite (WebSpark.Bootswatch.Tests)
- GitHub Actions workflow for separate framework testing
- PowerShell script (`run-multi-framework-tests.ps1`) for local multi-framework testing
- Framework-specific package versions for Microsoft.Extensions.FileProviders.Embedded
- Test documentation in WebSpark.Bootswatch.Tests/README.md
- Multi-framework testing summary documentation

### Changed
- Updated `TargetFramework` to `TargetFrameworks` (net8.0;net9.0;net10.0)
- Microsoft.Extensions.FileProviders.Embedded: 9.0.9 ? Framework-specific versions (8.0.11, 9.0.9, 10.0.0)
- WebSpark.HttpClientUtility: 1.2.0 ? 2.1.1
- Bumped package version from 1.30.0 to 1.31.0
- Updated package release notes with multi-targeting details
- Enhanced README.md with multi-framework documentation

### Removed
- System.Text.RegularExpressions package reference (now included in framework)
- Redundant package dependencies across all target frameworks

### Fixed
- Package version conflicts across different target frameworks
- Transitive dependency issues with multi-targeting

## [1.30.0] - 2025-01-07

### Changed
- Updated all NuGet dependencies to latest stable versions
- Enhanced repository with GitHub NuGet best practices
- Added comprehensive README.md with badges and quick links
- Created community health files (CONTRIBUTING.md, SECURITY.md, CODE_OF_CONDUCT.md)
- Structured changelog documentation
- Professional issue templates for bug reports and feature requests
- Improved documentation organization

### Security
- Updated dependencies to address potential security vulnerabilities
- Enhanced package metadata with security policies

## [1.20.0] - 2025-01-07

### Changed
- Updated Microsoft.Extensions.FileProviders.Embedded to version 9.0.9
- Updated WebSpark.HttpClientUtility to version 1.2.0
- Refreshed package dependencies to latest compatible versions
- Improved package stability and compatibility

### Security
- Applied latest security patches through dependency updates

## [1.10.3] - 2025-05-20

### Fixed
- Minor bug fixes and stability improvements
- Enhanced error handling in theme loading
- Improved logging and diagnostics

### Changed
- Optimized StyleCache initialization performance
- Enhanced HTTP client resilience with better retry policies

## [1.10.0] - 2025-05-15

### Added
- **Bootswatch Theme Switcher Tag Helper** - New `<bootswatch-theme-switcher />` component
- Sample layout file (BootswatchLayout.cshtml) included in NuGet package
- Comprehensive integration documentation
- Theme switching JavaScript component
- Cookie-based theme persistence
- Light/dark mode support with `data-bs-theme` attribute

### Changed
- Improved documentation with step-by-step integration guide
- Enhanced StyleCache with better initialization patterns
- Updated demo application with theme switcher examples

### Fixed
- Static file serving issues with embedded resources
- Theme persistence across page navigations
- JavaScript compatibility with Bootstrap 5

## [1.0.0] - Initial Release

### Added
- Initial release of WebSpark.Bootswatch
- Complete Bootswatch theme integration
- StyleCache service for theme caching
- BootswatchStyleProvider for theme management
- Static file middleware for embedded resources
- Support for all official Bootswatch themes
- Custom theme support (Mom and Texecon themes)
- Bootstrap 5 compatibility
- ASP.NET Core Razor Pages and MVC support
- Comprehensive XML documentation
- Demo application showcasing all features

### Features
- ?? All Bootswatch themes included
- ? High-performance caching
- ?? Easy integration with extension methods
- ?? Responsive design support
- ?? Tag helper support
- ?? Production-ready error handling
- ?? Full IntelliSense documentation

---

## Migration Guides

### Migrating to 1.31.0 from 1.30.0

The 1.31.0 release is fully backward compatible. The main change is the addition of multi-framework support:

#### What's New
- Your project can now target .NET 8.0, 9.0, or 10.0
- NuGet will automatically select the appropriate assembly version
- No code changes required

#### Recommendations
1. Update to the latest version:
   ```bash
   dotnet add package WebSpark.Bootswatch --version 1.31.0
   ```

2. Verify your application runs correctly:
   ```bash
   dotnet build
   dotnet run
   ```

3. Run tests if you have them:
   ```bash
   dotnet test
   ```

### Migrating to 1.30.0 from 1.20.0

No breaking changes. Update packages and verify functionality:

```bash
dotnet add package WebSpark.Bootswatch --version 1.30.0
dotnet add package WebSpark.HttpClientUtility --version 1.2.0
```

### Migrating to 1.10.0 from 1.0.0

The 1.10.0 release introduces the theme switcher tag helper:

1. Add tag helper registration to `_ViewImports.cshtml`:
   ```csharp
   @addTagHelper *, WebSpark.Bootswatch
   ```

2. Use the new theme switcher in your layout:
   ```html
   <bootswatch-theme-switcher />
   ```

3. Update middleware configuration:
   ```csharp
   app.UseBootswatchAll(); // Simplified configuration
   ```

---

## Support

For issues, questions, or contributions:
- ?? **Bug Reports**: [GitHub Issues](https://github.com/MarkHazleton/WebSpark.Bootswatch/issues)
- ?? **Discussions**: [GitHub Discussions](https://github.com/MarkHazleton/WebSpark.Bootswatch/discussions)
- ?? **Email**: mark@markhazleton.com

## Links

- [NuGet Package](https://www.nuget.org/packages/WebSpark.Bootswatch/)
- [GitHub Repository](https://github.com/MarkHazleton/WebSpark.Bootswatch)
- [Demo Site](https://bootswatch.markhazleton.com/)
- [Documentation](https://github.com/MarkHazleton/WebSpark.Bootswatch/wiki)
