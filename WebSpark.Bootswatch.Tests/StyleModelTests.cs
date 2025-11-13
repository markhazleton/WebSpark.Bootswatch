using FluentAssertions;
using WebSpark.Bootswatch.Model;
using Xunit;

namespace WebSpark.Bootswatch.Tests;

/// <summary>
/// Tests for StyleModel functionality across all target frameworks
/// </summary>
public class StyleModelTests
{
    [Fact]
    public void StyleModel_ShouldInitialize_WithDefaultValues()
    {
        // Act
        var styleModel = new StyleModel();

        // Assert
        styleModel.Should().NotBeNull();
    }

    [Theory]
    [InlineData("cerulean", "Cerulean")]
    [InlineData("cosmo", "Cosmo")]
    [InlineData("darkly", "Darkly")]
    [InlineData("flatly", "Flatly")]
    public void StyleModel_ShouldStore_ThemeNames(string themeName, string displayName)
    {
        // Arrange
        var styleModel = new StyleModel
        {
            name = themeName,
            description = displayName
        };

        // Assert
        styleModel.name.Should().Be(themeName);
        styleModel.description.Should().Be(displayName);
    }
}
