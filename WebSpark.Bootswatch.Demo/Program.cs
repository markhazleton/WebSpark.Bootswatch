using WebSpark.Bootswatch;
using WebSpark.Bootswatch.Provider;
using WebSpark.Bootswatch.Model;
using WebSpark.HttpClientUtility.RequestResult;
using WebSpark.Bootswatch.Demo.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Use the extension method to register Bootswatch services
builder.Services.AddBootswatchStyles();

// Use our custom implementation instead of the default one for HTTP requests
builder.Services.AddScoped<IHttpRequestResultService, HttpRequestResultService>();

// Register StyleCache as a singleton with logger
builder.Services.AddSingleton<StyleCache>();
builder.Services.AddLogging();

// Add detailed logging for static files middleware
builder.Logging.AddConsole().SetMinimumLevel(LogLevel.Debug);

var app = builder.Build();

// Initialize StyleCache in the background without blocking application startup
StyleCache.InitializeInBackground(app.Services);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

// First register the Bootswatch static files middleware to serve embedded static files
app.UseBootswatchStaticFiles();

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

// Then register the standard static files middleware
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
