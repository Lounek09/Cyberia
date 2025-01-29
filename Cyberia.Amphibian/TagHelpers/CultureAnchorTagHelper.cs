using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Cyberia.Amphibian.TagHelpers;

[HtmlTargetElement("a", Attributes = c_culturePageAttributeName)]
public sealed class CultureAnchorTagHelper : AnchorTagHelper
{
    private const string c_culturePageAttributeName = "asp-culture-page";

    public CultureAnchorTagHelper(IHtmlGenerator generator)
        : base(generator)
    {

    }

    [HtmlAttributeName(c_culturePageAttributeName)]
    public required string CulturePage { get; set; }

    public override void Init(TagHelperContext context)
    {
        Page = CulturePage;

        if (ViewContext.ViewData.TryGetValue("Culture", out var value) && value is string culture)
        {
            RouteValues.Add("culture", culture);
        }
    }
}
