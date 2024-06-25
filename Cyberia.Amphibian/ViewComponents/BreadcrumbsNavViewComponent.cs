using Microsoft.AspNetCore.Mvc;

using System.Text;

namespace Cyberia.Amphibian.ViewComponents;

public readonly record struct BreadCrumbsItem(string Name, string Route);

public sealed class BreadcrumbsNavViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        List<BreadCrumbsItem> items = [];

        var path = ViewContext.HttpContext.Request.Path.ToString().AsSpan();
        if (path.Length < 2)
        {
            return View(items);
        }

        // Handle first slash
        items.Add(new BreadCrumbsItem(WebTranslations.Page_Index_Title, "/index"));
        path = path[1..];

        // Handle culture segment
        var index = path.IndexOf('/');
        if (index == -1)
        {
            return View(items);
        }
        path = path[(index + 1)..];

        // Handle remaining segments
        StringBuilder routeBuilder = new();
        StringBuilder translationKeyBuilder = new();
        while (!path.IsEmpty)
        {
            index = path.IndexOf('/');
            var segment = (index == -1 ? path : path[..index]).ToString();

            translationKeyBuilder.Append(segment.Capitalize());
            routeBuilder.Append('/').Append(segment);

            //TODO: Need a more robust way to handle this
            var name = WebTranslations.ResourceManager.GetString($"Page.{translationKeyBuilder}.Title") ?? segment;

            items.Add(new BreadCrumbsItem(name, routeBuilder.ToString()));

            path = index == -1 ? ReadOnlySpan<char>.Empty : path[(index + 1)..];
        }

        return View(items);
    }
}
