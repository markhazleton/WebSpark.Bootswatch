using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Runtime.InteropServices;
using WebSpark.Bootswatch.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebSpark.Bootswatch.Tests;

/// <summary>
/// Tests that verify framework compatibility across .NET 8.0, 9.0, and 10.0
/// </summary>
[TestClass]
public class FrameworkCompatibilityTests
{
    public TestContext? TestContext { get; set; }

    [TestMethod]
    public void TargetFramework_ShouldBeCorrect()
    {
        // Arrange & Act
        var runtimeVersion = RuntimeInformation.FrameworkDescription;

        TestContext?.WriteLine($"Framework: {runtimeVersion}");
        TestContext?.WriteLine($"Runtime: {Environment.Version}");

        // Assert
        runtimeVersion.Should().NotBeNullOrEmpty();

        // .NET 8, 9, or 10 will have version 8.0+, 9.0+, or 10.0+
        var version = Environment.Version;
        version.Major.Should().BeOneOf(8, 9, 10);
    }

    [TestMethod]
    public void StyleCache_ShouldInitialize_OnAllFrameworks()
    {
        // Arrange
        var serviceProvider = CreateTestServiceProvider();

        // Act
        var styleCache = new StyleCache(serviceProvider);

        // Assert
        styleCache.Should().NotBeNull();
        TestContext?.WriteLine($"StyleCache initialized successfully on {RuntimeInformation.FrameworkDescription}");
    }

    [TestMethod]
    public void FileProviders_ShouldWork_OnAllFrameworks()
    {
        // Arrange & Act
        var embeddedProvider = new Microsoft.Extensions.FileProviders.EmbeddedFileProvider(
            typeof(FrameworkCompatibilityTests).Assembly);

        // Assert
        embeddedProvider.Should().NotBeNull();
        TestContext?.WriteLine($"EmbeddedFileProvider works on {RuntimeInformation.FrameworkDescription}");
    }

    private static IServiceProvider CreateTestServiceProvider()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddSingleton<StyleCache>();
        return services.BuildServiceProvider();
    }
}
