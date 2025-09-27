using System.Reflection;

namespace WebSpark.Bootswatch.Demo.Services;

/// <summary>
/// Service to provide version and build information for the application
/// </summary>
public class VersionService
{
    private readonly string _version;
    private readonly DateTime _buildDate;
    private readonly string _nugetPackageUrl;

    public VersionService()
    {
        // Get version from WebSpark.Bootswatch library
        _version = GetWebSparkBootswatchVersion();
        
        // Get build date from assembly (fallback to file creation time)
        _buildDate = GetBuildDate();
        
        // NuGet package URL
        _nugetPackageUrl = "https://www.nuget.org/packages/WebSpark.Bootswatch/";
    }

    /// <summary>
    /// Gets the current version of the WebSpark.Bootswatch library
    /// </summary>
    public string Version => _version;

    /// <summary>
    /// Gets the build date of the application
    /// </summary>
    public DateTime BuildDate => _buildDate;

    /// <summary>
    /// Gets the formatted build date string
    /// </summary>
    public string FormattedBuildDate => _buildDate.ToString("yyyy-MM-dd HH:mm UTC");

    /// <summary>
    /// Gets the NuGet package URL
    /// </summary>
    public string NuGetPackageUrl => _nugetPackageUrl;

    /// <summary>
    /// Gets the short version (without build metadata)
    /// </summary>
    public string ShortVersion
    {
        get
        {
            var version = _version;
            var plusIndex = version.IndexOf('+');
            return plusIndex > 0 ? version[..plusIndex] : version;
        }
    }

    private static string GetWebSparkBootswatchVersion()
    {
        try
        {
            // Try to get version from WebSpark.Bootswatch assembly
            var bootswatchAssembly = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(a => a.GetName().Name == "WebSpark.Bootswatch");

            if (bootswatchAssembly != null)
            {
                var informationalVersion = bootswatchAssembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
                if (!string.IsNullOrEmpty(informationalVersion))
                {
                    return informationalVersion;
                }

                var assemblyVersion = bootswatchAssembly.GetName().Version?.ToString();
                if (!string.IsNullOrEmpty(assemblyVersion))
                {
                    return assemblyVersion;
                }
            }

            // Fallback to current assembly
            var currentAssembly = Assembly.GetExecutingAssembly();
            return currentAssembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion 
                  ?? currentAssembly.GetName().Version?.ToString() 
                  ?? "1.20.0";
        }
        catch
        {
            return "1.20.0";
        }
    }

    private static DateTime GetBuildDate()
    {
        try
        {
            // Try to get build date from current assembly first
            var currentAssembly = Assembly.GetExecutingAssembly();
            
            // Check for build date in assembly metadata
            var buildDateAttribute = currentAssembly.GetCustomAttributes<AssemblyMetadataAttribute>()
                .FirstOrDefault(attr => attr.Key == "BuildDate");
            
            if (buildDateAttribute?.Value != null && 
                DateTime.TryParse(buildDateAttribute.Value, out var buildDate))
            {
                return buildDate.ToUniversalTime();
            }

            // Try to get from WebSpark.Bootswatch assembly
            var bootswatchAssembly = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(a => a.GetName().Name == "WebSpark.Bootswatch");

            if (bootswatchAssembly != null)
            {
                var bootswatchBuildDate = bootswatchAssembly.GetCustomAttributes<AssemblyMetadataAttribute>()
                    .FirstOrDefault(attr => attr.Key == "BuildDate");
                
                if (bootswatchBuildDate?.Value != null && 
                    DateTime.TryParse(bootswatchBuildDate.Value, out var bsBuildDate))
                {
                    return bsBuildDate.ToUniversalTime();
                }

                // Fallback to assembly file time for WebSpark.Bootswatch
                var assemblyLocation = bootswatchAssembly.Location;
                if (!string.IsNullOrEmpty(assemblyLocation) && File.Exists(assemblyLocation))
                {
                    return File.GetCreationTimeUtc(assemblyLocation);
                }
            }

            // Fallback to current assembly file creation time
            var currentAssemblyLocation = currentAssembly.Location;
            if (!string.IsNullOrEmpty(currentAssemblyLocation) && File.Exists(currentAssemblyLocation))
            {
                return File.GetCreationTimeUtc(currentAssemblyLocation);
            }

            // Final fallback to current time
            return DateTime.UtcNow;
        }
        catch
        {
            // If anything fails, return current time
            return DateTime.UtcNow;
        }
    }
}