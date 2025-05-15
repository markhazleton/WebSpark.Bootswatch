using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebSpark.Bootswatch.Services;
using System.Text.Encodings.Web;

namespace WebSpark.Bootswatch.Helpers;

/// <summary>
/// Helper class for working with Bootswatch themes in views and layouts
/// </summary>
public static class BootswatchThemeHelper
{
    /// <summary>
    /// Gets the current theme name from the request cookies or returns the default
    /// </summary>
    /// <param name="context">The HTTP context</param>
    /// <param name="defaultTheme">The default theme name to use if no cookie is set</param>
    /// <returns>The current theme name</returns>
    public static string GetCurrentThemeName(HttpContext context, string defaultTheme = "default")
    {
        return context.Request.Cookies["bootswatch-theme"] ?? defaultTheme;
    }

    /// <summary>
    /// Gets the current color mode from the request cookies or returns the default
    /// </summary>
    /// <param name="context">The HTTP context</param>
    /// <param name="defaultMode">The default color mode to use if no cookie is set</param>
    /// <returns>The current color mode</returns>
    public static string GetCurrentColorMode(HttpContext context, string defaultMode = "light")
    {
        return context.Request.Cookies["bootswatch-color-mode"] ?? defaultMode;
    }

    /// <summary>
    /// Gets the URL for the current theme from the cache
    /// </summary>
    /// <param name="styleCache">The style cache service</param>
    /// <param name="themeName">The name of the theme</param>
    /// <param name="defaultUrl">The default URL to use if the theme isn't found</param>
    /// <returns>The URL for the theme's CSS</returns>
    public static string GetThemeUrl(StyleCache styleCache, string themeName, string defaultUrl = "/lib/bootstrap/dist/css/bootstrap.min.css")
    {
        if (themeName == "default")
            return defaultUrl;

        return styleCache.GetStyle(themeName)?.cssCdn ?? defaultUrl;
    }

    /// <summary>
    /// Provides the HTML markup for the theme switcher component
    /// </summary>
    /// <returns>HTML markup for the theme switcher component</returns>
    public static string GetThemeSwitcherHtml(StyleCache styleCache, HttpContext context)
    {
        var currentStyle = GetCurrentThemeName(context);
        var styles = styleCache.GetAllStyles();

        var html = @"<div class=""bootswatch-theme-switcher d-flex align-items-center"">
    <!-- Color Mode Toggle -->
    <div class=""me-2"">
        <button class=""btn btn-sm btn-outline-secondary"" id=""bootswatch-color-mode-toggle"" type=""button""
            aria-label=""Toggle color mode"">
            <i class=""bootswatch-color-mode-icon""></i>
            <span class=""bootswatch-color-mode-text""></span>
        </button>
    </div>

    <!-- Theme Dropdown -->
    <div class=""dropdown"">
        <button class=""btn btn-sm btn-outline-secondary dropdown-toggle"" type=""button"" id=""bootswatchThemeDropdown""
            data-bs-toggle=""dropdown"" aria-expanded=""false"">
            Theme
        </button>
        <ul class=""dropdown-menu dropdown-menu-end bootswatch-dropdown-menu"" aria-labelledby=""bootswatchThemeDropdown"">";

        foreach (var style in styles)
        {
            var activeClass = style.name == currentStyle ? "active" : "";
            html += $@"
            <li>
                <a class=""dropdown-item {activeClass}"" href=""#"" data-theme=""{HtmlEncode(style.name)}""
                    data-theme-url=""{HtmlEncode(style.cssCdn)}"">
                    {HtmlEncode(style.name)}
                </a>
            </li>";
        }

        html += @"
        </ul>
    </div>
</div>";

        return html;
    }

    private static string HtmlEncode(string? value)
    {
        if (string.IsNullOrEmpty(value))
            return string.Empty;

        return HtmlEncoder.Default.Encode(value);
    }

    /// <summary>
    /// Gets the HTML attributes needed for the theme-aware HTML tag
    /// </summary>
    /// <param name="context">The HTTP context</param>
    /// <param name="additionalAttributes">Any additional attributes to include</param>
    /// <returns>Dictionary of HTML attributes</returns>
    public static IDictionary<string, object> GetHtmlAttributes(HttpContext context, object? additionalAttributes = null)
    {
        var attributes = new Dictionary<string, object>
        {
            { "lang", "en" },
            { "data-bs-theme", GetCurrentColorMode(context) }
        };

        // Add any additional attributes
        if (additionalAttributes != null)
        {
            var properties = additionalAttributes.GetType().GetProperties();
            foreach (var prop in properties)
            {
                var value = prop.GetValue(additionalAttributes);
                if (value != null)
                {
                    attributes[prop.Name] = value;
                }
            }
        }

        return attributes;
    }

    /// <summary>
    /// Renders the Bootswatch theme switcher as a Tag Helper for easy integration.
    /// </summary>
    public static string RenderThemeSwitcherTagHelper(StyleCache styleCache, HttpContext context)
    {
        return GetThemeSwitcherHtml(styleCache, context);
    }
}