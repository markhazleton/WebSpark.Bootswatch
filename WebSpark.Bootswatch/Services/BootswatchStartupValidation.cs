using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WebSpark.Bootswatch.Services;

/// <summary>
/// Validates required configuration at application startup and logs warnings if configuration is missing or incomplete.
/// This helps developers identify configuration issues early in the development process.
/// </summary>
public class BootswatchStartupValidation : IHostedService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<BootswatchStartupValidation> _logger;

    /// <summary>
    /// Initializes a new instance of the BootswatchStartupValidation class.
    /// </summary>
    /// <param name="configuration">The application configuration</param>
    /// <param name="logger">The logger for diagnostic messages</param>
    public BootswatchStartupValidation(
        IConfiguration configuration,
        ILogger<BootswatchStartupValidation> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    /// <summary>
    /// Executes configuration validation when the application starts.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A task representing the asynchronous operation</returns>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        ValidateConfiguration();
        return Task.CompletedTask;
    }

    /// <summary>
    /// Performs cleanup when the application is stopping (no-op for this service).
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A completed task</returns>
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    /// <summary>
    /// Validates the presence and correctness of required configuration sections.
    /// </summary>
    private void ValidateConfiguration()
    {
        var hasWarnings = false;

        // Validate CsvOutputFolder
        var csvFolder = _configuration["CsvOutputFolder"];
        if (string.IsNullOrEmpty(csvFolder))
        {
            _logger.LogWarning(
                "CsvOutputFolder is not configured in appsettings.json. " +
                "This may cause issues with theme caching. " +
                "Recommended: Add \"CsvOutputFolder\": \"c:\\\\temp\\\\WebSpark\\\\CsvOutput\" to your configuration.");
            hasWarnings = true;
        }

        // Validate HttpRequestResultPollyOptions
        var pollySection = _configuration.GetSection("HttpRequestResultPollyOptions");
        if (!pollySection.Exists())
        {
            _logger.LogWarning(
                "HttpRequestResultPollyOptions is not configured in appsettings.json. " +
                "Using default retry policies. For production deployments, add this section with appropriate values:\n" +
                "  \"HttpRequestResultPollyOptions\": {{\n" +
                "    \"MaxRetryAttempts\": 3,\n" +
                "    \"RetryDelaySeconds\": 1,\n" +
                "    \"CircuitBreakerThreshold\": 3,\n" +
                "    \"CircuitBreakerDurationSeconds\": 10\n" +
                "  }}");
            hasWarnings = true;
        }
        else
        {
            // Validate individual Polly options
            if (string.IsNullOrEmpty(pollySection["MaxRetryAttempts"]))
            {
                _logger.LogWarning("HttpRequestResultPollyOptions.MaxRetryAttempts is not configured. Using default value.");
                hasWarnings = true;
            }
            if (string.IsNullOrEmpty(pollySection["RetryDelaySeconds"]))
            {
                _logger.LogWarning("HttpRequestResultPollyOptions.RetryDelaySeconds is not configured. Using default value.");
                hasWarnings = true;
            }
        }

        // Validate BootswatchOptions (optional, but log info if not present)
        var bootswatchSection = _configuration.GetSection("BootswatchOptions");
        if (!bootswatchSection.Exists())
        {
            _logger.LogInformation(
                "BootswatchOptions is not configured in appsettings.json. " +
                "Using default theme (bootstrap). To customize theme behavior, add this section:\n" +
                "  \"BootswatchOptions\": {{\n" +
                "    \"DefaultTheme\": \"yeti\",\n" +
                "    \"EnableCaching\": true,\n" +
                "    \"CacheDurationMinutes\": 60\n" +
                "  }}");
        }

        // Log success message if no warnings
        if (!hasWarnings)
        {
            _logger.LogInformation("✅ WebSpark.Bootswatch configuration validation completed successfully.");
        }
        else
        {
            _logger.LogWarning(
                "⚠️ WebSpark.Bootswatch configuration validation completed with warnings. " +
                "The application will function with default values, but you should review the warnings above. " +
                "For complete setup guide, see: https://github.com/MarkHazleton/WebSpark.Bootswatch/wiki/Getting-Started");
        }
    }
}
