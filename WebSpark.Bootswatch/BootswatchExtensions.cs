using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using System.Reflection;
using WebSpark.Bootswatch.Model;
using WebSpark.Bootswatch.Provider;

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
                ctx.Context.Response.Headers.Add("X-Served-By", "WebSpark.Bootswatch.EmbeddedFileProvider");
            }
        });

        return app;
    }
}