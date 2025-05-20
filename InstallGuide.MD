# WebSpark.Bootswatch Installation Guide

![WebSpark Bootswatch Logo](WebSpark.png)

## Overview

This guide provides comprehensive instructions for installing, configuring, and using the WebSpark.Bootswatch package in ASP.NET Core applications. WebSpark.Bootswatch integrates [Bootswatch](https://bootswatch.com/) themes with your application, enabling dynamic theme switching and customization with Bootstrap 5.

[![NuGet](https://img.shields.io/nuget/v/WebSpark.Bootswatch.svg)](https://www.nuget.org/packages/WebSpark.Bootswatch/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

**Last Updated:** May 20, 2025

## Current version: 1.10.2

## Important Links

- **Demo Site**: [https://bootswatch.markhazleton.com/](https://bootswatch.markhazleton.com/)
- **NuGet Package**: [https://www.nuget.org/packages/WebSpark.Bootswatch](https://www.nuget.org/packages/WebSpark.Bootswatch)
- **GitHub Repository**: [https://github.com/MarkHazleton/WebSpark.Bootswatch](https://github.com/MarkHazleton/WebSpark.Bootswatch)

## Table of Contents

- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Basic Configuration](#basic-configuration)
- [Advanced Configuration](#advanced-configuration)
- [Theme Switching Implementation](#theme-switching-implementation)
- [Caching Implementation](#caching-implementation)
- [Troubleshooting](#troubleshooting)
- [Best Practices](#best-practices)

## Prerequisites

Before installing WebSpark.Bootswatch, ensure you have:

- .NET SDK 8.0 or later
- An ASP.NET Core web application (MVC or Razor Pages)
- Basic understanding of Bootstrap framework

## Installation

### Via NuGet Package Manager

```powershell
Install-Package WebSpark.Bootswatch
```

### Via .NET CLI

```bash
dotnet add package WebSpark.Bootswatch
```

### Via Project Reference

For development or when using a local version, add a project reference to your .csproj file:

```xml
<ItemGroup>
  <ProjectReference Include="..\WebSpark.Bootswatch\WebSpark.Bootswatch.csproj" />
</ItemGroup>
```

## Basic Configuration

### 1. Register Services

Add the necessary using statements at the top of your `Program.cs` file:

```csharp
using WebSpark.Bootswatch;
using WebSpark.Bootswatch.Provider;
using WebSpark.Bootswatch.Model;
using WebSpark.Bootswatch.Services;
using WebSpark.Bootswatch.Helpers;
```

Register Bootswatch services in the dependency injection container:

```csharp
var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddRazorPages(); // or AddControllersWithViews() for MVC

// Register Bootswatch theme switcher services (includes StyleCache)
builder.Services.AddBootswatchThemeSwitcher();

var app = builder.Build();
```

### 2. Configure Middleware

Configure the HTTP request pipeline to use Bootswatch features:

```csharp
// Configure middleware
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
app.UseAuthorization();
app.MapRazorPages(); // or MapControllerRoute() for MVC

app.Run();
```

## Advanced Configuration

### Using the StyleCache Service

WebSpark.Bootswatch includes a built-in StyleCache service that is automatically registered when you call `AddBootswatchThemeSwitcher()`:

```csharp
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
        // Access all available styles
        var allStyles = _styleCache.GetAllStyles();
        
        // Get a specific style by name
        var currentStyle = _styleCache.GetStyle("darkly");
        
        return View(new { AllStyles = allStyles, CurrentStyle = currentStyle });
    }
}
```

### Custom HTTP Request Implementation (Optional)

If you need custom HTTP request handling:

```csharp
// Use custom implementation for HTTP requests
builder.Services.AddScoped<IHttpRequestResultService, HttpRequestResultService>();
```

## Theme Switching Implementation

### 1. Update Layout for Dynamic Themes

Modify your `_Layout.cshtml` file to use the theme switcher:

```cshtml
@using WebSpark.Bootswatch.Services
@using WebSpark.Bootswatch.Helpers
@inject StyleCache StyleCache
<!DOCTYPE html>
<html lang="en" data-bs-theme="@(BootswatchThemeHelper.GetCurrentColorMode(HttpContext))">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>@ViewData["Title"] - Your App</title>

    @{
        var themeName = BootswatchThemeHelper.GetCurrentThemeName(HttpContext);
        var themeUrl = BootswatchThemeHelper.GetThemeUrl(StyleCache, themeName);
    }

    <link id="bootswatch-theme-stylesheet" rel="stylesheet" href="@themeUrl" />
    <script src="/_content/WebSpark.Bootswatch/js/bootswatch-theme-switcher.js"></script>
</head>
<body>
    <!-- Header with navigation -->
    <header>
        <nav class="navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-white border-bottom box-shadow mb-3">
            <div class="container">
                <!-- Your navigation items -->
                
                <!-- Theme switcher component -->
                @Html.Raw(BootswatchThemeHelper.GetThemeSwitcherHtml(StyleCache, HttpContext))
            </div>
        </nav>
    </header>
    
    <!-- Main content container -->
    <div class="container">
        <main role="main" class="pb-3">
            @RenderBody()
        </main>
    </div>
    
    <!-- Footer -->
    <footer class="border-top footer text-muted">
        <div class="container">
            &copy; @DateTime.Now.Year - Your App
        </div>
    </footer>
    
    <!-- Scripts -->
    <script src="~/lib/jquery/dist/jquery.min.js"></script>
    <script src="~/lib/bootstrap/dist/js/bootstrap.bundle.min.js"></script>
    <script src="~/js/site.js" asp-append-version="true"></script>
    
    @await RenderSectionAsync("Scripts", required: false)
</body>
</html>
```

### 2. Theme Switcher Helper Methods

The `BootswatchThemeHelper` class provides several useful methods:

```csharp
// Get the current theme name from cookies
var themeName = BootswatchThemeHelper.GetCurrentThemeName(HttpContext);

// Get the current color mode (light/dark) from cookies
var colorMode = BootswatchThemeHelper.GetCurrentColorMode(HttpContext);

// Get the URL for a theme's CSS
var themeUrl = BootswatchThemeHelper.GetThemeUrl(StyleCache, themeName);

// Generate HTML for the theme switcher component
var switcherHtml = BootswatchThemeHelper.GetThemeSwitcherHtml(StyleCache, HttpContext);

// Get HTML attributes for the HTML tag with theme support
var htmlAttributes = BootswatchThemeHelper.GetHtmlAttributes(HttpContext);
```

See [ThemeSwitcherGuide.md](ThemeSwitcherGuide.md) for more details on theme switching.

## Caching Implementation

The WebSpark.Bootswatch package includes a built-in `StyleCache` service that automatically caches styles to improve performance. The StyleCache handles:

1. **Efficient style retrieval** - Styles are loaded once and cached for subsequent requests
2. **Thread-safe operations** - The cache is designed to be used in multi-threaded environments
3. **Memory management** - Optimized for minimal memory footprint

You don't need to implement your own caching logic anymore - simply inject and use the `StyleCache` service.

For custom cache control, you can still add cache headers to your static files:

```csharp
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        // Cache static files for 1 day
        ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=86400");
    }
});
```

## Troubleshooting

### Theme Not Loading

1. **Check Network Requests**  
   Use browser developer tools (F12) to inspect network requests for CSS files.

2. **Verify Middleware Order**  
   Ensure you're using `app.UseBootswatchAll()` after `app.UseStaticFiles()`.

3. **Check StyleCache Usage**  
   Make sure you're injecting and using the StyleCache correctly.

4. **Clear Browser Cache**  
   Hard refresh with Ctrl+F5 to clear cached CSS files.

### Missing Themes

1. **Check API Connection**  
   Ensure your application can reach the Bootswatch API.

2. **Inspect Logs**  
   Look for any error messages during StyleCache initialization.

3. **Add Fallback Themes**  
   Always use the fallback mechanism with conditional checks as shown in the examples.

## Best Practices

1. **Default Theme Fallback**  
   Always use the `BootswatchThemeHelper.GetThemeUrl()` method which includes fallback handling:

   ```csharp
   var themeUrl = BootswatchThemeHelper.GetThemeUrl(StyleCache, themeName);
   ```

2. **User Preferences**  
   Use the built-in cookie mechanism for storing and retrieving user theme preferences.

3. **Performance Optimization**  
   Use the built-in StyleCache for optimal performance.

4. **CDN Fallbacks**  
   Implement fallbacks for CDN-hosted theme files in case of CDN failures.

5. **Responsive Testing**  
   Test themes across different screen sizes to ensure responsive behavior.

## Demo Application

The `WebSpark.Bootswatch.Demo` application in this repository provides a complete reference implementation showcasing all features discussed in this guide, including:

- Theme switching
- Light/dark mode toggle
- StyleCache usage
- Proper middleware configuration
- Responsive layouts
- Bootstrap 5 components

## Further Resources

- [Bootstrap 5 Documentation](https://getbootstrap.com/docs/5.0/)
- [Bootswatch Themes](https://bootswatch.com/)
- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core/)

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

&copy; 2025 WebSpark. All rights reserved.
