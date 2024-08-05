using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

using System.Text.Encodings.Web;

namespace Cyberia.Amphibian.TagHelpers;

[HtmlTargetElement("a", Attributes = c_MainMenuItemAttributeName)]
public sealed class MainMenuItemTagHelper : TagHelper
{
    private const string c_MainMenuItemAttributeName = "main-menu-item";

    public override int Order => -999; //Run just after the default anchor tag helper

    [ViewContext]
    [HtmlAttributeNotBound]
    public required ViewContext ViewContext { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (output.Attributes.TryGetAttribute("href", out var hrefAttribute))
        {
            var selected = IsSelected(hrefAttribute.Value.ToString());
            if (!string.IsNullOrEmpty(selected))
            {
                output.AddClass(selected, HtmlEncoder.Default);
            }
        }

        output.Attributes.RemoveAll(c_MainMenuItemAttributeName);
    }

    private string IsSelected(string? href)
    {
        return ViewContext.HttpContext.Request.Path.StartsWithSegments(href) ? "selected" : "";
    }
}
