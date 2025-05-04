using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace WebSpark.Bootswatch.Demo.Pages
{
    public class PrivacyModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;

        public string CurrentColorMode { get; private set; }

        public PrivacyModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            // Get the current color mode from cookie or default to "light"
            CurrentColorMode = Request.Cookies["color-mode"] ?? "light";
            _logger.LogInformation("Privacy page accessed with color mode: {ColorMode}", CurrentColorMode);
        }
    }
}