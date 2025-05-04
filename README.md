# WebSpark.Bootswatch

A .NET Razor Class Library for integrating Bootstrap themes from [Bootswatch](https://bootswatch.com/) into ASP.NET Core applications. This library also includes custom themes and provides a simple way to switch between themes at runtime.

[![NuGet](https://img.shields.io/nuget/v/WebSpark.Bootswatch.svg)](https://www.nuget.org/packages/WebSpark.Bootswatch/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

## Features

- Integrates Bootswatch themes with ASP.NET Core applications
- Provides a simple interface to switch between themes at runtime
- Includes custom themes (mom, texecon) as well as standard Bootswatch themes
- Caches themes to improve performance
- Fully documented with XML comments for IntelliSense support

## Compatibility

This package targets:

- .NET 9.0
- .NET 8.0

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

Add the WebSpark.Bootswatch services to your application in `Program.cs`:

```csharp
var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddRazorPages();
builder.Services.AddBootswatchStyles(); // Add Bootswatch services

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseBootswatchStaticFiles(); // Enable Bootswatch static files

app.UseRouting();
app.MapRazorPages();

app.Run();
```

### 2. Create a Theme Switcher Component

Create a partial view `_ThemeSwitcher.cshtml` to allow users to change themes:

```html
@using WebSpark.Bootswatch.Model
@inject IStyleProvider StyleProvider

@{
    var styles = await StyleProvider.GetAsync();
}

<div class="dropdown">
    <button class="btn btn-secondary dropdown-toggle" type="button" data-bs-toggle="dropdown" aria-expanded="false">
        Change Theme
    </button>
    <ul class="dropdown-menu">
        @foreach (var style in styles)
        {
            <li><a class="dropdown-item" href="?theme=@style.name">@style.name</a></li>
        }
    </ul>
</div>
```

### 3. Use Theme in Layout

Update your `_Layout.cshtml` file to use the current theme:

```html
@using WebSpark.Bootswatch.Model
@inject IStyleProvider StyleProvider

@{
    var theme = Context.Request.Query["theme"].ToString() ?? "default";
    var currentStyle = await StyleProvider.GetAsync(theme);
}

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>@ViewData["Title"] - WebSpark.Bootswatch.Demo</title>
    
    <!-- Use the theme from Bootswatch -->
    @if (!string.IsNullOrEmpty(currentStyle.cssCdn))
    {
        <link rel="stylesheet" href="@currentStyle.cssCdn" />
    }
    else
    {
        <link rel="stylesheet" href="~/lib/bootstrap/dist/css/bootstrap.min.css" />
    }
    
    <link rel="stylesheet" href="~/css/site.css" asp-append-version="true" />
</head>
<body>
    <!-- Rest of your layout -->
    <partial name="_ThemeSwitcher" />
    
    <!-- Content -->
    @RenderBody()
    
    <!-- Scripts -->
    <script src="~/lib/jquery/dist/jquery.min.js"></script>
    <script src="~/lib/bootstrap/dist/js/bootstrap.bundle.min.js"></script>
    <script src="~/js/site.js" asp-append-version="true"></script>
    @await RenderSectionAsync("Scripts", required: false)
</body>
</html>
```

## Advanced Usage

### Caching Themes

For improved performance, you can implement a caching service:

```csharp
public class StyleCache
{
    private readonly IStyleProvider _styleProvider;
    private readonly IMemoryCache _memoryCache;
    private const string CacheKey = "BootswatchStyles";

    public StyleCache(IStyleProvider styleProvider, IMemoryCache memoryCache)
    {
        _styleProvider = styleProvider;
        _memoryCache = memoryCache;
    }

    public async Task<IEnumerable<StyleModel>> GetStylesAsync()
    {
        if (!_memoryCache.TryGetValue(CacheKey, out IEnumerable<StyleModel> styles))
        {
            styles = await _styleProvider.GetAsync();
            _memoryCache.Set(CacheKey, styles, TimeSpan.FromHours(1));
        }
        return styles;
    }
}
```

### Custom Theme Implementation

You can create your own theme provider by implementing the `IStyleProvider` interface:

```csharp
public class CustomStyleProvider : IStyleProvider
{
    public Task<IEnumerable<StyleModel>> GetAsync()
    {
        // Return your custom themes
    }

    public Task<StyleModel> GetAsync(string name)
    {
        // Return a specific theme by name
    }
}
```

## License

This project is licensed under the MIT License - see the LICENSE file for details.
