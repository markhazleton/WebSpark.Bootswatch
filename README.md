# WebSpark.Bootswatch

A .NET Razor Class Library that provides seamless integration of [Bootswatch](https://bootswatch.com/) themes into ASP.NET Core applications. Built on Bootstrap 5, this library offers modern, responsive theming with dynamic theme switching, light/dark mode support, and comprehensive caching mechanisms.

**Multi-Framework Support**: Targets .NET 8.0 (LTS), .NET 9.0 (STS), and .NET 10.0 for maximum compatibility.

[![NuGet Version](https://img.shields.io/nuget/v/WebSpark.Bootswatch.svg)](https://www.nuget.org/packages/WebSpark.Bootswatch/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/WebSpark.Bootswatch.svg)](https://www.nuget.org/packages/WebSpark.Bootswatch/)
[![GitHub License](https://img.shields.io/github/license/MarkHazleton/WebSpark.Bootswatch)](https://github.com/MarkHazleton/WebSpark.Bootswatch/blob/main/LICENSE)
[![.NET](https://github.com/MarkHazleton/WebSpark.Bootswatch/actions/workflows/dotnet.yml/badge.svg)](https://github.com/MarkHazleton/WebSpark.Bootswatch/actions/workflows/dotnet.yml)
[![Multi-Framework Tests](https://github.com/MarkHazleton/WebSpark.Bootswatch/actions/workflows/multi-framework-tests.yml/badge.svg)](https://github.com/MarkHazleton/WebSpark.Bootswatch/actions/workflows/multi-framework-tests.yml)
[![GitHub Stars](https://img.shields.io/github/stars/MarkHazleton/WebSpark.Bootswatch)](https://github.com/MarkHazleton/WebSpark.Bootswatch/stargazers)

> **Latest Release**: v1.34.0 - Demo site UI improvements for better theme visibility

## 🚀 Quick Links

- **📦 NuGet Package**: [WebSpark.Bootswatch](https://www.nuget.org/packages/WebSpark.Bootswatch)
- **🎨 Demo Site**: [bootswatch.markhazleton.com](https://bootswatch.markhazleton.com/)
- **📚 Documentation**: [GitHub Wiki](https://github.com/MarkHazleton/WebSpark.Bootswatch/wiki)
- **🐛 Issues**: [Report a Bug](https://github.com/MarkHazleton/WebSpark.Bootswatch/issues)

## ✨ Features

- **🎨 Complete Bootswatch Integration**: All official Bootswatch themes plus custom themes
- **🌓 Light/Dark Mode Support**: Automatic theme detection and switching
- **⚡ High Performance**: Built-in caching with `StyleCache` service
- **🔧 Easy Integration**: Single-line setup with extension methods
- **📱 Responsive Design**: Mobile-first Bootstrap 5 foundation
- **🎯 Tag Helper Support**: `<bootswatch-theme-switcher />` for easy UI integration
- **🔒 Production Ready**: Comprehensive error handling and fallback mechanisms
- **📖 Full Documentation**: IntelliSense support and XML documentation
- **🎁 Multi-Framework**: Supports .NET 8.0 (LTS), 9.0 (STS), and 10.0

## ⚠️ IMPORTANT: Required Dependencies

**WebSpark.Bootswatch requires WebSpark.HttpClientUtility to be installed AND registered separately.**

### ✅ Quick Setup Checklist

Before starting, ensure you complete ALL of these steps:

- [ ] Install **both** packages: `WebSpark.Bootswatch` AND `WebSpark.HttpClientUtility`
- [ ] Add both using statements to `Program.cs`
- [ ] Register `AddHttpClientUtility()` **BEFORE** `AddBootswatchThemeSwitcher()`
- [ ] Add required configuration to `appsettings.json`
- [ ] Use `UseBootswatchAll()` **BEFORE** `UseStaticFiles()` in middleware pipeline

**Missing any of these steps will cause runtime errors!**

### Common Setup Mistake

```csharp
// ❌ WRONG - Missing HttpClientUtility registration
builder.Services.AddBootswatchThemeSwitcher();

// ✅ CORRECT - HttpClientUtility registered first
using WebSpark.Bootswatch;
using WebSpark.HttpClientUtility;

builder.Services.AddHttpClientUtility();      // Must be FIRST
builder.Services.AddBootswatchThemeSwitcher(); // Then this
```

## 📋 Prerequisites

### Framework Support

The library supports multiple .NET versions:

| Framework | Status | Support Level |
|-----------|--------|---------------|
| .NET 8.0 | ✅ Supported | LTS (Long Term Support) |
| .NET 9.0 | ✅ Supported | STS (Standard Term Support) |
| .NET 10.0 | ✅ Supported | Current Release |

Your project can target any of these frameworks and will receive the appropriate version of the library.

### Required Dependencies

```xml
<PackageReference Include="WebSpark.Bootswatch" Version="1.34.0" />
<PackageReference Include="WebSpark.HttpClientUtility" Version="2.1.1" />
```

### Configuration Requirements

Add to your `appsettings.json` for dynamic theme fetching:

```json
{
  "CsvOutputFolder": "c:\\temp\\WebSpark\\CsvOutput",
  "HttpRequestResultPollyOptions": {
    "MaxRetryAttempts": 3,
    "RetryDelaySeconds": 1,
    "CircuitBreakerThreshold": 3,
    "CircuitBreakerDurationSeconds": 10
  }
}
```

## 🛠️ Installation

### Package Manager Console

```powershell
Install-Package WebSpark.Bootswatch
Install-Package WebSpark.HttpClientUtility
```

### .NET CLI

```bash
dotnet add package WebSpark.Bootswatch
dotnet add package WebSpark.HttpClientUtility
```

### PackageReference

```xml
<PackageReference Include="WebSpark.Bootswatch" Version="1.34.0" />
<PackageReference Include="WebSpark.HttpClientUtility" Version="2.1.1" />
```

The NuGet package automatically selects the correct assembly based on your project's target framework.

## ⚡ Quick Start

### Step 1: Install BOTH Required Packages

```bash
# Install WebSpark.Bootswatch
dotnet add package WebSpark.Bootswatch

# Install REQUIRED dependency (NOT automatically installed)
dotnet add package WebSpark.HttpClientUtility
```

**Verify Installation:**
Your `.csproj` should now include BOTH packages:

```xml
<PackageReference Include="WebSpark.Bootswatch" Version="1.34.0" />
<PackageReference Include="WebSpark.HttpClientUtility" Version="2.1.1" />
```

### Step 2: Add Required Configuration

Create or update `appsettings.json`:

```json
{
  "CsvOutputFolder": "c:\\temp\\WebSpark\\CsvOutput",
  "HttpRequestResultPollyOptions": {
    "MaxRetryAttempts": 3,
    "RetryDelaySeconds": 1,
    "CircuitBreakerThreshold": 3,
    "CircuitBreakerDurationSeconds": 10
  },
  "BootswatchOptions": {
    "DefaultTheme": "yeti",
    "EnableCaching": true,
    "CacheDurationMinutes": 60
  }
}
```

### Step 3: Configure Services in Program.cs

Add using statements at the top:

```csharp
using WebSpark.Bootswatch;
using WebSpark.HttpClientUtility;
```

Register services in the **correct order**:

```csharp
var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddRazorPages();
builder.Services.AddHttpContextAccessor();

// ⚠️ CRITICAL: Register HttpClientUtility FIRST
builder.Services.AddHttpClientUtility();

// Then register Bootswatch theme switcher
builder.Services.AddBootswatchThemeSwitcher();

var app = builder.Build();

// Configure middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// ⚠️ CRITICAL: UseBootswatchAll() must come BEFORE UseStaticFiles()
app.UseBootswatchAll();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();

app.Run();
```

### Step 4: Update _ViewImports.cshtml

```csharp
@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
@addTagHelper *, WebSpark.Bootswatch
```

### Step 5: Update _Layout.cshtml

Add required using statements and inject StyleCache:

```csharp
@using WebSpark.Bootswatch.Services
@using WebSpark.Bootswatch.Helpers
@inject StyleCache StyleCache
```

Update the HTML structure:

```html
<!DOCTYPE html>
<html lang="en" data-bs-theme="@(BootswatchThemeHelper.GetCurrentColorMode(Context))">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>@ViewData["Title"]</title>
    @{
        var themeName = BootswatchThemeHelper.GetCurrentThemeName(Context);
        var themeUrl = BootswatchThemeHelper.GetThemeUrl(StyleCache, themeName);
    }
    <link id="bootswatch-theme-stylesheet" rel="stylesheet" href="@themeUrl" />
    <script src="/_content/WebSpark.Bootswatch/js/bootswatch-theme-switcher.js"></script>
    
    <!-- Bootstrap Icons -->
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css" />
</head>
<body>
    <nav class="navbar navbar-expand-lg">
        <div class="container">
            <a class="navbar-brand" href="/">My App</a>
            <ul class="navbar-nav ms-auto">
                <!-- Your navigation items -->
                
                <!-- Theme Switcher Tag Helper -->
                <bootswatch-theme-switcher />
            </ul>
        </div>
    </nav>
    
    <main>
        @RenderBody()
    </main>
    
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
    @await RenderSectionAsync("Scripts", required: false)
</body>
</html>
```

### Step 6: Verify Setup

Build and run your application:

```bash
dotnet build
dotnet run
```

**Expected Results:**

- ✅ Application starts without errors
- ✅ Theme switcher appears in navigation
- ✅ Default theme is applied
- ✅ Theme switching works
- ✅ Light/dark mode toggle functions

---

## ⚠️ Common Errors & Solutions

### Error: "Unable to resolve service for type 'IHttpRequestResultService'"

**Full Error Message:**

```
System.AggregateException: Some services are not able to be constructed 
(Error while validating the service descriptor 'ServiceType: WebSpark.Bootswatch.Model.IStyleProvider 
Lifetime: Scoped ImplementationType: WebSpark.Bootswatch.Provider.BootswatchStyleProvider': 
Unable to resolve service for type 'WebSpark.HttpClientUtility.RequestResult.IHttpRequestResultService'
```

**Cause:** `WebSpark.HttpClientUtility` services are not registered.

**Solution:**

1. Verify package is installed: `dotnet list package | findstr HttpClientUtility`
2. Add using statement: `using WebSpark.HttpClientUtility;`
3. Register services BEFORE Bootswatch:

```csharp
using WebSpark.Bootswatch;
using WebSpark.HttpClientUtility;

builder.Services.AddHttpClientUtility();      // ✅ Must be FIRST
builder.Services.AddBootswatchThemeSwitcher(); // ✅ Then this
```

---

### Error: "Themes not loading" or "404 errors for theme files"

**Cause:** Middleware is in wrong order.

**Solution:**
Ensure `UseBootswatchAll()` comes BEFORE `UseStaticFiles()`:

```csharp
// ✅ CORRECT ORDER
app.UseBootswatchAll();    // First
app.UseStaticFiles();      // Then this

// ❌ WRONG ORDER (will fail)
app.UseStaticFiles();
app.UseBootswatchAll();
```

---

### Error: "Configuration section not found"

**Cause:** Missing or incorrect `appsettings.json` configuration.

**Solution:**
Ensure ALL required sections are present in `appsettings.json`:

```json
{
  "CsvOutputFolder": "c:\\temp\\WebSpark\\CsvOutput",
  "HttpRequestResultPollyOptions": {
    "MaxRetryAttempts": 3,
    "RetryDelaySeconds": 1,
    "CircuitBreakerThreshold": 3,
    "CircuitBreakerDurationSeconds": 10
  }
}
```

---

### Error: Theme switcher not visible

**Cause:** Tag helper not registered in `_ViewImports.cshtml`.

**Solution:**
Add to `_ViewImports.cshtml`:

```csharp
@addTagHelper *, WebSpark.Bootswatch
```

---

## 🎯 Advanced Usage

### StyleCache Service

```csharp
public class HomeController : Controller
{
    private readonly StyleCache _styleCache;

    public HomeController(StyleCache styleCache)
    {
        _styleCache = styleCache;
    }

    public IActionResult Index()
    {
        // Get all available themes
        var allThemes = _styleCache.GetAllStyles();
        
        // Get specific theme
        var defaultTheme = _styleCache.GetStyle("default");
        
        return View(allThemes);
    }
}
```

### Theme Helper Methods

```csharp
// Get current theme information
var currentTheme = BootswatchThemeHelper.GetCurrentThemeName(Context);
var colorMode = BootswatchThemeHelper.GetCurrentColorMode(Context);
var themeUrl = BootswatchThemeHelper.GetThemeUrl(StyleCache, currentTheme);

// Generate theme switcher HTML
var switcherHtml = BootswatchThemeHelper.GetThemeSwitcherHtml(StyleCache, Context);
```

### Custom Theme Integration

```csharp
// Add custom themes to your StyleCache
public void ConfigureServices(IServiceCollection services)
{
    services.AddBootswatchThemeSwitcher();
    services.Configure<BootswatchOptions>(options =>
    {
        options.CustomThemes.Add(new StyleModel
        {
            Name = "custom-theme",
            Description = "My Custom Theme",
            CssPath = "/css/custom-theme.css"
        });
    });
}
```

## 🧪 Testing & Demo

### Demo Project

Explore the complete implementation in our demo project:

```bash
git clone https://github.com/MarkHazleton/WebSpark.Bootswatch.git
cd WebSpark.Bootswatch
dotnet run --project WebSpark.Bootswatch.Demo
```

The demo showcases:

- ✅ All Bootswatch themes
- ✅ Light/dark mode switching
- ✅ Responsive design patterns
- ✅ Integration examples
- ✅ Performance optimizations

### Multi-Framework Testing

The library includes comprehensive tests that run on all supported frameworks:

```bash
# Test all frameworks
dotnet test

# Test specific framework
dotnet test --framework net8.0
dotnet test --framework net9.0
dotnet test --framework net10.0

# Use PowerShell script for detailed output
.\run-multi-framework-tests.ps1
```

Our CI/CD pipeline runs separate test jobs for each framework, ensuring compatibility across all supported .NET versions.

## 🏗️ Architecture

### Core Components

| Component | Purpose | Lifecycle |
|-----------|---------|-----------|
| `StyleCache` | Theme data caching | Singleton |
| `BootswatchStyleProvider` | Theme management | Scoped |
| `BootswatchThemeHelper` | Static utilities | Static |
| `BootswatchThemeSwitcherTagHelper` | UI component | Transient |

### Middleware Pipeline

The correct middleware order is crucial:

```csharp
app.UseBootswatchStaticFiles(); // 1. Bootswatch static files
app.UseStaticFiles();           // 2. Application static files  
app.UseRouting();               // 3. Routing
```

### Multi-Framework Package Structure

The NuGet package contains separate assemblies for each target framework:

```
WebSpark.Bootswatch.1.31.0.nupkg
├── lib/
│   ├── net8.0/
│   │   └── WebSpark.Bootswatch.dll
│   ├── net9.0/
│   │   └── WebSpark.Bootswatch.dll
│   └── net10.0/
│       └── WebSpark.Bootswatch.dll
```

Each assembly is compiled with framework-specific optimizations and references the appropriate version of dependencies.

## 🔧 Configuration Options

### Middleware Configuration

```csharp
// Full configuration
app.UseBootswatchAll();

// Or individual components
app.UseBootswatchStaticFiles();
app.UseBootswatchThemeRoutes();
```

### Service Configuration

```csharp
services.AddBootswatchThemeSwitcher(options =>
{
    options.DefaultTheme = "bootstrap";
    options.EnableCaching = true;
    options.CacheDurationMinutes = 60;
});
```

## 🚀 Performance

### Caching Strategy

- **Theme Data**: Cached in `StyleCache` singleton
- **HTTP Requests**: Resilient HTTP client with Polly
- **Static Files**: Embedded resources with cache headers
- **Background Loading**: Non-blocking theme initialization

### Bundle Optimization

- **CSS**: Minified Bootswatch themes
- **JavaScript**: Lightweight theme switcher (~2KB)
- **Icons**: Optimized SVG assets

### Framework-Specific Optimizations

Each target framework receives optimized builds:

- **.NET 8.0**: LTS-optimized with proven stability
- **.NET 9.0**: Enhanced performance features
- **.NET 10.0**: Latest runtime optimizations

## 🔒 Security

- ✅ **Input Validation**: Theme names sanitized and validated
- ✅ **XSS Protection**: HTML encoding in all outputs
- ✅ **HTTPS**: Secure external resource loading
- ✅ **CSP Friendly**: No inline scripts or styles
- ✅ **CORS Compliant**: Proper resource sharing policies

## 🛠️ Troubleshooting

For detailed troubleshooting, see the [Common Errors & Solutions](#-common-errors--solutions) section above.

### Quick Reference

| Issue | Solution |
|-------|----------|
| Service resolution error | Register `AddHttpClientUtility()` before `AddBootswatchThemeSwitcher()` |
| Themes not loading | Check middleware order: `UseBootswatchAll()` before `UseStaticFiles()` |
| Theme switcher not visible | Ensure `@addTagHelper *, WebSpark.Bootswatch` in `_ViewImports.cshtml` |
| Missing dependencies | Install `WebSpark.HttpClientUtility` package |
| Configuration errors | Add required `appsettings.json` configuration |
| Wrong framework version | NuGet automatically selects correct version based on your target framework |

### Debug Mode

Enable detailed logging:

```csharp
builder.Services.AddLogging(config =>
{
    config.AddConsole();
    config.SetMinimumLevel(LogLevel.Debug);
});
```

## 📊 Browser Support

| Browser | Version | Status |
|---------|---------|---------|
| Chrome | 90+ | ✅ Fully Supported |
| Firefox | 88+ | ✅ Fully Supported |
| Safari | 14+ | ✅ Fully Supported |
| Edge | 90+ | ✅ Fully Supported |
| IE | 11 | ❌ Not Supported |

## 🤝 Contributing

We welcome contributions! Please see [CONTRIBUTING.md](./copilot/CONTRIBUTING.md) for guidelines.

### Development Setup

```bash
# Clone repository
git clone https://github.com/MarkHazleton/WebSpark.Bootswatch.git
cd WebSpark.Bootswatch

# Restore dependencies
dotnet restore

# Build solution (builds for all target frameworks)
dotnet build

# Run tests (tests all frameworks)
dotnet test

# Run demo
dotnet run --project WebSpark.Bootswatch.Demo
```

### Testing Contributions

When contributing, ensure your changes work across all target frameworks:

```bash
# Run comprehensive multi-framework tests
.\run-multi-framework-tests.ps1 -Configuration Release
```

### Contribution Areas

- 🐛 Bug fixes and improvements
- 📚 Documentation enhancements
- 🎨 New theme contributions
- 🧪 Test coverage expansion
- 🚀 Performance optimizations
- 🎯 Framework-specific optimizations

## 📝 Changelog

### [1.34.0] - 2025-12-03

- 🎨 **Demo Site UI Improvements**: Enhanced hero section visibility across all themes
- 🎨 **Better Contrast**: Added shadow effects and explicit color classes for readability
- 🎨 **Typography Enhancements**: Improved visual hierarchy with better weights
- ✅ **No Library Changes**: Demo-only improvements, library remains unchanged
- ✅ **No Breaking Changes**: Fully backward compatible

### [1.33.0] - 2025-12-03

- ✅ **Dependency Validation**: Automatic detection of missing required services
- ✅ **Configuration Validation**: Startup validation service with helpful warnings
- ✅ **Enhanced XML Documentation**: Comprehensive IntelliSense with code examples
- ✅ **Improved Error Messages**: Clear, actionable error messages with solutions
- 📚 **README Rewrite**: Complete step-by-step setup guide with troubleshooting
- ✅ **No Breaking Changes**: Fully backward compatible

### [1.31.0] - 2025-01-13

- ✅ **Multi-Framework Support**: Added .NET 8.0, 9.0, and 10.0 targeting
- ✅ **Updated Dependencies**: Framework-specific package versions
- ✅ **Comprehensive Testing**: Multi-framework test suite with CI/CD
- ✅ **Removed Legacy Code**: Eliminated System.Text.RegularExpressions (now in BCL)
- ✅ **No Breaking Changes**: Fully backward compatible

### [1.30.0] - 2025-01-07

- ✅ Updated all NuGet dependencies to latest versions
- ✅ Enhanced security with latest dependency versions
- ✅ No breaking changes

### [1.10.3] - 2025-05-20

- ✅ Patch release with minor improvements
- ✅ Enhanced logging and diagnostics

### [1.10.0] - 2025-05-15

- ✅ Added Bootswatch Theme Switcher Tag Helper
- ✅ Included sample layout file in NuGet package
- ✅ Improved documentation and integration guides

[View Full Changelog](./copilot/CHANGELOG.md)

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](./LICENSE) file for details.

### Third-Party Licenses

- **Bootstrap**: MIT License
- **Bootswatch**: MIT License  
- **WebSpark.HttpClientUtility**: MIT License

See [NOTICE.txt](./NOTICE.txt) for complete attribution.

## 🙏 Acknowledgments

- **Bootstrap Team** - For the amazing Bootstrap framework
- **Thomas Park** - Creator of Bootswatch themes
- **.NET Team** - For excellent multi-targeting support
- **Contributors** - Everyone who has contributed to this project

## 📞 Support

- 📖 **Documentation**: [GitHub Wiki](https://github.com/MarkHazleton/WebSpark.Bootswatch/wiki)
- 🐛 **Bug Reports**: [GitHub Issues](https://github.com/MarkHazleton/WebSpark.Bootswatch/issues)
- 💬 **Discussions**: [GitHub Discussions](https://github.com/MarkHazleton/WebSpark.Bootswatch/discussions)
- 📧 **Email**: [Contact Author](mailto:mark@markhazleton.com)

---

<div align="center">
  <p>Made with ❤️ by <a href="https://github.com/MarkHazleton">Mark Hazleton</a></p>
  <p>
    <a href="https://github.com/MarkHazleton/WebSpark.Bootswatch">⭐ Star this repo</a> •
    <a href="https://github.com/MarkHazleton/WebSpark.Bootswatch/fork">🔀 Fork</a> •
    <a href="https://github.com/MarkHazleton/WebSpark.Bootswatch/issues">🐛 Report Bug</a> •
    <a href="https://github.com/MarkHazleton/WebSpark.Bootswatch/discussions">💬 Discuss</a>
  </p>
</div>
