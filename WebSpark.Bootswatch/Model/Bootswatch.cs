using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WebSpark.Bootswatch.Model;

/// <summary>
/// BootswatchResponse Api Response Model
/// </summary>
public class BootswatchResponse
{
#pragma warning disable IDE1006 // Naming Styles
    /// <summary>
    /// Bootswatch API version
    /// </summary>
    public string? version { get; set; }
    /// <summary>
    /// List of available themes
    /// </summary>
    public List<BootswatchStyle>? themes { get; set; }
#pragma warning restore IDE1006 // Naming Styles
}

/// <summary>
/// Represents a collection of Bootswatch themes with version information
/// </summary>
public class BootswatchTheme
{
    /// <summary>
    /// Gets or sets the Bootswatch API version
    /// </summary>
    [JsonPropertyName("version")]
    public string version { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the collection of available Bootswatch themes
    /// </summary>
    [JsonPropertyName("themes")]
    public List<Theme> themes { get; set; } = new List<Theme>();
}

/// <summary>
/// Represents a single Bootswatch theme with all its assets and metadata
/// </summary>
public class Theme
{
    /// <summary>
    /// Gets or sets the name of the theme
    /// </summary>
    [JsonPropertyName("name")]
    public string name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the theme
    /// </summary>
    [JsonPropertyName("description")]
    public string description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the URL to the theme's thumbnail image
    /// </summary>
    [JsonPropertyName("thumbnail")]
    public string thumbnail { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the URL to the theme's preview page
    /// </summary>
    [JsonPropertyName("preview")]
    public string preview { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the URL to the theme's unminified CSS file
    /// </summary>
    [JsonPropertyName("css")]
    public string css { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the URL to the theme's minified CSS file
    /// </summary>
    [JsonPropertyName("cssMin")]
    public string cssMin { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the URL to the theme's CDN-hosted CSS file
    /// </summary>
    [JsonPropertyName("cssCdn")]
    public string cssCdn { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the URL to the theme's unminified right-to-left CSS file
    /// </summary>
    [JsonPropertyName("cssRtl")]
    public string cssRtl { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the URL to the theme's minified right-to-left CSS file
    /// </summary>
    [JsonPropertyName("cssRtlMin")]
    public string cssRtlMin { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the URL to the theme's CDN-hosted right-to-left CSS file
    /// </summary>
    [JsonPropertyName("cssRtlCdn")]
    public string cssRtlCdn { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the URL to the theme's SCSS source file
    /// </summary>
    [JsonPropertyName("scss")]
    public string scss { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the URL to the theme's SCSS variables file
    /// </summary>
    [JsonPropertyName("scssVariables")]
    public string scssVariables { get; set; } = string.Empty;
}