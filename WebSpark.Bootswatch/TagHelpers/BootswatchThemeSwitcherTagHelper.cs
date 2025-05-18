using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using WebSpark.Bootswatch.Helpers;
using WebSpark.Bootswatch.Services;

namespace WebSpark.Bootswatch.TagHelpers
{
    [HtmlTargetElement("bootswatch-theme-switcher")]
    public class BootswatchThemeSwitcherTagHelper : TagHelper
    {
        private readonly StyleCache _styleCache;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BootswatchThemeSwitcherTagHelper(StyleCache styleCache, IHttpContextAccessor httpContextAccessor)
        {
            _styleCache = styleCache;
            _httpContextAccessor = httpContextAccessor;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var html = BootswatchThemeHelper.GetThemeSwitcherHtml(_styleCache, _httpContextAccessor.HttpContext);
            output.TagName = null; // Remove the wrapping tag
            output.Content.SetHtmlContent(html);
        }
    }
}
