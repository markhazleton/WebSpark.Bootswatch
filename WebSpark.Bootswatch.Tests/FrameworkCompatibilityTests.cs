using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Runtime.InteropServices;
using WebSpark.Bootswatch.Services;
using Xunit;
using Xunit.Abstractions;

namespace WebSpark.Bootswatch.Tests;

/// <summary>
/// Tests that verify framework compatibility across .NET 8.0, 9.0, and 10.0
/// </summary>
public class FrameworkCompatibilityTests
{
    private readonly ITestOutputHelper _output;

    public FrameworkCompatibilityTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void TargetFramework_ShouldBeCorrect()
    {
        // Arrange & Act
        var runtimeVersion = RuntimeInformation.FrameworkDescription;
        
        _output.WriteLine($"Framework: {runtimeVersion}");
        _output.WriteLine($"Runtime: {Environment.Version}");

        // Assert
        runtimeVersion.Should().NotBeNullOrEmpty();
        
        // .NET 8, 9, or 10 will have version 8.0+, 9.0+, or 10.0+
        var version = Environment.Version;
        version.Major.Should().BeOneOf(8, 9, 10);
    }

    [Fact]
    public void StyleCache_ShouldInitialize_OnAllFrameworks()
    {
        // Arrange
        var serviceProvider = CreateTestServiceProvider();

        // Act
        var styleCache = new StyleCache(serviceProvider);

        // Assert
        styleCache.Should().NotBeNull();
        _output.WriteLine($"StyleCache initialized successfully on {RuntimeInformation.FrameworkDescription}");
    }

    [Fact]
    public void FileProviders_ShouldWork_OnAllFrameworks()
    {
        // Arrange & Act
        var embeddedProvider = new Microsoft.Extensions.FileProviders.EmbeddedFileProvider(
            typeof(FrameworkCompatibilityTests).Assembly);

        // Assert
        embeddedProvider.Should().NotBeNull();
        _output.WriteLine($"EmbeddedFileProvider works on {RuntimeInformation.FrameworkDescription}");
    }

    private static IServiceProvider CreateTestServiceProvider()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        return services.BuildServiceProvider();
    }
}
