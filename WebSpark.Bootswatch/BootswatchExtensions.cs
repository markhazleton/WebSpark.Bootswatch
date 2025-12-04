using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using WebSpark.Bootswatch.Provider;
using WebSpark.Bootswatch.Services;
using WebSpark.HttpClientUtility.RequestResult;

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
    /// <exception cref="InvalidOperationException">
    /// Thrown when required WebSpark.HttpClientUtility services are not registered.
    /// </exception>
    /// <example>
    /// <code>
    /// // In Program.cs, register services in correct order:
    /// using WebSpark.Bootswatch;
    /// using WebSpark.HttpClientUtility;
    /// 
    /// builder.Services.AddHttpClientUtility();      // Required first
    /// builder.Services.AddBootswatchStyles();       // Then this
    /// </code>
    /// </example>
    public static IServiceCollection AddBootswatchStyles(this IServiceCollection services)
    {
        // Validate that HttpClientUtility is registered
        ValidateHttpClientUtilityRegistration(services);

        services.AddHttpClient();
        services.AddScoped<Model.IStyleProvider, BootswatchStyleProvider>();

        return services;
    }

    /// <summary>
    /// Adds the Bootswatch style provider, style cache service, and required services to the IServiceCollection
    /// </summary>
    /// <param name="services">The service collection to add services to</param>
    /// <returns>The service collection for chaining</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when required WebSpark.HttpClientUtility services are not registered.
    /// </exception>
    /// <example>
    /// <code>
    /// // In Program.cs, register services in correct order:
    /// using WebSpark.Bootswatch;
    /// using WebSpark.HttpClientUtility;
    /// 
    /// builder.Services.AddHttpClientUtility();           // Required first
    /// builder.Services.AddBootswatchStylesWithCache();   // Then this
    /// </code>
    /// </example>
    public static IServiceCollection AddBootswatchStylesWithCache(this IServiceCollection services)
    {
        services.AddBootswatchStyles();
        services.AddSingleton<StyleCache>();

        return services;
    }

    /// <summary>
    /// Adds the Bootswatch theme switcher components and services to the IServiceCollection.
    /// This is the recommended method for most applications as it includes all necessary components.
    /// </summary>
    /// <param name="services">The service collection to add services to</param>
    /// <returns>The service collection for chaining</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when required WebSpark.HttpClientUtility services are not registered.
    /// Call builder.Services.AddHttpClientUtility() before this method.
    /// </exception>
    /// <example>
    /// <code>
    /// // REQUIRED: Register services in correct order
    /// using WebSpark.Bootswatch;
    /// using WebSpark.HttpClientUtility;
    /// 
    /// var builder = WebApplication.CreateBuilder(args);
    /// 
    /// // Step 1: Register HttpClientUtility FIRST
    /// builder.Services.AddHttpClientUtility();
    /// 
    /// // Step 2: Register Bootswatch theme switcher
    /// builder.Services.AddBootswatchThemeSwitcher();
    /// 
    /// // Step 3: Configure middleware
    /// var app = builder.Build();
    /// app.UseBootswatchAll();    // Must be before UseStaticFiles()
    /// app.UseStaticFiles();
    /// </code>
    /// </example>
    /// <remarks>
    /// This method requires:
    /// 1. WebSpark.HttpClientUtility package installed
    /// 2. AddHttpClientUtility() called before this method
    /// 3. Configuration in appsettings.json (HttpRequestResultPollyOptions section)
    /// 
    /// For troubleshooting, see: https://github.com/MarkHazleton/WebSpark.Bootswatch#-common-errors--solutions
    /// </remarks>
    public static IServiceCollection AddBootswatchThemeSwitcher(this IServiceCollection services)
    {
        services.AddBootswatchStylesWithCache();

        // Add configuration validation hosted service
        services.AddHostedService<BootswatchStartupValidation>();

        // Add MVC parts if necessary, but don't add RazorPages since that's application-specific
        services.AddMvcCore().ConfigureApplicationPartManager(apm =>
            apm.ApplicationParts.Add(new AssemblyPart(typeof(BootswatchExtensions).Assembly)));

        return services;
    }

    /// <summary>
    /// Validates that WebSpark.HttpClientUtility services are registered.
    /// </summary>
    /// <param name="services">The service collection to validate</param>
    /// <exception cref="InvalidOperationException">
    /// Thrown when IHttpRequestResultService is not registered.
    /// </exception>
    private static void ValidateHttpClientUtilityRegistration(IServiceCollection services)
    {
        var httpRequestResultService = services.FirstOrDefault(
            d => d.ServiceType == typeof(IHttpRequestResultService));

        if (httpRequestResultService == null)
        {
            throw new InvalidOperationException(
                "❌ WebSpark.HttpClientUtility services are not registered.\n\n" +
                "REQUIRED SETUP:\n" +
                "1. Install package: dotnet add package WebSpark.HttpClientUtility\n" +
                "2. Add using statement: using WebSpark.HttpClientUtility;\n" +
                "3. Register services BEFORE Bootswatch:\n\n" +
                "   using WebSpark.Bootswatch;\n" +
                "   using WebSpark.HttpClientUtility;\n\n" +
                "   builder.Services.AddHttpClientUtility();      // ✅ FIRST\n" +
                "   builder.Services.AddBootswatchThemeSwitcher(); // ✅ THEN THIS\n\n" +
                "4. Add configuration to appsettings.json:\n" +
                "   {\n" +
                "     \"HttpRequestResultPollyOptions\": {\n" +
                "       \"MaxRetryAttempts\": 3,\n" +
                "       \"RetryDelaySeconds\": 1\n" +
                "     }\n" +
                "   }\n\n" +
                "For complete setup guide, see: https://github.com/MarkHazleton/WebSpark.Bootswatch/wiki/Getting-Started");
        }
    }

    /// <summary>
    /// Initializes the Bootswatch style cache in the background to avoid blocking application startup.
    /// The cache will be populated asynchronously with available themes from the Bootswatch API.
    /// </summary>
    /// <param name="app">The web application to configure</param>
    /// <returns>The web application for chaining</returns>
    /// <remarks>
    /// This method starts a background task to populate the theme cache without blocking
    /// application startup. If the API is unavailable, default themes will be used.
    /// </remarks>
    /// <example>
    /// <code>
    /// var app = builder.Build();
    /// 
    /// // Initialize cache in background
    /// app.UseBootswatchStyleCache();
    /// </code>
    /// </example>
    public static IApplicationBuilder UseBootswatchStyleCache(this IApplicationBuilder app)
    {
        StyleCache.InitializeInBackground(app.ApplicationServices);
        return app;
    }

    /// <summary>
    /// Adds the embedded static files from WebSpark.Bootswatch to the web application.
    /// This middleware serves CSS, JavaScript, and other assets embedded in the library.
    /// </summary>
    /// <param name="app">The web application to configure</param>
    /// <returns>The web application for chaining</returns>
    /// <remarks>
    /// <para>
    /// ⚠️ IMPORTANT: This middleware MUST be called BEFORE UseStaticFiles() in the pipeline.
    /// </para>
    /// <para>
    /// The middleware serves files from the /_content/WebSpark.Bootswatch path and includes
    /// appropriate caching headers for optimal performance.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// var app = builder.Build();
    /// 
    /// // ✅ CORRECT ORDER
    /// app.UseBootswatchStaticFiles();    // First - Bootswatch resources
    /// app.UseStaticFiles();              // Then - Application resources
    /// 
    /// // ❌ WRONG ORDER (will not work)
    /// app.UseStaticFiles();
    /// app.UseBootswatchStaticFiles();
    /// </code>
    /// </example>
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
    /// Configures the application to use all Bootswatch features including the theme switcher.
    /// This is the recommended method as it configures both the style cache and static files in the correct order.
    /// </summary>
    /// <param name="app">The web application to configure</param>
    /// <returns>The web application for chaining</returns>
    /// <remarks>
    /// <para>
    /// ⚠️ IMPORTANT: This middleware MUST be called BEFORE UseStaticFiles() in the pipeline.
    /// </para>
    /// <para>
    /// This method combines UseBootswatchStyleCache() and UseBootswatchStaticFiles() for convenience.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// var app = builder.Build();
    /// 
    /// // ✅ CORRECT MIDDLEWARE ORDER
    /// app.UseHttpsRedirection();
    /// app.UseBootswatchAll();        // Must be before UseStaticFiles()
    /// app.UseStaticFiles();
    /// app.UseRouting();
    /// app.UseAuthorization();
    /// app.MapRazorPages();
    /// </code>
    /// </example>
    public static IApplicationBuilder UseBootswatchAll(this IApplicationBuilder app)
    {
        app.UseBootswatchStyleCache();
        app.UseBootswatchStaticFiles();
        return app;
    }
}