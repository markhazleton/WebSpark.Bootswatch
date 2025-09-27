# Copilot Instructions for WebSpark.Bootswatch

## Project Overview

WebSpark.Bootswatch is a .NET 9 Razor Class Library that integrates Bootswatch themes into ASP.NET Core applications. It provides seamless Bootstrap 5 theme switching capabilities with light/dark mode support, caching mechanisms, and embedded static files.

**Projects:**
- **WebSpark.Bootswatch**: Main library (.NET 9 Razor Class Library)
- **WebSpark.Bootswatch.Demo**: Demo application (.NET 9 ASP.NET Core)

## Key Architecture Components

### Core Services
- `StyleCache`: Singleton service for caching Bootswatch theme data
- `BootswatchStyleProvider`: Provider for fetching and managing themes
- `BootswatchThemeHelper`: Static helper methods

### Models & Interfaces
- `StyleModel`, `BootswatchStyle`: Theme data structures
- `IStyleProvider`: Theme provider interface

### UI Components
- `BootswatchThemeSwitcherTagHelper`: Tag helper for theme switching
- JavaScript component for client-side functionality

## Coding Standards

### C# Conventions
- Use nullable reference types consistently
- Implement proper async/await patterns
- Follow Microsoft naming conventions (PascalCase for public members)
- Use `var` for obvious type declarations
- Implement structured logging

### Dependency Injection
- Use constructor injection for all services
- Register services using extension methods (`AddBootswatchThemeSwitcher()`)
- Follow singleton pattern for `StyleCache`

### Middleware Pipeline Order
```csharp
app.UseBootswatchStaticFiles(); // Before UseStaticFiles()
app.UseStaticFiles();
```

## NuGet Package Best Practices

### Essential Package Properties
1. **Semantic Versioning (SemVer)**: Use Major.Minor.Patch[-prerelease]
2. **README File**: Include comprehensive README.md in package
3. **License**: Use SPDX license expression (e.g., `MIT`)
4. **Description**: Concise, informative package description
5. **Tags**: Space-delimited keywords for discoverability
6. **Package Icon**: 128x128 pixels, JPEG/PNG, max 1MB
7. **PDBs and Source Link**: Include for debugging support
8. **XML Documentation**: Generate and include for IntelliSense
9. **Target Frameworks**: Support appropriate .NET versions
10. **Package Signing**: Digital signature for integrity

### Package Configuration Example
```xml
<PropertyGroup>
    <GeneratePackageOnBuild>true</GeneratePackageOnBuild>
    <GenerateDocumentationFile>true</GenerateDocumentationFile>
    <IncludeSymbols>true</IncludeSymbols>
    <SymbolPackageFormat>snupkg</SymbolPackageFormat>
    <PublishRepositoryUrl>true</PublishRepositoryUrl>
    <EmbedUntrackedSources>true</EmbedUntrackedSources>
    <PackageReadmeFile>README.md</PackageReadmeFile>
    <PackageLicenseExpression>MIT</PackageLicenseExpression>
</PropertyGroup>
```

## File Organization Guidelines

### Copilot-Generated Documentation
- **ALL** copilot-generated .md files MUST go in `/copilot/session-{date}` folder
- Use format: `/copilot/session-YYYY-MM-DD/filename.md`
- Exception: `README.md` remains in repository root
- Move existing .md files to `/copilot/` folder (already done)

### Repository Structure
```
/
??? README.md                           # Main project documentation
??? copilot/                           # Copilot-generated content
?   ??? session-2025-09-26/           # Session-specific files
?   ??? BEST_PRACTICES.md             # Moved existing files
?   ??? ThemeSwitcherGuide.md         # Moved existing files
?   ??? ...                           # Other moved files
??? WebSpark.Bootswatch/              # Main library
??? WebSpark.Bootswatch.Demo/         # Demo application
```

## Common Development Tasks

### Adding New Themes
1. Update `BootswatchStyleProvider` with new theme definitions
2. Embed CSS files as resources
3. Update demo pages
4. Test theme switching functionality

### Performance Optimization
- Implement HTTP request caching for Bootswatch API
- Use background services for non-blocking initialization
- Add cache control headers for static files
- Consider CDN fallbacks

### Error Handling Best Practices
- Provide graceful fallbacks to default Bootstrap theme
- Log errors with appropriate severity levels
- Return safe defaults for invalid inputs
- Implement circuit breaker patterns for external API calls

## Security & Deployment

### Security Considerations
- Validate theme names to prevent injection attacks
- Sanitize user inputs in theme switching
- Use HTTPS for external resources
- Implement proper CORS policies

### Deployment Notes
- Package static files as embedded resources
- Include all dependencies in NuGet package
- Test package installation in clean environment
- Verify compatibility across .NET versions

## Testing Guidelines

### Unit Testing Focus
- Service initialization and caching logic
- Theme switching functionality
- Error handling and fallback scenarios
- Mock external dependencies

### Integration Testing
- Complete theme switching workflow
- Static file serving
- Cookie persistence
- JavaScript component functionality

## Documentation Standards

### Code Comments
- Use XML documentation for all public APIs
- Include usage examples
- Document complex algorithms
- Explain non-obvious implementation decisions

### Generated Documentation
- All new .md files go in `/copilot/session-{date}/`
- Update existing documentation in-place
- Link to demo application for examples
- Provide troubleshooting sections

## Common Pitfalls to Avoid

- Don't block application startup with theme initialization
- Don't forget correct middleware registration order
- Don't cache themes indefinitely without refresh mechanism
- Don't ignore browser compatibility for JavaScript
- Don't hardcode theme URLs without fallback options
- Don't place copilot-generated .md files in root directory

## Development Environment

- Visual Studio 2022 or VS Code with C# extension
- .NET 9 SDK installed
- Chrome/Edge Developer Tools for debugging
- Node.js for JavaScript tooling (if needed)

## Quick Integration Summary

```csharp
// Program.cs
builder.Services.AddBootswatchThemeSwitcher();
app.UseBootswatchAll();

// Layout
@inject StyleCache StyleCache
<bootswatch-theme-switcher />

// _ViewImports.cshtml
@addTagHelper *, WebSpark.Bootswatch