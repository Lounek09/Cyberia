using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

using System.Text.Encodings.Web;

namespace Cyberia.Amphibian.TagHelpers;

[HtmlTargetElement("a", Attributes = c_htmxMenuItemAttributeName)]
public sealed class HtmxMenuItemTagHelper : TagHelper
{
    private const string c_htmxMenuItemAttributeName = "htmx-menu-item";

    public override int Order => -999; //Run just after the default anchor tag helper

    [ViewContext]
    [HtmlAttributeNotBound]
    public required ViewContext ViewContext { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (output.Attributes.TryGetAttribute("href", out var hrefAttribute))
        {
            output.Attributes.SetAttribute("hx-get", hrefAttribute.Value);
            output.Attributes.SetAttribute("hx-target", "body");
            output.Attributes.SetAttribute("hx-push-url", "true");
            output.AddClass(IsSelected(hrefAttribute.Value.ToString()), HtmlEncoder.Default);
        }

        output.Attributes.RemoveAll(c_htmxMenuItemAttributeName);
    }

    private string IsSelected(string? href)
    {
        return ViewContext.HttpContext.Request.Path.StartsWithSegments(href) ? "selected" : "";
    }
}
