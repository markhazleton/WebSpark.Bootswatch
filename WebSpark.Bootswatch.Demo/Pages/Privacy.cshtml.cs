using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebSpark.Bootswatch.Demo.Pages
{
    public class PrivacyModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;

        // Make the setter public to match the class visibility (to fix the required member error)
        public required string CurrentColorMode { get; set; }

        public PrivacyModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            // Get the current color mode from cookie or default to "light"
            CurrentColorMode = Request.Cookies["bootswatch-color-mode"] ?? "light";
            _logger.LogInformation("Privacy page accessed with color mode: {ColorMode}", CurrentColorMode);
        }
    }
}