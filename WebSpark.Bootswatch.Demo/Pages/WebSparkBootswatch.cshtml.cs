using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebSpark.Bootswatch.Demo.Pages
{
    public class WebSparkBootswatchModel : PageModel
    {
        public string BuildDate { get; private set; } = string.Empty;
        public string NuGetVersion { get; private set; } = string.Empty;
        public string NuGetPublished { get; private set; } = string.Empty;
        public string NuGetAuthors { get; private set; } = string.Empty;
        public string NuGetDescription { get; private set; } = string.Empty;
        public string NuGetProjectUrl { get; private set; } = string.Empty;
        public string NuGetLicenseUrl { get; private set; } = string.Empty;
        public string NuGetTags { get; private set; } = string.Empty;
        public string NuGetIconUrl { get; private set; } = string.Empty;
        public string NuGetReadmeUrl { get; private set; } = string.Empty;
        public string NuGetMinClientVersion { get; private set; } = string.Empty;
        public string NuGetTargetFrameworks { get; private set; } = string.Empty;
        public List<string> NuGetDependencies { get; private set; } = new List<string>();

        public async Task OnGetAsync()
        {
            BuildDate = GetBuildDate();
            await FetchNuGetInfo();
        }

        private string GetBuildDate()
        {
            var assembly = typeof(WebSparkBootswatchModel).Assembly;
            var filePath = assembly.Location;
            if (System.IO.File.Exists(filePath))
            {
                var buildDate = System.IO.File.GetLastWriteTime(filePath);
                return buildDate.ToString("MMMM d, yyyy");
            }
            return "Unknown";
        }

        private async Task FetchNuGetInfo()
        {
            var url = "https://api.nuget.org/v3/registration5-gz-semver2/webspark.bootswatch/index.json";
            var handler = new System.Net.Http.HttpClientHandler
            {
                AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate
            };
            using var http = new System.Net.Http.HttpClient(handler);
            try
            {
                using var stream = await http.GetStreamAsync(url);
                using var doc = await System.Text.Json.JsonDocument.ParseAsync(stream);
                var root = doc.RootElement;
                var items = root.GetProperty("items");
                System.Text.Json.JsonElement? latestCatalog = null;
                string? latestVersion = null;
                string? latestPublished = null;
                DateTime latestDate = DateTime.MinValue;
                foreach (var page in items.EnumerateArray())
                {
                    if (!page.TryGetProperty("items", out var pageItems) || pageItems.ValueKind != System.Text.Json.JsonValueKind.Array)
                        continue;
                    foreach (var entry in pageItems.EnumerateArray())
                    {
                        if (!entry.TryGetProperty("catalogEntry", out var catalog))
                            continue;
                        var version = catalog.GetProperty("version").GetString();
                        var published = catalog.GetProperty("published").GetString();
                        if (DateTime.TryParse(published, out var pubDate))
                        {
                            if (pubDate > latestDate)
                            {
                                latestDate = pubDate;
                                latestVersion = version;
                                latestPublished = published;
                                latestCatalog = catalog;
                            }
                        }
                    }
                }
                NuGetVersion = latestVersion ?? "Unavailable";
                if (latestPublished != null && DateTime.TryParse(latestPublished, out var finalDate))
                    NuGetPublished = finalDate.ToString("MMMM d, yyyy");
                else
                    NuGetPublished = latestPublished ?? "Unavailable";
                // Extract additional fields from latestCatalog
                if (latestCatalog.HasValue)
                {
                    var catalog = latestCatalog.Value;
                    NuGetAuthors = catalog.TryGetProperty("authors", out var authors) ? authors.GetString() ?? string.Empty : string.Empty;
                    NuGetDescription = catalog.TryGetProperty("description", out var desc) ? desc.GetString() ?? string.Empty : string.Empty;
                    NuGetProjectUrl = catalog.TryGetProperty("projectUrl", out var proj) ? proj.GetString() ?? string.Empty : string.Empty;
                    NuGetLicenseUrl = catalog.TryGetProperty("licenseUrl", out var lic) ? lic.GetString() ?? string.Empty : string.Empty;
                    NuGetTags = catalog.TryGetProperty("tags", out var tags) ? string.Join(", ", tags.EnumerateArray().Select(t => t.GetString())) : (catalog.TryGetProperty("tags", out var tagsStr) ? tagsStr.GetString() ?? string.Empty : string.Empty);
                    NuGetIconUrl = catalog.TryGetProperty("iconUrl", out var icon) ? icon.GetString() ?? string.Empty : string.Empty;
                    NuGetReadmeUrl = catalog.TryGetProperty("readmeUrl", out var readme) ? readme.GetString() ?? string.Empty : string.Empty;
                    NuGetMinClientVersion = catalog.TryGetProperty("minClientVersion", out var mincli) ? mincli.GetString() ?? string.Empty : string.Empty;
                    // Target Frameworks
                    if (catalog.TryGetProperty("dependencyGroups", out var depGroups) && depGroups.ValueKind == System.Text.Json.JsonValueKind.Array)
                    {
                        var frameworks = new List<string>();
                        var dependencies = new List<string>();
                        foreach (var group in depGroups.EnumerateArray())
                        {
                            if (group.TryGetProperty("targetFramework", out var tf))
                                frameworks.Add(tf.GetString() ?? string.Empty);
                            if (group.TryGetProperty("dependencies", out var deps) && deps.ValueKind == System.Text.Json.JsonValueKind.Array)
                            {
                                foreach (var dep in deps.EnumerateArray())
                                {
                                    if (dep.TryGetProperty("id", out var depId))
                                    {
                                        var depStr = depId.GetString() ?? string.Empty;
                                        if (dep.TryGetProperty("range", out var depRange))
                                            depStr += $" {depRange.GetString()}";
                                        dependencies.Add(depStr);
                                    }
                                }
                            }
                        }
                        NuGetTargetFrameworks = string.Join(", ", frameworks.Where(f => !string.IsNullOrWhiteSpace(f)));
                        NuGetDependencies = dependencies;
                    }
                }
            }
            catch
            {
                NuGetVersion = "Unavailable";
                NuGetPublished = "Unavailable";
                NuGetAuthors = string.Empty;
                NuGetDescription = string.Empty;
                NuGetProjectUrl = string.Empty;
                NuGetLicenseUrl = string.Empty;
                NuGetTags = string.Empty;
                NuGetIconUrl = string.Empty;
                NuGetReadmeUrl = string.Empty;
                NuGetMinClientVersion = string.Empty;
                NuGetTargetFrameworks = string.Empty;
                NuGetDependencies = new List<string>();
            }
        }
    }
}