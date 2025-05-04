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

public class BootswatchTheme
{
    [JsonPropertyName("version")]
    public string version { get; set; } = string.Empty;

    [JsonPropertyName("themes")]
    public List<Theme> themes { get; set; } = new List<Theme>();
}

public class Theme
{
    [JsonPropertyName("name")]
    public string name { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string description { get; set; } = string.Empty;

    [JsonPropertyName("thumbnail")]
    public string thumbnail { get; set; } = string.Empty;

    [JsonPropertyName("preview")]
    public string preview { get; set; } = string.Empty;

    [JsonPropertyName("css")]
    public string css { get; set; } = string.Empty;

    [JsonPropertyName("cssMin")]
    public string cssMin { get; set; } = string.Empty;

    [JsonPropertyName("cssCdn")]
    public string cssCdn { get; set; } = string.Empty;

    [JsonPropertyName("cssRtl")]
    public string cssRtl { get; set; } = string.Empty;

    [JsonPropertyName("cssRtlMin")]
    public string cssRtlMin { get; set; } = string.Empty;

    [JsonPropertyName("cssRtlCdn")]
    public string cssRtlCdn { get; set; } = string.Empty;

    [JsonPropertyName("scss")]
    public string scss { get; set; } = string.Empty;

    [JsonPropertyName("scssVariables")]
    public string scssVariables { get; set; } = string.Empty;
}