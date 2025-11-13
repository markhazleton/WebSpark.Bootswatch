using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using WebSpark.Bootswatch.Services;
using Xunit;

namespace WebSpark.Bootswatch.Tests;

/// <summary>
/// Tests for StyleCache service across all target frameworks
/// </summary>
public class StyleCacheTests
{
    [Fact]
    public void StyleCache_ShouldInitialize()
    {
        // Arrange
        var serviceProvider = CreateServiceProvider();

        // Act
        var styleCache = new StyleCache(serviceProvider);

        // Assert
        styleCache.Should().NotBeNull();
    }

    [Fact]
    public void GetAllStyles_ShouldReturnList()
    {
        // Arrange
        var serviceProvider = CreateServiceProvider();
        var styleCache = new StyleCache(serviceProvider);

        // Act
        var styles = styleCache.GetAllStyles();

        // Assert
        styles.Should().NotBeNull();
        styles.Should().BeOfType<List<Model.StyleModel>>();
    }

    [Fact]
    public void GetStyle_WithNonExistentName_ShouldReturnEmptyModel()
    {
        // Arrange
        var serviceProvider = CreateServiceProvider();
        var styleCache = new StyleCache(serviceProvider);

        // Act
        var style = styleCache.GetStyle("nonexistent-theme");

        // Assert
        style.Should().NotBeNull();
    }

    private static IServiceProvider CreateServiceProvider()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddSingleton<StyleCache>();
        return services.BuildServiceProvider();
    }
}
