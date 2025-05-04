# WebSpark.Bootswatch Documentation

WebSpark.Bootswatch is a .NET Razor Class Library that integrates [Bootswatch](https://bootswatch.com/) themes into ASP.NET Core applications. This library allows you to easily switch between multiple Bootstrap themes at runtime.

## Installation

Install via NuGet Package Manager:

```
Install-Package WebSpark.Bootswatch
```

Or via .NET CLI:

```
dotnet add package WebSpark.Bootswatch
```

## Setup

### 1. Add the WebSpark.Bootswatch services in your Program.cs

```csharp
builder.Services.AddBootswatch();
```

### 2. Register the theme service in your Program.cs

```csharp
builder.Services.AddScoped<IStyleProvider, BootswatchStyleProvider>();
```

### 3. Add the static file middleware to serve Bootswatch styles

```csharp
app.UseStaticFiles();
```

## Usage

### Including Styles in Layout

Add the following to your `_Layout.cshtml` file:

```html
@inject WebSpark.Bootswatch.Model.IStyleProvider StyleProvider
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>@ViewData["Title"]</title>
    
    <!-- Reference the current Bootswatch theme -->
    <link rel="stylesheet" href="@StyleProvider.GetCurrentStyleUrl()" />
    
    <!-- Your other CSS references -->
</head>
<body>
    <!-- Your layout content -->
</body>
</html>
```

### Switching Themes

#### Option 1: Via Controller

```csharp
[HttpGet("SetTheme/{name}")]
public IActionResult SetTheme(string name)
{
    // Access the IStyleProvider to change the theme
    var styleProvider = HttpContext.RequestServices.GetRequiredService<IStyleProvider>();
    styleProvider.SetCurrentStyle(name);
    
    // Redirect back to the previous page
    return Redirect(Request.Headers["Referer"].ToString());
}
```

#### Option 2: Via Razor Page

```csharp
public class IndexModel : PageModel
{
    private readonly IStyleProvider _styleProvider;

    public IndexModel(IStyleProvider styleProvider)
    {
        _styleProvider = styleProvider;
    }
    
    public IActionResult OnPostChangeTheme(string themeName)
    {
        _styleProvider.SetCurrentStyle(themeName);
        return RedirectToPage();
    }
}
```

### Creating a Theme Selector

To allow users to switch themes, create a dropdown menu:

```html
@inject WebSpark.Bootswatch.Model.IStyleProvider StyleProvider

<div class="dropdown">
    <button class="btn btn-secondary dropdown-toggle" type="button" id="themeDropdown" data-bs-toggle="dropdown" aria-expanded="false">
        Themes
    </button>
    <ul class="dropdown-menu" aria-labelledby="themeDropdown">
        @foreach (var style in StyleProvider.GetAvailableStyles())
        {
            <li>
                <a class="dropdown-item @(style.Name == StyleProvider.GetCurrentStyleName() ? "active" : "")" 
                   href="@Url.Action("SetTheme", "Home", new { name = style.Name })">
                    @style.DisplayName
                </a>
            </li>
        }
    }
    </ul>
</div>
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

### Caching

For better performance, consider using a caching service:

```csharp
public class StyleCache : IStyleCache
{
    private readonly IMemoryCache _cache;
    
    public StyleCache(IMemoryCache cache)
    {
        _cache = cache;
    }
    
    public BootswatchStyle GetCachedStyle(string styleName)
    {
        return _cache.GetOrCreate($"style_{styleName}", entry => {
            entry.SlidingExpiration = TimeSpan.FromHours(1);
            // Load and return the style
        });
    }
}
```

### Custom Theme Implementation

You can implement your own themes by creating a custom `StyleProvider`:

```csharp
public class CustomStyleProvider : IStyleProvider
{
    private readonly List<StyleModel> _styles = new();
    
    public CustomStyleProvider()
    {
        // Add your custom themes
        _styles.Add(new StyleModel { Name = "custom-theme", DisplayName = "Custom Theme", Url = "/css/custom-theme.css" });
    }
    
    // Implement the interface methods
}
```

## License

WebSpark.Bootswatch is licensed under the MIT License. See the LICENSE file for details.
