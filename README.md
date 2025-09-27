# WebSpark.Bootswatch

A .NET 9 Razor Class Library that provides seamless integration of [Bootswatch](https://bootswatch.com/) themes into ASP.NET Core applications. Built on Bootstrap 5, this library offers modern, responsive theming with dynamic theme switching, light/dark mode support, and comprehensive caching mechanisms.

[![NuGet Version](https://img.shields.io/nuget/v/WebSpark.Bootswatch.svg)](https://www.nuget.org/packages/WebSpark.Bootswatch/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/WebSpark.Bootswatch.svg)](https://www.nuget.org/packages/WebSpark.Bootswatch/)
[![GitHub License](https://img.shields.io/github/license/MarkHazleton/WebSpark.Bootswatch)](https://github.com/MarkHazleton/WebSpark.Bootswatch/blob/main/LICENSE)
[![.NET](https://github.com/MarkHazleton/WebSpark.Bootswatch/actions/workflows/dotnet.yml/badge.svg)](https://github.com/MarkHazleton/WebSpark.Bootswatch/actions/workflows/dotnet.yml)
[![GitHub Stars](https://img.shields.io/github/stars/MarkHazleton/WebSpark.Bootswatch)](https://github.com/MarkHazleton/WebSpark.Bootswatch/stargazers)

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

## 📋 Prerequisites

### Required Dependencies

```xml
<PackageReference Include="WebSpark.Bootswatch" Version="1.30.0" />
<PackageReference Include="WebSpark.HttpClientUtility" Version="1.2.0" />
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
<PackageReference Include="WebSpark.Bootswatch" Version="1.30.0" />
<PackageReference Include="WebSpark.HttpClientUtility" Version="1.2.0" />
```

## ⚡ Quick Start

### 1. Configure Services (`Program.cs`)

```csharp
var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddRazorPages();
builder.Services.AddHttpContextAccessor();
builder.Services.AddBootswatchThemeSwitcher();

var app = builder.Build();

// Configure pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseBootswatchAll(); // Must be before UseStaticFiles()
app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();

app.Run();
```

### 2. Update Layout (`_Layout.cshtml`)

```html
@using WebSpark.Bootswatch.Services
@using WebSpark.Bootswatch.Helpers
@inject StyleCache StyleCache
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
</head>
<body>
    <nav class="navbar navbar-expand-lg">
        <div class="container">
            <!-- Your navigation items -->
            <ul class="navbar-nav flex-grow-1">
                <!-- Nav items here -->
            </ul>
            <!-- Theme switcher -->
            <bootswatch-theme-switcher />
        </div>
    </nav>
    
    <main>
        @RenderBody()
    </main>
    
    <script src="~/lib/bootstrap/dist/js/bootstrap.bundle.min.js"></script>
    @await RenderSectionAsync("Scripts", required: false)
</body>
</html>
```

### 3. Register Tag Helper (`_ViewImports.cshtml`)

```csharp
@addTagHelper *, WebSpark.Bootswatch
```

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

## 🧪 Demo Project

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

## 🔒 Security

- ✅ **Input Validation**: Theme names sanitized and validated
- ✅ **XSS Protection**: HTML encoding in all outputs
- ✅ **HTTPS**: Secure external resource loading
- ✅ **CSP Friendly**: No inline scripts or styles
- ✅ **CORS Compliant**: Proper resource sharing policies

## 🛠️ Troubleshooting

### Common Issues

| Issue | Solution |
|-------|----------|
| Themes not loading | Check middleware order: `UseBootswatchAll()` before `UseStaticFiles()` |
| Theme switcher not visible | Ensure `@addTagHelper *, WebSpark.Bootswatch` in `_ViewImports.cshtml` |
| Missing dependencies | Install `WebSpark.HttpClientUtility` package |
| Configuration errors | Add required `appsettings.json` configuration |

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

# Build solution
dotnet build

# Run tests
dotnet test

# Run demo
dotnet run --project WebSpark.Bootswatch.Demo
```

### Contribution Areas

- 🐛 Bug fixes and improvements
- 📚 Documentation enhancements
- 🎨 New theme contributions
- 🧪 Test coverage expansion
- 🚀 Performance optimizations

## 📝 Changelog

### [1.20.0] - 2025-01-07
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
