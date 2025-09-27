# Best Practices for .NET Bootstrap Web Applications

This document outlines best practices for building performant, secure, and maintainable .NET web applications using Bootstrap, based on the WebSpark.Bootswatch implementation.

## ?? Table of Contents

1. [Security](#security)
2. [Performance](#performance)
3. [Accessibility](#accessibility)
4. [SEO & Metadata](#seo--metadata)
5. [Code Organization](#code-organization)
6. [Testing](#testing)
7. [Deployment](#deployment)
8. [Monitoring](#monitoring)

## ?? Security

### Headers and CSP
```csharp
// Add security headers middleware
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Add("X-Frame-Options", "DENY");
    context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
    context.Response.Headers.Add("Referrer-Policy", "strict-origin-when-cross-origin");
    
    // Content Security Policy
    var csp = "default-src 'self'; " +
              "style-src 'self' 'unsafe-inline' https://cdn.jsdelivr.net; " +
              "script-src 'self' 'unsafe-inline'; " +
              "img-src 'self' data: https:; " +
              "font-src 'self' https://cdn.jsdelivr.net;";
    context.Response.Headers.Add("Content-Security-Policy", csp);
    
    await next();
});
```

### HSTS Configuration
```csharp
builder.Services.AddHsts(options =>
{
    options.MaxAge = TimeSpan.FromDays(365);
    options.IncludeSubdomains = true;
    options.Preload = true;
});
```

### Input Validation
- Always validate user input
- Use model binding with validation attributes
- Implement CSRF protection for forms
- Sanitize HTML content when displaying user input

## ? Performance

### Response Compression
```csharp
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
```

### Caching Strategies
```csharp
// Response caching
builder.Services.AddResponseCaching();

// Static file caching
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        var maxAge = app.Environment.IsDevelopment() ? 3600 : 31536000;
        ctx.Context.Response.Headers.Add("Cache-Control", $"public, max-age={maxAge}");
    }
});
```

### Resource Loading Optimization
```html
<!-- Preload critical resources -->
<link rel="preload" href="@themeUrl" as="style" onload="this.onload=null;this.rel='stylesheet'">
<noscript><link rel="stylesheet" href="@themeUrl"></noscript>

<!-- DNS prefetch for external resources -->
<link rel="dns-prefetch" href="//cdn.jsdelivr.net">

<!-- Defer non-critical scripts -->
<script src="~/lib/bootstrap/dist/js/bootstrap.bundle.min.js" defer></script>
```

### Image Optimization
- Use WebP format when possible
- Implement responsive images with `srcset`
- Lazy load images below the fold
- Optimize image sizes for different screen densities

## ? Accessibility

### Semantic HTML
```html
<!-- Use semantic HTML elements -->
<header role="banner">
  <nav role="navigation" aria-label="Main navigation">
    <!-- navigation content -->
  </nav>
</header>

<main role="main" id="main-content">
  <!-- main content -->
</main>

<footer role="contentinfo">
  <!-- footer content -->
</footer>
```

### Skip Navigation
```html
<a href="#main-content" class="visually-hidden-focusable">Skip to main content</a>
```

### ARIA Labels and Current State
```html
<a class="nav-link" asp-page="/Index" 
   aria-current="@(ViewContext.RouteData.Values["page"]?.ToString() == "/Index" ? "page" : null)">
   Home
</a>

<button aria-expanded="false" aria-haspopup="true" aria-controls="dropdown-menu">
  Menu
</button>
```

### Color and Contrast
- Ensure sufficient color contrast (4.5:1 for normal text, 3:1 for large text)
- Don't rely solely on color to convey information
- Test with color blindness simulators

## ?? SEO & Metadata

### Essential Meta Tags
```html
<meta name="description" content="Descriptive page content summary" />
<meta name="author" content="Author Name" />
<meta name="theme-color" content="#007bff" />

<!-- Open Graph / Facebook -->
<meta property="og:type" content="website" />
<meta property="og:title" content="Page Title" />
<meta property="og:description" content="Page description" />
<meta property="og:image" content="image-url" />

<!-- Twitter -->
<meta name="twitter:card" content="summary" />
<meta name="twitter:title" content="Page Title" />
<meta name="twitter:description" content="Page description" />
```

### Structured Data
- Implement JSON-LD structured data
- Use appropriate schema.org types
- Test with Google's Structured Data Testing Tool

## ??? Code Organization

### Service Registration Pattern
```csharp
// Extension methods for clean service registration
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBootswatchThemeSwitcher(this IServiceCollection services)
    {
        services.AddBootswatchStylesWithCache();
        // ... other registrations
        return services;
    }
}
```

### Configuration Management
```csharp
// Use strongly-typed configuration
public class BootswatchOptions
{
    public string DefaultTheme { get; set; } = "bootstrap";
    public bool EnableCaching { get; set; } = true;
    public TimeSpan CacheExpiration { get; set; } = TimeSpan.FromHours(24);
}

// Register configuration
builder.Services.Configure<BootswatchOptions>(
    builder.Configuration.GetSection("Bootswatch"));
```

### Dependency Injection Best Practices
- Use constructor injection
- Register services with appropriate lifetimes
- Use interfaces for abstraction
- Avoid service locator pattern

## ?? Testing

### Unit Testing
```csharp
[Test]
public void StyleCache_GetStyle_ReturnsCorrectStyle()
{
    // Arrange
    var mockProvider = new Mock<IStyleProvider>();
    var cache = new StyleCache(mockProvider.Object);
    
    // Act
    var result = cache.GetStyle("bootstrap");
    
    // Assert
    Assert.IsNotNull(result);
    Assert.AreEqual("bootstrap", result.Name);
}
```

### Integration Testing
```csharp
[Test]
public async Task GET_ThemeSwitcher_ReturnsSuccessStatusCode()
{
    // Arrange
    var client = _factory.CreateClient();
    
    // Act
    var response = await client.GetAsync("/theme-switcher");
    
    // Assert
    response.EnsureSuccessStatusCode();
}
```

### End-to-End Testing
- Use tools like Playwright or Selenium
- Test theme switching functionality
- Verify accessibility compliance
- Test responsive design on different screen sizes

## ?? Deployment

### Environment Configuration
```json
{
  "Production": {
    "Logging": {
      "LogLevel": {
        "Default": "Warning"
      }
    },
    "Bootswatch": {
      "EnableCaching": true,
      "CacheExpiration": "24:00:00"
    }
  }
}
```

### Health Checks
```csharp
builder.Services.AddHealthChecks()
    .AddCheck<StyleCacheHealthCheck>("style-cache")
    .AddCheck("self", () => HealthCheckResult.Healthy());

app.MapHealthChecks("/health");
```

### Docker Considerations
```dockerfile
# Use multi-stage builds
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["WebSpark.Bootswatch.Demo/WebSpark.Bootswatch.Demo.csproj", "WebSpark.Bootswatch.Demo/"]
RUN dotnet restore "WebSpark.Bootswatch.Demo/WebSpark.Bootswatch.Demo.csproj"
```

## ?? Monitoring

### Logging Best Practices
```csharp
public class ThemeController : Controller
{
    private readonly ILogger<ThemeController> _logger;
    
    public ThemeController(ILogger<ThemeController> logger)
    {
        _logger = logger;
    }
    
    public IActionResult SwitchTheme(string theme)
    {
        _logger.LogInformation("Theme switched to {Theme} by user {UserId}", 
            theme, User.Identity?.Name);
        // ... implementation
    }
}
```

### Performance Monitoring
- Use Application Insights or similar APM tools
- Monitor response times and error rates
- Track user interactions with theme switching
- Monitor resource usage and caching effectiveness

### Error Tracking
```csharp
// Global exception handling
app.UseExceptionHandler("/Error");

// Custom error logging
public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;
    
    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred");
            // Handle exception
        }
    }
}
```

## ?? Additional Recommendations

### Bootstrap-Specific Best Practices
1. **Use Bootstrap's utility classes** for spacing, colors, and typography
2. **Leverage responsive breakpoints** for different screen sizes
3. **Customize Bootstrap variables** rather than overriding CSS
4. **Use Bootstrap components consistently** across your application
5. **Test dark mode compatibility** when implementing theme switching

### Performance Considerations
1. **Bundle and minify assets** for production
2. **Use CDN for Bootstrap assets** when appropriate
3. **Implement lazy loading** for non-critical components
4. **Optimize bundle sizes** by including only needed Bootstrap components

### Security Considerations
1. **Validate theme names** to prevent path traversal attacks
2. **Sanitize user preferences** stored in cookies
3. **Use HTTPS** for all theme-related requests
4. **Implement rate limiting** for theme switching endpoints

### Maintainability
1. **Document component usage** and customizations
2. **Use consistent naming conventions** for CSS classes and variables
3. **Implement automated testing** for UI components
4. **Keep Bootstrap version updated** for security patches

This guide provides a foundation for building robust .NET Bootstrap web applications. Adapt these practices to your specific requirements and always stay updated with the latest security and performance recommendations.