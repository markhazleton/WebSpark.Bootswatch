# Theme Switcher Implementation Guide

WebSpark.Bootswatch includes a built-in theme switcher and light/dark mode functionality that can be easily integrated into any ASP.NET Core application. This guide explains how to use these features.

## Features

- **Theme Switching**: Users can select from all available Bootswatch themes
- **Light/Dark Mode**: Toggle between light and dark color schemes
- **Cookie Persistence**: User preferences are saved in cookies
- **Simple Integration**: Add to any ASP.NET Core application with minimal code

## Implementation Steps

### 1. Register Services

In your `Program.cs` file, add the required services:

```csharp
// Add Bootswatch theme switcher services (includes StyleCache)
builder.Services.AddBootswatchThemeSwitcher();

// Later in the pipeline configuration:
// Use all Bootswatch features (includes StyleCache and static files)
app.UseBootswatchAll();
```

### 2. Modify Your Layout

Update your layout file to include the theme stylesheet and JavaScript:

```html
@using WebSpark.Bootswatch.Services
@using WebSpark.Bootswatch.Helpers
@inject StyleCache StyleCache
<!DOCTYPE html>
<html lang="en" data-bs-theme="@(BootswatchThemeHelper.GetCurrentColorMode(HttpContext))">
<head>
    <!-- Meta tags, title, etc. -->
    
    @{
        var themeName = BootswatchThemeHelper.GetCurrentThemeName(HttpContext);
        var themeUrl = BootswatchThemeHelper.GetThemeUrl(StyleCache, themeName);
    }
    
    <!-- Include the theme stylesheet with the correct ID -->
    <link id="bootswatch-theme-stylesheet" rel="stylesheet" href="@themeUrl" />
    
    <!-- Include the theme switcher JavaScript -->
    <script src="/_content/WebSpark.Bootswatch/js/bootswatch-theme-switcher.js"></script>
</head>
<body>
    <!-- Your layout content -->
    
    <!-- Add the theme switcher component where you want it to appear -->
    @Html.Raw(BootswatchThemeHelper.GetThemeSwitcherHtml(StyleCache, HttpContext))
</body>
</html>
```

## Helper Methods

The `BootswatchThemeHelper` class provides methods to help with theme implementation:

- `GetCurrentThemeName(HttpContext)`: Gets the current theme name from cookies
- `GetCurrentColorMode(HttpContext)`: Gets the current color mode (light/dark) from cookies
- `GetThemeUrl(StyleCache, themeName)`: Gets the URL for a theme's CSS file
- `GetThemeSwitcherHtml(StyleCache, HttpContext)`: Generates HTML for the theme switcher component
- `GetHtmlAttributes(HttpContext, additionalAttributes)`: Gets HTML attributes for the HTML tag with theme support

## How It Works

The theme switcher functionality works through several components:

1. **Cookie Storage**: Theme preferences are stored in cookies:
   - `bootswatch-theme`: Stores the selected theme name
   - `bootswatch-color-mode`: Stores the selected color mode (light/dark)

2. **JavaScript Handling**: The included JavaScript file handles theme switching without requiring page reloads:
   - Updates the theme stylesheet href
   - Toggles the color mode attribute
   - Sets cookies for persistence
   - Updates UI to reflect current selections

3. **HTML Generation**: The `GetThemeSwitcherHtml` method generates a Bootstrap-compatible dropdown with all available themes and a color mode toggle button.

## Customization

You can customize the theme switcher by adding your own CSS. The theme switcher HTML uses these CSS classes:

- `.bootswatch-theme-switcher`: The main container
- `.bootswatch-color-mode-icon`: The icon for the color mode toggle
- `.bootswatch-color-mode-text`: The text for the color mode toggle
- `.bootswatch-dropdown-menu`: The dropdown menu for themes

## Static Files

All theme switcher files are served as static files from the path `/_content/WebSpark.Bootswatch/`.

## Example Implementation

For a complete working example, see the `ThemeSwitcherExample.cshtml` file in the demo project.
