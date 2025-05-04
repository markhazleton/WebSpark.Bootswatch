namespace WebSpark.Bootswatch.Model;

/// <summary>
/// Represents a Bootswatch theme style with its metadata and resource URLs
/// </summary>
public class BootswatchStyle
{
#pragma warning disable IDE1006 // Naming Styles
    /// <summary>
    /// Gets or sets the name of the Bootswatch theme
    /// </summary>
    public string? name { get; set; }

    /// <summary>
    /// Gets or sets the description of the Bootswatch theme
    /// </summary>
    public string? description { get; set; }

    /// <summary>
    /// Gets or sets the URL to the theme's thumbnail image
    /// </summary>
    public string? thumbnail { get; set; }

    /// <summary>
    /// Gets or sets the URL to the theme's preview page
    /// </summary>
    public string? preview { get; set; }

    /// <summary>
    /// Gets or sets the URL to the theme's unminified CSS file
    /// </summary>
    public string? css { get; set; }

    /// <summary>
    /// Gets or sets the URL to the theme's minified CSS file
    /// </summary>
    public string? cssMin { get; set; }

    /// <summary>
    /// Gets or sets the URL to the theme's CDN-hosted CSS file
    /// </summary>
    public string? cssCdn { get; set; }

    /// <summary>
    /// Gets or sets the URL to the theme's LESS source file
    /// </summary>
    public string? less { get; set; }

    /// <summary>
    /// Gets or sets the URL to the theme's LESS variables file
    /// </summary>
    public string? lessVariables { get; set; }

    /// <summary>
    /// Gets or sets the URL to the theme's SCSS source file
    /// </summary>
    public string? scss { get; set; }

    /// <summary>
    /// Gets or sets the URL to the theme's SCSS variables file
    /// </summary>
    public string? scssVariables { get; set; }
#pragma warning restore IDE1006 // Naming Styles
}
