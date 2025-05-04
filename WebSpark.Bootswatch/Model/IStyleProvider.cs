namespace WebSpark.Bootswatch.Model;

/// <summary>
/// Interface for theme style providers that retrieve and manage available themes
/// </summary>
public interface IStyleProvider
{
    /// <summary>
    /// Gets a list of all available theme styles
    /// </summary>
    /// <returns>A collection of StyleModel objects representing the available themes</returns>
    Task<IEnumerable<StyleModel>> GetAsync();

    /// <summary>
    /// Gets a specific theme style by its name
    /// </summary>
    /// <param name="name">The name of the theme to retrieve</param>
    /// <returns>A StyleModel object representing the requested theme</returns>
    Task<StyleModel> GetAsync(string name);
}
