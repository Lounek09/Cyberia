﻿using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Cyberia.Amphibian.TagHelpers;

[HtmlTargetElement("a", Attributes = c_culturePageAttributeName)]
public class CultureAnchorTagHelper : AnchorTagHelper
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

        if (!RouteValues.TryGetValue("culture", out _))
        {
            RouteValues.Add("culture", ViewContext.ViewBag.Culture);
        }
    }
}