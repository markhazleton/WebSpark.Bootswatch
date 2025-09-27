using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Collections.Frozen;
using WebSpark.Bootswatch.Model;

namespace WebSpark.Bootswatch.Services;

/// <summary>
/// Singleton service that caches style models from the Bootswatch API
/// </summary>
public class StyleCache : IDisposable
{
    private volatile FrozenSet<StyleModel>? _stylesSet;
    private volatile FrozenDictionary<string, StyleModel>? _stylesDictionary;
    private readonly IServiceProvider _serviceProvider;
    private volatile bool _isInitialized = false;
    private readonly SemaphoreSlim _initializationSemaphore = new(1, 1);
    private Task? _initializationTask = null;
    private readonly ILogger<StyleCache>? _logger;
    private bool _disposed = false;

    /// <summary>
    /// Initializes a new instance of the <see cref="StyleCache"/> class
    /// </summary>
    /// <param name="serviceProvider">The service provider</param>
    /// <param name="logger">Optional logger</param>
    public StyleCache(IServiceProvider serviceProvider, ILogger<StyleCache>? logger = null)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    /// <summary>
    /// Gets all available styles
    /// </summary>
    /// <returns>List of all available style models</returns>
    public List<StyleModel> GetAllStyles()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

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
                    _logger?.LogWarning("Timed out waiting for styles to load. Returning cached styles if available.");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error waiting for style initialization");
            }
        }

        // Return frozen collection for better performance and thread safety
        var stylesSet = _stylesSet;
        return stylesSet?.ToList() ?? new List<StyleModel>();
    }

    /// <summary>
    /// Gets a specific style by name with O(1) lookup performance
    /// </summary>
    /// <param name="name">Name of the style to retrieve</param>
    /// <returns>The requested style model or default if not found</returns>
    public StyleModel GetStyle(string name)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        if (string.IsNullOrEmpty(name))
            return new StyleModel();

        // Use dictionary lookup for O(1) performance instead of LINQ
        var stylesDictionary = _stylesDictionary;
        if (stylesDictionary != null && stylesDictionary.TryGetValue(name, out var style))
        {
            return style;
        }

        // Fallback for non-initialized cache
        if (!_isInitialized && _initializationTask != null)
        {
            try
            {
                if (Task.WaitAny(new[] { _initializationTask }, TimeSpan.FromSeconds(1)) == 0)
                {
                    stylesDictionary = _stylesDictionary;
                    if (stylesDictionary != null && stylesDictionary.TryGetValue(name, out style))
                    {
                        return style;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error waiting for style initialization in GetStyle");
            }
        }

        return new StyleModel();
    }

    /// <summary>
    /// Starts initialization asynchronously without blocking
    /// </summary>
    public void StartInitialization()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        if (_initializationTask == null)
        {
            // Use double-checked locking pattern with semaphore for better performance
            if (_initializationSemaphore.Wait(0)) // Non-blocking check
            {
                try
                {
                    if (_initializationTask == null)
                    {
                        _initializationTask = InitializeInternalAsync();
                    }
                }
                finally
                {
                    _initializationSemaphore.Release();
                }
            }
        }
    }

    /// <summary>
    /// Internal initialization method
    /// </summary>
    private async Task InitializeInternalAsync()
    {
        try
        {
            await LoadStyles().ConfigureAwait(false);
            _isInitialized = true;
            _logger?.LogInformation("StyleCache successfully initialized with {Count} styles", _stylesSet?.Count ?? 0);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error during StyleCache initialization");
        }
    }

    /// <summary>
    /// Loads styles from the Bootswatch provider
    /// </summary>
    public async Task LoadStyles()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        // Create a scope to resolve the IStyleProvider since it's registered as scoped
        using var scope = _serviceProvider.CreateScope();
        var styleProvider = scope.ServiceProvider.GetRequiredService<IStyleProvider>();

        // Get styles from the provider
        var styles = await styleProvider.GetAsync().ConfigureAwait(false);
        var stylesList = styles.ToList();

        // Create frozen collections for better performance and memory efficiency
        _stylesSet = stylesList.ToFrozenSet();
        
        // Create dictionary with case-insensitive comparison for better usability
        var dictionary = new Dictionary<string, StyleModel>(StringComparer.OrdinalIgnoreCase);
        foreach (var style in stylesList)
        {
            if (!string.IsNullOrEmpty(style.name))
            {
                dictionary[style.name] = style;
            }
        }
        _stylesDictionary = dictionary.ToFrozenDictionary(StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Initializes the StyleCache by loading styles from the Bootswatch provider
    /// </summary>
    public static void InitializeInBackground(IServiceProvider serviceProvider)
    {
        var styleCache = serviceProvider.GetRequiredService<StyleCache>();
        styleCache.StartInitialization();
    }

    /// <summary>
    /// Dispose method to clean up resources
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Protected dispose method
    /// </summary>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            _initializationSemaphore?.Dispose();
            _disposed = true;
        }
    }
}