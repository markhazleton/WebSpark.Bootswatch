# WebSpark.Bootswatch

A .NET Razor Class Library for integrating Bootstrap 5 themes from [Bootswatch](https://bootswatch.com/) into ASP.NET Core applications. This library simplifies the process of applying modern, responsive themes to your web applications, leveraging the power of Bootstrap 5.

[![NuGet](https://img.shields.io/nuget/v/WebSpark.Bootswatch.svg)](https://www.nuget.org/packages/WebSpark.Bootswatch/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

## Features

- Seamless integration of Bootswatch themes with ASP.NET Core applications.
- Built on Bootstrap 5, offering modern, responsive, and mobile-first design.
- **NEW!** Integrated theme switcher component with light/dark mode support.
- Easy runtime theme switching with cookie persistence.
- Includes custom themes (e.g., Mom, Texecon) alongside standard Bootswatch themes.
- Built-in caching mechanism for improved performance through StyleCache service.
- Fully documented with IntelliSense support.

## Benefits of Bootstrap 5

- **Responsive Design**: Build mobile-first, responsive web applications effortlessly.
- **Modern Components**: Access a wide range of pre-designed components.
- **Customizable**: Easily customize themes to match your branding.
- **No jQuery Dependency**: Bootstrap 5 removes the dependency on jQuery, making it lighter and faster.

## Installation

Install the package via NuGet Package Manager:

```shell
Install-Package WebSpark.Bootswatch
```

Or via .NET CLI:

```shell
dotnet add package WebSpark.Bootswatch
```

## Quick Start

### 1. Register Services

In the `Program.cs` file of your ASP.NET Core application, register the Bootswatch services:

```csharp
var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddRazorPages();

// Add Bootswatch theme switcher services (includes StyleCache)
builder.Services.AddBootswatchThemeSwitcher();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Use all Bootswatch features (includes StyleCache and static files)
app.UseBootswatchAll();

app.UseRouting();
app.MapRazorPages();

app.Run();
```

### 2. Update Your Layout

Modify your `_Layout.cshtml` file to use the theme switcher:

```html
@using WebSpark.Bootswatch.Services
@using WebSpark.Bootswatch.Helpers
@inject StyleCache StyleCache
<!DOCTYPE html>
<html lang="en" data-bs-theme="@(BootswatchThemeHelper.GetCurrentColorMode(HttpContext))">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>@ViewData["Title"] - WebSpark.Bootswatch.Demo</title>
    
    @{
        var themeName = BootswatchThemeHelper.GetCurrentThemeName(HttpContext);
        var themeUrl = BootswatchThemeHelper.GetThemeUrl(StyleCache, themeName);
    }
    
    <link id="bootswatch-theme-stylesheet" rel="stylesheet" href="@themeUrl" />
    <script src="/_content/WebSpark.Bootswatch/js/bootswatch-theme-switcher.js"></script>
</head>
<body>
    <!-- Your layout content -->
    
    <!-- Add the theme switcher where you want it to appear -->
    @Html.Raw(BootswatchThemeHelper.GetThemeSwitcherHtml(StyleCache, HttpContext))
    
    <!-- Content -->
    @RenderBody()
    
    <!-- Scripts -->
    <script src="~/lib/jquery/dist/jquery.min.js"></script>
    <script src="~/lib/bootstrap/dist/js/bootstrap.bundle.min.js"></script>
    @await RenderSectionAsync("Scripts", required: false)
</body>
</html>
```

No JavaScript implementation is needed - the theme switcher functionality is now built into the library!

## Advanced Usage

### Using the Integrated StyleCache Service

The package includes a built-in `StyleCache` service for improved performance:

```csharp
// In your Controller or Razor Page
using WebSpark.Bootswatch.Services;

public class HomeController : Controller
{
    private readonly StyleCache _styleCache;

    public HomeController(StyleCache styleCache)
    {
        _styleCache = styleCache;
    }

    public IActionResult Index()
    {
        // Get all available styles
        var styles = _styleCache.GetAllStyles();
        
        // Get a specific style by name
        var defaultStyle = _styleCache.GetStyle("default");
        
        return View();
    }
}
```

### Theme Switcher Helpers

The library now includes the `BootswatchThemeHelper` class with useful methods:

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

For more details on the theme switcher, see [ThemeSwitcherGuide.md](ThemeSwitcherGuide.md).

## Demo Project

For a complete example of how to integrate WebSpark.Bootswatch, refer to the `WebSpark.Bootswatch.Demo` project included in this repository. It demonstrates:

- Registering Bootswatch services.
- Using the theme switcher component.
- Applying themes dynamically in the layout.
- Using the StyleCache service for efficient theme management.

## License

This project is licensed under the MIT License - see the LICENSE file for details.
