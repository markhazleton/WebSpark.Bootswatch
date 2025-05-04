# WebSpark.Bootswatch Installation Guide

![WebSpark Bootswatch Logo](WebSpark.png)

## Overview

This guide provides comprehensive instructions for installing, configuring, and using the WebSpark.Bootswatch package in ASP.NET Core applications. WebSpark.Bootswatch integrates [Bootswatch](https://bootswatch.com/) themes with your application, enabling dynamic theme switching and customization with Bootstrap 5.

[![NuGet](https://img.shields.io/nuget/v/WebSpark.Bootswatch.svg)](https://www.nuget.org/packages/WebSpark.Bootswatch/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

**Last Updated:** May 4, 2025

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
```

Register Bootswatch services in the dependency injection container:

```csharp
var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddRazorPages(); // or AddControllersWithViews() for MVC

// Register Bootswatch services
builder.Services.AddBootswatchStyles();

var app = builder.Build();
```

### 2. Configure Middleware

Configure the HTTP request pipeline to use Bootswatch static files:

```csharp
// Configure middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// Register Bootswatch static files middleware BEFORE standard static files
app.UseBootswatchStaticFiles();

// Standard static files middleware
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();
app.MapRazorPages(); // or MapControllerRoute() for MVC

app.Run();
```

> **Important**: `app.UseBootswatchStaticFiles()` must be called before `app.UseStaticFiles()` to ensure proper serving of embedded static files.

## Advanced Configuration

### 1. Create StyleCache Service

Create a `StyleCache.cs` file in your Services folder to cache Bootswatch styles:

```csharp
using Microsoft.Extensions.Logging;
using WebSpark.Bootswatch.Model;
using WebSpark.Bootswatch.Provider;

namespace YourApp.Services;

public class StyleCache
{
    private readonly List<StyleModel> _styles = new();
    private readonly IServiceProvider _serviceProvider;
    private bool _isInitialized = false;
    private readonly object _lockObject = new();
    private Task? _initializationTask = null;
    private readonly ILogger<StyleCache>? _logger;

    public StyleCache(IServiceProvider serviceProvider, ILogger<StyleCache>? logger = null)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public List<StyleModel> GetAllStyles()
    {
        // If we're not initialized yet but loading has started, wait for a maximum of 3 seconds
        if (!_isInitialized && _initializationTask != null)
        {
            try
            {
                if (Task.WaitAny(new[] { _initializationTask }, TimeSpan.FromSeconds(3)) == 0)
                {
                    // Task completed successfully
                }
                else
                {
                    _logger?.LogWarning("Timed out waiting for styles to load. Returning default styles.");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error waiting for style initialization");
            }
        }

        // Even if initialization hasn't completed, return what we have
        lock (_lockObject)
        {
            return _styles.ToList(); // Return a copy to avoid potential thread issues
        }
    }

    public StyleModel GetStyle(string name)
    {
        var styles = GetAllStyles();
        return styles.FirstOrDefault(s => s.name == name) ?? new StyleModel();
    }

    public void StartInitialization()
    {
        if (_initializationTask == null)
        {
            lock (_lockObject)
            {
                if (_initializationTask == null)
                {
                    _initializationTask = Task.Run(async () =>
                    {
                        try
                        {
                            await LoadStyles();
                            _isInitialized = true;
                            _logger?.LogInformation("StyleCache successfully initialized with {Count} styles", _styles.Count);
                        }
                        catch (Exception ex)
                        {
                            _logger?.LogError(ex, "Error during StyleCache initialization");
                        }
                    });
                }
            }
        }
    }

    public async Task LoadStyles()
    {
        // Create a scope to resolve the IStyleProvider since it's registered as scoped
        using var scope = _serviceProvider.CreateScope();
        var styleProvider = scope.ServiceProvider.GetRequiredService<IStyleProvider>();

        // Get styles from the provider
        var styles = await styleProvider.GetAsync();

        // Clear and populate the styles collection
        lock (_lockObject)
        {
            _styles.Clear();
            _styles.AddRange(styles);
        }
    }

    public static void InitializeInBackground(IServiceProvider serviceProvider)
    {
        var styleCache = serviceProvider.GetRequiredService<StyleCache>();
        styleCache.StartInitialization();
    }
}
```

### 2. Register and Initialize StyleCache

Add these lines to your `Program.cs` file:

```csharp
// Register StyleCache as a singleton
builder.Services.AddSingleton<StyleCache>();
builder.Services.AddLogging();

var app = builder.Build();

// Initialize StyleCache in the background without blocking application startup
StyleCache.InitializeInBackground(app.Services);
```

### 3. Custom HTTP Request Implementation (Optional)

If you need custom HTTP request handling:

```csharp
// Use custom implementation for HTTP requests
builder.Services.AddScoped<IHttpRequestResultService, HttpRequestResultService>();
```

## Theme Switching Implementation

### 1. Update Layout for Dynamic Themes

Modify your `_Layout.cshtml` file to dynamically load the selected theme:

```cshtml
@using YourApp.Services
@using WebSpark.Bootswatch.Model
@inject StyleCache StyleCache
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>@ViewData["Title"] - Your App</title>

    @{
        var themeName = Context.Request.Cookies["bootswatch-theme"] ?? "default";
        var themeUrl = themeName == "default" ?
            "/lib/bootstrap/dist/css/bootstrap.min.css" :
            StyleCache.GetStyle(themeName)?.cssCdn ?? "/lib/bootstrap/dist/css/bootstrap.min.css";
    }

    <link id="theme-stylesheet" rel="stylesheet" href="@themeUrl" />
    <link rel="stylesheet" href="~/css/site.css" asp-append-version="true" />
    <link rel="stylesheet" href="~/YourApp.styles.css" asp-append-version="true" />
</head>
<body>
    <!-- Header with navigation -->
    <header>
        <nav class="navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-white border-bottom box-shadow mb-3">
            <div class="container">
                <!-- Your navigation items -->
                
                <!-- Theme switcher component -->
                <partial name="_ThemeSwitcher" />
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

### 2. Create Theme Switcher Component

Create a `_ThemeSwitcher.cshtml` partial view in your Pages/Shared folder:

```cshtml
@using YourApp.Services
@inject StyleCache StyleCache

<div class="dropdown theme-switcher">
    <button class="btn btn-outline-secondary dropdown-toggle" type="button" data-bs-toggle="dropdown" aria-expanded="false">
        Theme
    </button>
    <ul class="dropdown-menu">
        <li><a class="dropdown-item" href="#" onclick="setTheme('default')">Default</a></li>
        @foreach (var style in StyleCache.GetAllStyles())
        {
            <li><a class="dropdown-item" href="#" onclick="setTheme('@style.name')">@style.name</a></li>
        }
    </ul>
</div>

<script>
    function setTheme(themeName) {
        // Set cookie to remember theme choice
        document.cookie = "bootswatch-theme=" + themeName + "; path=/; max-age=31536000"; // 1 year expiry
        
        // Update CSS link
        var themeUrl = themeName === 'default' 
            ? '/lib/bootstrap/dist/css/bootstrap.min.css' 
            : '@StyleCache.GetAllStyles().FirstOrDefault()?.cssCdn'.replace('@StyleCache.GetAllStyles().FirstOrDefault()?.name', themeName);
        
        document.getElementById('theme-stylesheet').href = themeUrl;
        
        return false;
    }
</script>
```

## Caching Implementation

### 1. Add Cache Control Headers

Enhance performance by adding cache control headers for static files:

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

### 2. Add Logging for Troubleshooting

Add middleware to log theme loading issues:

```csharp
app.Use(async (context, next) =>
{
    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
    var path = context.Request.Path.Value;

    // Log all requests that look like they're trying to access theme CSS files
    if (path?.Contains("bootstrap.min.css") == true)
    {
        logger.LogInformation("Requested theme CSS: {Path}", path);
    }

    await next();

    // Log if the response is a 404 for theme CSS files
    if (context.Response.StatusCode == 404 && path?.Contains("bootstrap.min.css") == true)
    {
        logger.LogWarning("404 Not Found for theme CSS: {Path}", path);
    }
});
```

## Troubleshooting

### Theme Not Loading

1. **Check Network Requests**  
   Use browser developer tools (F12) to inspect network requests for CSS files.

2. **Verify Middleware Order**  
   Ensure `UseBootswatchStaticFiles()` is called before `UseStaticFiles()`.

3. **Check Cache Initialization**  
   Verify StyleCache initializes correctly by checking application logs.

4. **Clear Browser Cache**  
   Hard refresh with Ctrl+F5 to clear cached CSS files.

### Missing Themes

1. **Check API Connection**  
   Ensure your application can reach the Bootswatch API.

2. **Inspect Logs**  
   Look for any error messages during StyleCache initialization.

3. **Add Fallback Themes**  
   Modify the StyleCache to include default themes if API calls fail.

## Best Practices

1. **Non-Blocking Initialization**  
   Initialize the StyleCache in the background to avoid blocking application startup.

2. **Error Handling**  
   Always provide fallback styles if theme loading fails.

3. **User Preferences**  
   Store user theme preferences in cookies or user profile for persistence.

4. **Performance Optimization**  
   Cache theme styles to reduce API calls and improve page load times.

5. **CDN Fallbacks**  
   Implement fallbacks for CDN-hosted theme files in case of CDN failures.

6. **Responsive Testing**  
   Test themes across different screen sizes to ensure responsive behavior.

## Demo Application

The `WebSpark.Bootswatch.Demo` application in this repository provides a complete reference implementation showcasing all features discussed in this guide, including:

- Theme switching
- Caching
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
