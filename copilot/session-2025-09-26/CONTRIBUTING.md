# Contributing to WebSpark.Bootswatch

Thank you for considering contributing to WebSpark.Bootswatch! This document provides guidelines and information for contributors.

## ?? How to Contribute

### Reporting Issues

Before creating an issue, please:

1. **Search existing issues** to avoid duplicates
2. **Use the issue templates** when available
3. **Provide clear, detailed information** including:
   - .NET version
   - ASP.NET Core version
   - Browser and version (if UI-related)
   - Steps to reproduce
   - Expected vs actual behavior
   - Code samples or screenshots

### Suggesting Enhancements

Enhancement suggestions are welcome! Please:

1. **Check existing discussions** and issues first
2. **Clearly describe the enhancement** and its benefits
3. **Provide use cases** or examples
4. **Consider backward compatibility** implications

### Code Contributions

#### Getting Started

1. **Fork the repository**
   ```bash
   git clone https://github.com/your-username/WebSpark.Bootswatch.git
   cd WebSpark.Bootswatch
   ```

2. **Set up development environment**
   ```bash
   # Restore dependencies
   dotnet restore
   
   # Build solution
   dotnet build
   
   # Run tests
   dotnet test
   
   # Run demo project
   dotnet run --project WebSpark.Bootswatch.Demo
   ```

3. **Create a feature branch**
   ```bash
   git checkout -b feature/your-feature-name
   ```

#### Development Guidelines

##### Code Style

- Follow **Microsoft C# Coding Conventions**
- Use **nullable reference types** consistently
- Implement **proper async/await** patterns
- Follow **PascalCase** for public members
- Use `var` for obvious type declarations
- Add **XML documentation** for all public APIs

##### Architecture Patterns

- Use **constructor injection** for dependencies
- Follow **singleton pattern** for `StyleCache`
- Implement **proper error handling** with graceful fallbacks
- Use **structured logging** with appropriate levels

##### Testing Requirements

- **Unit tests** for all new functionality
- **Integration tests** for middleware and services
- **Maintain or improve** existing test coverage
- **Mock external dependencies** appropriately

#### Pull Request Process

1. **Update documentation** as needed
2. **Add or update tests** for your changes
3. **Ensure all tests pass**
   ```bash
   dotnet test
   ```
4. **Update CHANGELOG.md** with your changes
5. **Create a pull request** with:
   - Clear title and description
   - Reference to related issues
   - Screenshots for UI changes
   - Breaking change notes (if any)

##### Pull Request Checklist

- [ ] Code follows project conventions
- [ ] Tests added/updated and passing
- [ ] Documentation updated
- [ ] CHANGELOG.md updated
- [ ] No breaking changes (or clearly documented)
- [ ] Commit messages are clear and descriptive

## ??? Project Structure

```
WebSpark.Bootswatch/
??? WebSpark.Bootswatch/           # Main library
?   ??? Services/                  # Core services
?   ??? Helpers/                   # Static helpers
?   ??? TagHelpers/               # Razor tag helpers
?   ??? Models/                   # Data models
?   ??? wwwroot/                  # Static assets
??? WebSpark.Bootswatch.Demo/     # Demo application
??? copilot/                      # Documentation
??? .github/                      # GitHub workflows
```

## ?? Testing

### Running Tests

```bash
# All tests
dotnet test

# Specific project
dotnet test WebSpark.Bootswatch.Tests

# With coverage
dotnet test --collect:"XPlat Code Coverage"
```

### Test Categories

- **Unit Tests**: Core logic and services
- **Integration Tests**: Middleware and full workflows
- **UI Tests**: Tag helpers and components

### Writing Tests

```csharp
[Test]
public void StyleCache_GetStyle_ReturnsCorrectStyle()
{
    // Arrange
    var styleCache = new StyleCache();
    
    // Act
    var result = styleCache.GetStyle("bootstrap");
    
    // Assert
    Assert.NotNull(result);
    Assert.Equal("bootstrap", result.Name);
}
```

## ?? Package Management

### Dependencies

- Keep dependencies **up to date**
- Use **minimal required versions**
- Avoid **unnecessary dependencies**
- Document **breaking changes**

### Versioning

We follow **Semantic Versioning (SemVer)**:

- **MAJOR**: Breaking changes
- **MINOR**: New features, backward compatible
- **PATCH**: Bug fixes, backward compatible
- **PRERELEASE**: Unstable versions (-alpha, -beta, -rc)

## ?? Theme Contributions

### Adding New Themes

1. **Create CSS file** in `wwwroot/css/`
2. **Update BootswatchStyleProvider** with theme definition
3. **Add to embedded resources**
4. **Update demo pages**
5. **Test theme switching**

### Theme Requirements

- **Bootstrap 5 compatible**
- **Responsive design**
- **Light/dark mode support**
- **Proper contrast ratios**
- **Consistent naming conventions**

## ?? Documentation

### Areas Needing Documentation

- **API documentation** (XML comments)
- **Integration guides**
- **Troubleshooting guides**
- **Performance optimization**
- **Security considerations**

### Documentation Standards

- **Clear, concise language**
- **Code examples** for all features
- **Step-by-step instructions**
- **Screenshots** for UI components
- **Up-to-date** with current version

## ?? Code Review Process

### Review Criteria

- **Functionality**: Does it work as intended?
- **Code Quality**: Is it well-written and maintainable?
- **Performance**: Are there any performance implications?
- **Security**: Are there any security concerns?
- **Testing**: Is it adequately tested?
- **Documentation**: Is it properly documented?

### Review Timeline

- **Initial response**: Within 48 hours
- **Full review**: Within 1 week
- **Follow-up**: Within 24 hours of updates

## ?? Release Process

### Release Preparation

1. **Update version numbers**
2. **Update CHANGELOG.md**
3. **Update package metadata**
4. **Run full test suite**
5. **Build and test package locally**

### Release Steps

1. **Create release branch**
2. **Tag release**
3. **Build NuGet package**
4. **Publish to NuGet.org**
5. **Create GitHub release**
6. **Update demo site**

## ?? Getting Help

### Resources

- **GitHub Issues**: Bug reports and feature requests
- **GitHub Discussions**: Questions and community support
- **Wiki**: Detailed documentation
- **Demo Site**: Live examples

### Contact

- **Email**: [mark@markhazleton.com](mailto:mark@markhazleton.com)
- **GitHub**: [@MarkHazleton](https://github.com/MarkHazleton)

## ?? License

By contributing to WebSpark.Bootswatch, you agree that your contributions will be licensed under the MIT License.

---

Thank you for contributing to WebSpark.Bootswatch! ??