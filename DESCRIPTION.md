# WebSpark.Bootswatch Documentation

## Current version: 1.10.1

WebSpark.Bootswatch is a .NET Razor Class Library that integrates [Bootswatch](https://bootswatch.com/) themes into ASP.NET Core applications. This library allows you to easily switch between multiple Bootstrap themes at runtime.

## Quick Links

- **Demo Site**: [https://bootswatch.markhazleton.com/](https://bootswatch.markhazleton.com/)
- **NuGet Package**: [https://www.nuget.org/packages/WebSpark.Bootswatch](https://www.nuget.org/packages/WebSpark.Bootswatch)
- **GitHub Repository**: [https://github.com/MarkHazleton/WebSpark.Bootswatch](https://github.com/MarkHazleton/WebSpark.Bootswatch)

## Installation

Install via NuGet Package Manager:

```powershell
Install-Package WebSpark.Bootswatch
```

Or via .NET CLI:

```bash
dotnet add package WebSpark.Bootswatch
```

## Setup

### 1. Add the WebSpark.Bootswatch services in your Program.cs

```csharp
// Add Bootswatch theme switcher services (includes StyleCache)
builder.Services.AddBootswatchThemeSwitcher();
```

### 2. Add the Bootswatch middleware to serve theme files and components

```csharp
app.UseStaticFiles();

// Use all Bootswatch features (includes StyleCache and static files)
app.UseBootswatchAll();
```

## Usage

### Including Themes in Layout

Add the following to your `_Layout.cshtml` file:

```html
@using WebSpark.Bootswatch.Services
@using WebSpark.Bootswatch.Helpers
@inject StyleCache StyleCache
<!DOCTYPE html>
<html lang="en" data-bs-theme="@(BootswatchThemeHelper.GetCurrentColorMode(HttpContext))">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>@ViewData["Title"]</title>
    
    @{
        var themeName = BootswatchThemeHelper.GetCurrentThemeName(HttpContext);
        var themeUrl = BootswatchThemeHelper.GetThemeUrl(StyleCache, themeName);
    }
    
    <!-- Reference the current Bootswatch theme -->
    <link id="bootswatch-theme-stylesheet" rel="stylesheet" href="@themeUrl" />
    <script src="/_content/WebSpark.Bootswatch/js/bootswatch-theme-switcher.js"></script>
</head>
<body>
    <!-- Your layout content -->
    
    <!-- Add the theme switcher component -->
    @Html.Raw(BootswatchThemeHelper.GetThemeSwitcherHtml(StyleCache, HttpContext))
</body>
</html>
```

### Theme Switcher Component

The library now includes a built-in theme switcher component that provides:

1. Selection of all available Bootswatch themes
2. Light/dark mode toggle
3. User preference persistence via cookies

To add it to your layout, simply use:

```html
@Html.Raw(BootswatchThemeHelper.GetThemeSwitcherHtml(StyleCache, HttpContext))
```

## Available Themes

WebSpark.Bootswatch includes all standard Bootswatch themes plus some custom themes:

- Default Bootstrap
- Cerulean
- Cosmo
- Cyborg
- Darkly
- Flatly
- Journal
- Litera
- Lumen
- Lux
- Materia
- Minty
- Morph
- Pulse
- Quartz
- Sandstone
- Simplex
- Sketchy
- Slate
- Solar
- Spacelab
- Superhero
- United
- Vapor
- Yeti
- Zephyr
- Mom (Custom)
- Texecon (Custom)

## Advanced Usage

### Using the Built-in StyleCache

The WebSpark.Bootswatch package includes a built-in StyleCache service for efficient theme management:

```csharp
// In your Controller or Razor Page
using WebSpark.Bootswatch.Services;

public class MyController : Controller
{
    private readonly StyleCache _styleCache;
    
    public MyController(StyleCache styleCache)
    {
        _styleCache = styleCache;
    }
    
    public IActionResult Index()
    {
        // Get all available styles
        var styles = _styleCache.GetAllStyles();
        
        // Get a specific style by name
        var darklyStyle = _styleCache.GetStyle("darkly");
        
        return View(styles);
    }
}
```

### Theme Helper Methods

The library provides several helper methods in the `BootswatchThemeHelper` class:

```csharp
// Get the current theme name
var themeName = BootswatchThemeHelper.GetCurrentThemeName(HttpContext);

// Get the current color mode (light/dark)
var colorMode = BootswatchThemeHelper.GetCurrentColorMode(HttpContext);

// Get the URL for a theme
var themeUrl = BootswatchThemeHelper.GetThemeUrl(StyleCache, themeName);

// Get HTML for the theme switcher component
var switcherHtml = BootswatchThemeHelper.GetThemeSwitcherHtml(StyleCache, HttpContext);
```

## License

WebSpark.Bootswatch is licensed under the MIT License. See the LICENSE file for details.
