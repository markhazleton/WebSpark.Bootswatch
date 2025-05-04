namespace WebSpark.Bootswatch.Model;


/// <summary>
/// Style Service
/// </summary>
public interface IStyleProvider
{
    /// <summary>
    /// Get List of Themes
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<StyleModel>> GetAsync();
    /// <summary>
    /// Get Theme By Name
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    Task<StyleModel> GetAsync(string name);
}
