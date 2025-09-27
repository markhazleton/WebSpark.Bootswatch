using Microsoft.Extensions.Logging;
using System.Net;
using WebSpark.Bootswatch.Model;
using WebSpark.HttpClientUtility.RequestResult;

namespace WebSpark.Bootswatch.Provider;

/// <summary>
/// Bootswatch Style Provider
/// </summary>
public class BootswatchStyleProvider(
    ILogger<BootswatchStyleProvider> logger,
    IHttpRequestResultService resultService) : IStyleProvider
{
    private const string BootswatchApiUrl = "https://bootswatch.com/api/5.json";
    private const string StaticAssetBasePath = "/_content/WebSpark.Bootswatch";

    // Cache default styles to avoid recreating them on every call
    private static readonly List<StyleModel> DefaultStyles = new()
    {
        new()
        {
            name = "mom",
            css = $"{StaticAssetBasePath}/style/mom/css/bootstrap.min.css",
            cssMin = $"{StaticAssetBasePath}/style/mom/css/bootstrap.min.css",
            cssCdn = $"{StaticAssetBasePath}/style/mom/css/bootstrap.min.css",
            description = "Custom theme for MOM websites",
        },
        new()
        {
            name = "texecon",
            css = $"{StaticAssetBasePath}/style/texecon/css/bootstrap.min.css",
            cssMin = $"{StaticAssetBasePath}/style/texecon/css/bootstrap.min.css",
            cssCdn = $"{StaticAssetBasePath}/style/texecon/css/bootstrap.min.css",
            description = "Custom theme for TexEcon websites",
        }
    };

    /// <summary>
    /// Get all available styles
    /// </summary>
    /// <returns>Collection of styles including custom and Bootswatch themes</returns>
    public async Task<IEnumerable<StyleModel>> GetAsync()
    {
        return await GetSiteStylesAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// Get a specific style by name
    /// </summary>
    /// <param name="name">The name of the style to retrieve</param>
    /// <returns>The requested style or an empty style model if not found</returns>
    public async Task<StyleModel> GetAsync(string name)
    {
        if (string.IsNullOrEmpty(name))
            return new StyleModel();

        var styles = await GetSiteStylesAsync().ConfigureAwait(false);
        
        // Use more efficient lookup
        foreach (var style in styles)
        {
            if (string.Equals(style.name, name, StringComparison.OrdinalIgnoreCase))
            {
                return style;
            }
        }
        
        return new StyleModel();
    }

    /// <summary>
    /// Get all available site styles including custom embedded themes and Bootswatch themes
    /// </summary>
    /// <returns>List of all available styles</returns>
    public async Task<List<StyleModel>> GetSiteStylesAsync()
    {
        // Start with a copy of default styles to avoid modifying the cached version
        var siteStyle = new List<StyleModel>(DefaultStyles.Count + 20) // Pre-allocate capacity
        {
            // Add default styles
            DefaultStyles[0],
            DefaultStyles[1]
        };

        try
        {
            var themeRequestResult = new HttpRequestResult<BootswatchResponse>
            {
                CacheDurationMinutes = 20,
                RequestPath = BootswatchApiUrl
            };

            themeRequestResult = await resultService.HttpSendRequestResultAsync(themeRequestResult).ConfigureAwait(false);

            if (themeRequestResult.StatusCode != HttpStatusCode.OK || themeRequestResult.ResponseResults?.themes == null)
            {
                logger.LogWarning("Bootswatch API call failed with status code: {StatusCode}. Using default styles.", themeRequestResult.StatusCode);
                return siteStyle;
            }

            var themes = themeRequestResult.ResponseResults.themes;
            
            // Pre-allocate capacity based on known themes count
            if (siteStyle.Capacity < siteStyle.Count + themes.Count)
            {
                siteStyle.Capacity = siteStyle.Count + themes.Count;
            }

            // Use for loop for better performance than foreach
            for (int i = 0; i < themes.Count; i++)
            {
                siteStyle.Add(Create(themes[i]));
            }

            logger.LogInformation("Successfully loaded {Count} themes from Bootswatch API", themes.Count);
        }
        catch (HttpRequestException httpEx)
        {
            logger.LogWarning(httpEx, "HTTP error occurred while fetching themes from Bootswatch API. Using default styles.");
        }
        catch (TaskCanceledException tcEx) when (tcEx.InnerException is TimeoutException)
        {
            logger.LogWarning(tcEx, "Timeout occurred while fetching themes from Bootswatch API. Using default styles.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error occurred while fetching themes from Bootswatch API. Using default styles.");
        }

        return siteStyle;
    }

    /// <summary>
    /// Create a StyleModel from a BootswatchStyle
    /// </summary>
    /// <param name="theme">Bootswatch theme retrieved from API</param>
    /// <returns>StyleModel for the theme</returns>
    private static StyleModel Create(BootswatchStyle theme)
    {
        return new StyleModel()
        {
            scss = theme.scss,
            scssVariables = theme.scssVariables,
            css = theme.css,
            cssCdn = theme.cssCdn,
            cssMin = theme.cssMin,
            description = theme.description,
            less = theme.less,
            lessVariables = theme.lessVariables,
            name = theme.name,
            preview = theme.preview,
            thumbnail = theme.thumbnail
        };
    }
}

