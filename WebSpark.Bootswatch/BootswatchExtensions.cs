using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using WebSpark.Bootswatch.Provider;
using WebSpark.Bootswatch.Services;

namespace WebSpark.Bootswatch;

/// <summary>
/// Extension methods for WebSpark.Bootswatch integration
/// </summary>
public static class BootswatchExtensions
{
    /// <summary>
    /// Adds the Bootswatch style provider and services to the IServiceCollection
    /// </summary>
    /// <param name="services">The service collection to add services to</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddBootswatchStyles(this IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddScoped<Model.IStyleProvider, BootswatchStyleProvider>();

        return services;
    }

    /// <summary>
    /// Adds the Bootswatch style provider, style cache service, and required services to the IServiceCollection
    /// </summary>
    /// <param name="services">The service collection to add services to</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddBootswatchStylesWithCache(this IServiceCollection services)
    {
        services.AddBootswatchStyles();
        services.AddSingleton<StyleCache>();

        return services;
    }

    /// <summary>
    /// Adds the Bootswatch theme switcher components and services to the IServiceCollection
    /// </summary>
    /// <param name="services">The service collection to add services to</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddBootswatchThemeSwitcher(this IServiceCollection services)
    {
        services.AddBootswatchStylesWithCache();

        // Add MVC parts if necessary, but don't add RazorPages since that's application-specific
        services.AddMvcCore().ConfigureApplicationPartManager(apm =>
            apm.ApplicationParts.Add(new AssemblyPart(typeof(BootswatchExtensions).Assembly)));

        return services;
    }

    /// <summary>
    /// Initializes the Bootswatch style cache in the background to avoid blocking application startup
    /// </summary>
    /// <param name="app">The web application to configure</param>
    /// <returns>The web application for chaining</returns>
    public static IApplicationBuilder UseBootswatchStyleCache(this IApplicationBuilder app)
    {
        StyleCache.InitializeInBackground(app.ApplicationServices);
        return app;
    }

    /// <summary>
    /// Adds the embedded static files from WebSpark.Bootswatch to the web application
    /// </summary>
    /// <param name="app">The web application to configure</param>
    /// <returns>The web application for chaining</returns>
    public static IApplicationBuilder UseBootswatchStaticFiles(this IApplicationBuilder app)
    {
        var assembly = typeof(BootswatchExtensions).Assembly;

        // Create embedded file provider with explicit namespace
        var embeddedFileProvider = new EmbeddedFileProvider(
            assembly,
            "WebSpark.Bootswatch"
        );

        // Log available embedded resources to help with debugging
        var logger = app.ApplicationServices.GetService(typeof(ILogger<>).MakeGenericType(typeof(BootswatchExtensions))) as ILogger;
        if (logger != null)
        {
            // Get embedded resource names
            var resourceNames = assembly.GetManifestResourceNames();
            logger.LogDebug("Available embedded resources: {ResourceCount}", resourceNames.Length);
            foreach (var name in resourceNames)
            {
                logger.LogDebug("Resource: {ResourceName}", name);
            }
        }

        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = embeddedFileProvider,
            RequestPath = "/_content/WebSpark.Bootswatch",
            OnPrepareResponse = ctx =>
            {
                // Add debug headers to help identify the file provider serving the resource
                ctx.Context.Response.Headers["X-Served-By"] = "WebSpark.Bootswatch.EmbeddedFileProvider";
                
                // Add caching headers for CSS and JS files
                var file = ctx.File;
                var extension = Path.GetExtension(file.Name).ToLowerInvariant();
                
                if (extension == ".css" || extension == ".js")
                {
                    // Cache for 1 year, but allow revalidation
                    ctx.Context.Response.Headers["Cache-Control"] = "public, max-age=31536000, must-revalidate";
                    ctx.Context.Response.Headers["Expires"] = DateTime.UtcNow.AddYears(1).ToString("R");
                }
                else
                {
                    // Shorter cache for other assets
                    ctx.Context.Response.Headers["Cache-Control"] = "public, max-age=86400";
                }
            }
        });

        return app;
    }

    /// <summary>
    /// Configures the application to use all Bootswatch features including the theme switcher
    /// </summary>
    /// <param name="app">The web application to configure</param>
    /// <returns>The web application for chaining</returns>
    public static IApplicationBuilder UseBootswatchAll(this IApplicationBuilder app)
    {
        app.UseBootswatchStyleCache();
        app.UseBootswatchStaticFiles();
        return app;
    }
}