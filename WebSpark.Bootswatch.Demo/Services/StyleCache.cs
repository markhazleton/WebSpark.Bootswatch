using Microsoft.Extensions.Logging;
using WebSpark.Bootswatch.Model;
using WebSpark.Bootswatch.Provider;
using Microsoft.Extensions.DependencyInjection;

namespace WebSpark.Bootswatch.Demo.Services;

/// <summary>
/// Singleton service that caches style models from the Bootswatch API
/// </summary>
public class StyleCache
{
    private readonly List<StyleModel> _styles = new();
    private readonly IServiceProvider _serviceProvider;
    private bool _isInitialized = false;
    private readonly object _lockObject = new();
    private Task? _initializationTask = null;
    private readonly ILogger<StyleCache>? _logger;

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
                    _logger?.LogWarning("Timed out waiting for styles to load. Returning default styles.");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error waiting for style initialization");
            }
        }

        // Even if initialization hasn't completed, return what we have
        lock (_lockObject)
        {
            return _styles.ToList(); // Return a copy to avoid potential thread issues
        }
    }

    /// <summary>
    /// Gets a specific style by name
    /// </summary>
    /// <param name="name">Name of the style to retrieve</param>
    /// <returns>The requested style model or default if not found</returns>
    public StyleModel GetStyle(string name)
    {
        var styles = GetAllStyles();
        return styles.FirstOrDefault(s => s.name == name) ?? new StyleModel();
    }

    /// <summary>
    /// Starts initialization asynchronously without blocking
    /// </summary>
    public void StartInitialization()
    {
        if (_initializationTask == null)
        {
            lock (_lockObject)
            {
                if (_initializationTask == null)
                {
                    _initializationTask = Task.Run(async () =>
                    {
                        try
                        {
                            await LoadStyles();
                            _isInitialized = true;
                            _logger?.LogInformation("StyleCache successfully initialized with {Count} styles", _styles.Count);
                        }
                        catch (Exception ex)
                        {
                            _logger?.LogError(ex, "Error during StyleCache initialization");
                        }
                    });
                }
            }
        }
    }

    /// <summary>
    /// Loads styles from the Bootswatch provider
    /// </summary>
    public async Task LoadStyles()
    {
        // Create a scope to resolve the IStyleProvider since it's registered as scoped
        using var scope = _serviceProvider.CreateScope();
        var styleProvider = scope.ServiceProvider.GetRequiredService<IStyleProvider>();

        // Get styles from the provider
        var styles = await styleProvider.GetAsync();

        // Clear and populate the styles collection
        lock (_lockObject)
        {
            _styles.Clear();
            _styles.AddRange(styles);
        }
    }

    /// <summary>
    /// Initializes the StyleCache by loading styles from the Bootswatch provider
    /// </summary>
    public static void InitializeInBackground(IServiceProvider serviceProvider)
    {
        var styleCache = serviceProvider.GetRequiredService<StyleCache>();
        styleCache.StartInitialization();
    }
}