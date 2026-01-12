# Changelog - HISTORICAL

> **Note**: This is a historical changelog for session 2025-09-26. 
> For current version information, see the main [CHANGELOG.md](../../CHANGELOG.md) in the repository root.
> 
> **Current Version**: 2.0.0 (targets .NET 10 exclusively)

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
- **Historical**: This version supported .NET 8, 9, and 10. Version 2.0+ targets .NET 10 exclusively.

## [1.20.0] - 2025-01-07

### Changed
- Updated all NuGet package dependencies to their latest versions for improved security and compatibility
- Enhanced package reliability with latest dependency versions

### Security
- Updated dependencies to resolve potential vulnerabilities
- Improved package signing and source link configuration

### Note
- No breaking changes; all integration steps remain the same as previous versions
- **Historical**: This version supported multiple .NET versions.

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
- Better compatibility with .NET 9 framework (Historical)

### Changed
- Updated `Privacy.cshtml.cs` to log current color mode and use required members
- Minor code cleanup and documentation improvements in demo and main library
- Updated project files for compatibility with .NET 9 (demo) and .NET 7 (library) (Historical)

### Fixed
- Improved error handling in theme CSS loading
- Better fallback mechanisms for missing themes

### Note
- No breaking changes; all integration steps remain the same as v1.10.0
- **Historical**: Multi-framework support approach