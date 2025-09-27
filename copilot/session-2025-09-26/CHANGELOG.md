# Changelog

All notable changes to WebSpark.Bootswatch will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/), and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [1.30.0] - 2025-09-26

### Added
- Enhanced README.md with GitHub NuGet repository best practices
- Comprehensive contributing guidelines (CONTRIBUTING.md)
- Security policy and vulnerability reporting process (SECURITY.md)
- Code of conduct for community standards (CODE_OF_CONDUCT.md)
- Structured repository organization with `/copilot/` directory
- Professional GitHub issue templates for bug reports and feature requests
- Complete community health files for better project governance
- Professional badges and visual elements in README
- Architecture documentation and troubleshooting guides
- Browser compatibility matrix and performance metrics

### Changed
- Improved repository structure and documentation organization
- Enhanced copilot instructions with NuGet package best practices
- Restructured README.md with comprehensive sections and professional formatting
- Organized all copilot-generated content into session-based folders
- Enhanced developer experience with clear installation and integration guides

### Documentation
- Professional repository presentation following GitHub best practices
- Complete contribution workflow and guidelines
- Security-first approach with vulnerability reporting process
- Comprehensive API documentation and usage examples
- Enhanced troubleshooting and support resources

### Note
- No breaking changes; all integration steps remain the same as previous versions
- Repository now follows industry best practices for open-source .NET libraries
- Enhanced community engagement and contribution processes

## [1.20.0] - 2025-01-07

### Changed
- Updated all NuGet package dependencies to their latest versions for improved security and compatibility
- Enhanced package reliability with latest dependency versions

### Security
- Updated dependencies to resolve potential vulnerabilities
- Improved package signing and source link configuration

### Note
- No breaking changes; all integration steps remain the same as previous versions

## [1.10.3] - 2025-05-20

### Fixed
- Patch release with minor bug fixes and improvements
- Enhanced error handling in theme switching scenarios

### Changed
- Minor performance optimizations in StyleCache service
- Improved logging and diagnostics

### Note
- No breaking changes; all integration steps remain the same as v1.10.1

## [1.10.1] - 2025-05-18

### Added
- Enhanced logging and diagnostics for static file and theme CSS requests in demo project
- Better compatibility with .NET 9 framework

### Changed
- Updated `Privacy.cshtml.cs` to log current color mode and use required members
- Minor code cleanup and documentation improvements in demo and main library
- Updated project files for compatibility with .NET 9 (demo) and .NET 7 (library)

### Fixed
- Improved error handling in theme CSS loading
- Better fallback mechanisms for missing themes

### Note
- No breaking changes; all integration steps remain the same as v1.10.0

## [1.10.0] - 2025-05-15

### Added
- **New Feature**: Bootswatch theme switcher Tag Helper (`<bootswatch-theme-switcher />`)
- Sample layout file included in NuGet package for quick reference
- Support for `AddHttpContextAccessor()` when using the Tag Helper
- Enhanced JavaScript component for theme switching

### Changed
- Updated documentation to use Tag Helper as the preferred method
- Improved README and installation guide with clearer integration steps
- Enhanced demo layout to showcase best practices

### Fixed
- Ensured static files and theme switcher JavaScript are always available
- Documented correct middleware registration order
- Cleaned up demo layout to avoid duplicate theme switchers

### Documentation
- Comprehensive integration guides
- Best practices documentation
- Enhanced API documentation with examples

## [1.9.0] - 2025-04-01

### Added
- Light/dark mode support with automatic detection
- Cookie-based theme persistence
- Enhanced StyleCache service for better performance

### Changed
- Migrated to Bootstrap 5 foundation
- Improved responsive design patterns
- Enhanced error handling and fallback mechanisms

### Deprecated
- Legacy Bootstrap 4 support (will be removed in 2.0.0)

## [1.8.0] - 2025-03-15

### Added
- Custom theme support (Mom, Texecon themes)
- Embedded static file resources
- Comprehensive caching mechanisms

### Changed
- Improved middleware pipeline integration
- Enhanced service registration patterns

### Fixed
- Static file serving issues in some deployment scenarios
- Theme switching performance improvements

## [1.7.0] - 2025-02-01

### Added
- Initial Razor Class Library implementation
- Basic Bootswatch theme integration
- Core StyleCache service

### Security
- Initial security measures for theme name validation
- XSS protection in theme switching components

## [1.0.0] - 2024-12-01

### Added
- Initial release of WebSpark.Bootswatch
- Basic theme switching functionality
- ASP.NET Core integration

---

## Legend

- **Added**: New features
- **Changed**: Changes in existing functionality
- **Deprecated**: Soon-to-be removed features
- **Removed**: Now removed features
- **Fixed**: Bug fixes
- **Security**: Vulnerability fixes
- **Documentation**: Documentation improvements
- **Note**: Important notes for users