# WebSpark.Bootswatch

A .NET Razor Class Library for integrating Bootstrap 5 themes from [Bootswatch](https://bootswatch.com/) into ASP.NET Core applications. This library simplifies the process of applying modern, responsive themes to your web applications, leveraging the power of Bootstrap 5.

[![NuGet](https://img.shields.io/nuget/v/WebSpark.Bootswatch.svg)](https://www.nuget.org/packages/WebSpark.Bootswatch/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

## Quick Links

- **Demo Site**: [https://bootswatch.markhazleton.com/](https://bootswatch.markhazleton.com/)
- **NuGet Package**: [https://www.nuget.org/packages/WebSpark.Bootswatch](https://www.nuget.org/packages/WebSpark.Bootswatch)
- **GitHub Repository**: [https://github.com/MarkHazleton/WebSpark.Bootswatch](https://github.com/MarkHazleton/WebSpark.Bootswatch)

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

In the `Program.cs` file of your ASP.NET Core application, register the Bootswatch services. If you use the Tag Helper, also add `AddHttpContextAccessor()`:

```csharp
var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddRazorPages();

// (Optional) If you use the Tag Helper, add HttpContextAccessor
builder.Services.AddHttpContextAccessor();

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

// Use all Bootswatch features (includes StyleCache and static files)
app.UseBootswatchAll();

// (Optional) Add custom static file logging middleware if desired
// app.Use(...)

app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();
app.Run();
```

### 2. Update Your Layout

In your `_Layout.cshtml`, place the theme switcher Tag Helper outside the `<ul class="navbar-nav flex-grow-1">` and after the navigation links, as shown in the demo:

```html
<!-- ...existing nav markup... -->
<ul class="navbar-nav flex-grow-1">
    <!-- nav items -->
</ul>
<bootswatch-theme-switcher />
<!-- ...rest of layout... -->
```

This ensures the theme switcher appears in the correct place in the navigation bar, matching the demo project.

Register the Tag Helper in your `_ViewImports.cshtml`:

```csharp
@addTagHelper *, WebSpark.Bootswatch
```

If you use the manual method, do not place both the Tag Helper and the manual HTML helper in the same layout to avoid duplicate selectors.

**Manual alternative:**

```html
@Html.Raw(BootswatchThemeHelper.GetThemeSwitcherHtml(StyleCache, Context))
```

The rest of your layout setup remains the same. See the included sample layout file for a full example.

No JavaScript implementation is needed—the theme switcher functionality is built into the library!

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

The library includes the `BootswatchThemeHelper` class with useful methods. In Razor Pages, use `Context`:

```csharp
// Get the current theme name
var themeName = BootswatchThemeHelper.GetCurrentThemeName(Context);

// Get the current color mode (light/dark)
var colorMode = BootswatchThemeHelper.GetCurrentColorMode(Context);

// Get the URL for a theme
var themeUrl = BootswatchThemeHelper.GetThemeUrl(StyleCache, themeName);

// Get HTML for the theme switcher component
var switcherHtml = BootswatchThemeHelper.GetThemeSwitcherHtml(StyleCache, Context);
```

For more details on the theme switcher, see [ThemeSwitcherGuide.md](ThemeSwitcherGuide.md).

## How to Integrate WebSpark.Bootswatch into an Existing ASP.NET Core Web Application

Follow these steps to add Bootswatch theme support and the integrated theme switcher to your existing ASP.NET Core Razor Pages or MVC project:

### 1. Install the NuGet Package

Install the package via NuGet Package Manager or .NET CLI:

```shell
Install-Package WebSpark.Bootswatch
```

or

```shell
dotnet add package WebSpark.Bootswatch
```

### 2. Register Bootswatch Services

In your `Program.cs`, register the Bootswatch services after adding Razor Pages:

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

// Use all Bootswatch features (includes StyleCache and static files)
app.UseBootswatchAll();

app.UseRouting();
app.MapRazorPages();

app.Run();
```

### 3. Update Your Layout File

In your main layout file (e.g., `_Layout.cshtml`), add the following:

- Inject the `StyleCache` service.
- Use the `BootswatchThemeHelper` methods with `Context` (for Razor Pages) or `HttpContext` (for MVC) to set the theme and render the switcher.
- Add the Bootswatch theme switcher JavaScript.

Example for Razor Pages:

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
    @Html.Raw(BootswatchThemeHelper.GetThemeSwitcherHtml(StyleCache, Context))
    @RenderBody()
    <script src="~/lib/jquery/dist/jquery.min.js"></script>
    <script src="~/lib/bootstrap/dist/js/bootstrap.bundle.min.js"></script>
    @await RenderSectionAsync("Scripts", required: false)
</body>
</html>
```

### 4. (Optional) Use the StyleCache Service in Controllers or Pages

You can inject and use the `StyleCache` service to access available styles or theme information in your controllers or pages.

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
        var styles = _styleCache.GetAllStyles();
        var defaultStyle = _styleCache.GetStyle("default");
        return View();
    }
}
```

## Minimal Startup Example

Add these lines to your `Program.cs`:

```csharp
builder.Services.AddBootswatchThemeSwitcher();
app.UseBootswatchAll();
```

## Tag Helper Usage (Optional)

You can use the built-in Tag Helper to render the theme switcher in your layout:

```html
<bootswatch-theme-switcher />
```

Register the Tag Helper in your `_ViewImports.cshtml`:

```csharp
@addTagHelper *, WebSpark.Bootswatch
```

## Sample Layout File

A sample layout file is included in the NuGet package at:

```
contentFiles/any/any/BootswatchLayoutExample.cshtml
```

Copy or reference this file to quickly set up your layout.

## Demo Project

For a complete example of how to integrate WebSpark.Bootswatch, refer to the `WebSpark.Bootswatch.Demo` project included in this repository. It demonstrates:

- Registering Bootswatch services.
- Using the theme switcher component.
- Applying themes dynamically in the layout.
- Using the StyleCache service for efficient theme management.

## Release Notes

### v1.10.1 (2025-05-18)

- Improved logging and diagnostics for static file and theme CSS requests in the demo project.
- Updated `Privacy.cshtml.cs` to log the current color mode and use required members for better compatibility.
- Minor code cleanup and documentation improvements in the demo and main library.
- Updated project files for compatibility with .NET 9 (demo) and .NET 7 (library).
- No breaking changes; all integration steps remain the same as v1.10.0.

### v1.10.0 (2025-05-15)

- Added a Bootswatch theme switcher Tag Helper (`<bootswatch-theme-switcher />`) for easy integration in layouts and navigation bars.
- Included a sample layout file in the NuGet package for quick reference and copy-paste setup.
- Updated documentation and demo to use the Tag Helper as the preferred method.
- Added instructions and support for `AddHttpContextAccessor()` when using the Tag Helper.
- Improved README and install guide to clarify integration steps and best practices.
- Ensured static files and theme switcher JS are always available and documented correct middleware order.
- Cleaned up demo layout to avoid duplicate theme switchers and match recommended usage.

## License

This project is licensed under the MIT License - see the LICENSE file for details.
