using FluentAssertions;
using WebSpark.Bootswatch.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebSpark.Bootswatch.Tests;

/// <summary>
/// Tests for StyleModel functionality across all target frameworks
/// </summary>
[TestClass]
public class StyleModelTests
{
    [TestMethod]
    public void StyleModel_ShouldInitialize_WithDefaultValues()
    {
        // Act
        var styleModel = new StyleModel();

        // Assert
        styleModel.Should().NotBeNull();
    }

    [TestMethod]
    [DataRow("cerulean", "Cerulean")]
    [DataRow("cosmo", "Cosmo")]
    [DataRow("darkly", "Darkly")]
    [DataRow("flatly", "Flatly")]
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
