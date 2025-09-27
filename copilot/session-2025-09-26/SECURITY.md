# Security Policy

## Supported Versions

We provide security updates for the following versions of WebSpark.Bootswatch:

| Version | Supported          |
| ------- | ------------------ |
| 1.20.x  | ? Yes             |
| 1.10.x  | ? Yes             |
| 1.9.x   | ?? Limited Support |
| < 1.9   | ? No              |

## Reporting a Vulnerability

We take security vulnerabilities seriously. If you discover a security vulnerability in WebSpark.Bootswatch, please report it to us privately.

### How to Report

**Please do NOT report security vulnerabilities through public GitHub issues.**

Instead, please send an email to [mark@markhazleton.com](mailto:mark@markhazleton.com) with the following information:

- **Subject**: "Security Vulnerability - WebSpark.Bootswatch"
- **Description**: Detailed description of the vulnerability
- **Steps to Reproduce**: Clear steps to reproduce the issue
- **Impact**: Potential impact and severity assessment
- **Affected Versions**: Which versions are affected
- **Suggested Fix**: If you have suggestions for fixing the issue

### What to Expect

- **Acknowledgment**: We will acknowledge receipt of your report within 48 hours
- **Initial Assessment**: We will provide an initial assessment within 5 business days
- **Regular Updates**: We will keep you informed of our progress
- **Resolution**: We aim to resolve critical security issues within 30 days
- **Credit**: We will acknowledge your contribution (unless you prefer to remain anonymous)

### Security Response Process

1. **Verification**: We verify and reproduce the reported vulnerability
2. **Impact Assessment**: We assess the severity and impact
3. **Fix Development**: We develop and test a fix
4. **Coordinated Disclosure**: We coordinate the release with the reporter
5. **Public Disclosure**: We publish a security advisory after the fix is released

## Security Best Practices

When using WebSpark.Bootswatch, please follow these security best practices:

### Input Validation
- Always validate theme names before processing
- Sanitize user inputs in theme switching operations
- Use the built-in validation methods provided by the library

### HTTPS Usage
- Always use HTTPS in production environments
- Ensure all external resources are loaded over HTTPS
- Configure proper SSL/TLS settings

### Content Security Policy (CSP)
- WebSpark.Bootswatch is CSP-friendly (no inline scripts/styles)
- Configure appropriate CSP headers for your application
- Test theme switching functionality with strict CSP policies

### Dependencies
- Keep WebSpark.Bootswatch updated to the latest version
- Regularly update all dependencies
- Monitor security advisories for dependencies

### Example Secure Configuration

```csharp
// Program.cs - Secure configuration example
var builder = WebApplication.CreateBuilder(args);

// Enable HTTPS redirection
builder.Services.AddHttpsRedirection(options =>
{
    options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
    options.HttpsPort = 443;
});

// Configure HSTS
builder.Services.AddHsts(options =>
{
    options.Preload = true;
    options.IncludeSubDomains = true;
    options.MaxAge = TimeSpan.FromDays(365);
});

// Add Bootswatch services
builder.Services.AddBootswatchThemeSwitcher();

var app = builder.Build();

// Security middleware
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
    app.UseHttpsRedirection();
}

// Configure CSP headers
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("Content-Security-Policy", 
        "default-src 'self'; " +
        "style-src 'self' 'unsafe-inline'; " + // Required for dynamic theme loading
        "script-src 'self'; " +
        "img-src 'self' data:; " +
        "font-src 'self';");
    
    await next();
});

app.UseBootswatchAll();
app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();

app.Run();
```

## Security Features

### Built-in Security Measures

- **Input Sanitization**: All theme names are validated and sanitized
- **XSS Protection**: HTML encoding applied to all dynamic outputs
- **CSRF Protection**: Compatible with ASP.NET Core's CSRF protection
- **Safe Defaults**: Fallback to safe default themes on errors
- **No Inline Code**: No inline JavaScript or CSS in generated HTML

### Security Testing

We perform regular security testing including:

- **Static Code Analysis**: Automated security scanning
- **Dependency Scanning**: Regular dependency vulnerability checks
- **Penetration Testing**: Periodic security assessments
- **Code Reviews**: Security-focused code reviews

## Known Security Considerations

### Theme Name Validation
- Theme names are validated against a whitelist
- Invalid characters are stripped or rejected
- Path traversal attempts are blocked

### External Resources
- Bootswatch themes are loaded from trusted CDNs
- Fallback mechanisms prevent loading failures
- Resource integrity checking where possible

### Cookie Security
- Theme preferences stored in secure, HttpOnly cookies
- SameSite attributes configured appropriately
- Cookie expiration properly managed

## Vulnerability History

### CVE Database
We maintain records of any security vulnerabilities:

- **No known CVEs at this time**

### Security Advisories
- All security advisories are published on GitHub Security Advisories
- Subscribe to repository notifications for security updates

## Contact Information

- **Security Email**: [mark@markhazleton.com](mailto:mark@markhazleton.com)
- **Project Maintainer**: Mark Hazleton
- **GitHub**: [@MarkHazleton](https://github.com/MarkHazleton)

## Security Resources

- [OWASP Web Application Security Testing Guide](https://owasp.org/www-project-web-security-testing-guide/)
- [ASP.NET Core Security Best Practices](https://docs.microsoft.com/en-us/aspnet/core/security/)
- [GitHub Security Advisories](https://github.com/MarkHazleton/WebSpark.Bootswatch/security/advisories)

---

Thank you for helping keep WebSpark.Bootswatch and our users safe!