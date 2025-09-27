using WebSpark.Bootswatch;
using WebSpark.HttpClientUtility.RequestResult;
using Microsoft.AspNetCore.ResponseCompression;
using WebSpark.Bootswatch.Demo.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Add response compression
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[]
    {
        "text/css",
        "application/javascript",
        "text/javascript",
        "application/json",
        "text/json",
        "image/svg+xml"
    });
});

// Add response caching
builder.Services.AddResponseCaching();

// Use our custom implementation instead of the default one for HTTP requests
builder.Services.AddScoped<IHttpRequestResultService, HttpRequestResultService>();

// Add version service for footer information
builder.Services.AddSingleton<VersionService>();

// Use the extension method to register Bootswatch theme switcher (includes StyleCache)
builder.Services.AddBootswatchThemeSwitcher();
builder.Services.AddLogging();

// Add detailed logging for static files middleware
builder.Logging.AddConsole().SetMinimumLevel(LogLevel.Debug);

// Add HttpContextAccessor service
builder.Services.AddHttpContextAccessor();

// Add security headers
builder.Services.AddHsts(options =>
{
    options.MaxAge = TimeSpan.FromDays(365);
    options.IncludeSubDomains = true;
    options.Preload = true;
});

// Add health checks
builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Enable response compression
app.UseResponseCompression();

// Security headers middleware
app.Use(async (context, next) =>
{
    // Add security headers using the indexer to avoid duplicate key issues
    context.Response.Headers["X-Content-Type-Options"] = "nosniff";
    context.Response.Headers["X-Frame-Options"] = "DENY";
    context.Response.Headers["X-XSS-Protection"] = "1; mode=block";
    context.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
    
    // Content Security Policy - updated to allow CSS map files and external connections
    var csp = "default-src 'self'; " +
              "style-src 'self' 'unsafe-inline' https://cdn.jsdelivr.net; " +
              "script-src 'self' 'unsafe-inline'; " +
              "img-src 'self' data: https:; " +
              "font-src 'self' https://cdn.jsdelivr.net; " +
              "connect-src 'self' https://cdn.jsdelivr.net; " +
              "object-src 'none'; " +
              "base-uri 'self';";
    context.Response.Headers["Content-Security-Policy"] = csp;
    
    await next();
});

app.UseHttpsRedirection();

// Enable response caching
app.UseResponseCaching();

// Use all Bootswatch features including theme switcher
app.UseBootswatchAll();

// Add custom middleware to log and debug static file requests
app.Use(async (context, next) =>
{
    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
    var path = context.Request.Path.Value;

    // Log all requests that look like they're trying to access theme CSS files
    if (path?.Contains("bootstrap.min.css") == true)
    {
        logger.LogInformation("Requested theme CSS: {Path}", path);
    }

    await next();

    // Log if the response is a 404 for theme CSS files
    if (context.Response.StatusCode == 404 && path?.Contains("bootstrap.min.css") == true)
    {
        logger.LogWarning("404 Not Found for theme CSS: {Path}", path);
    }
});

// Configure static files with caching
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        var maxAge = app.Environment.IsDevelopment() ? 3600 : 31536000; // 1 hour in dev, 1 year in prod
        ctx.Context.Response.Headers["Cache-Control"] = $"public, max-age={maxAge}";
    }
});

app.UseRouting();

app.UseAuthorization();

// Add health check endpoint
app.MapHealthChecks("/health");

app.MapRazorPages();

app.Run();
