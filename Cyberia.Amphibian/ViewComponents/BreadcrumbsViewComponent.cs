using Microsoft.AspNetCore.Mvc;

using System.Text;

namespace Cyberia.Amphibian.ViewComponents;

public readonly record struct BreadCrumbsItem(string Name, string Route);

public sealed class BreadcrumbsViewComponent : ViewComponent
{
    //TODO: Add localization
    public IViewComponentResult Invoke()
    {
        List<BreadCrumbsItem> items = [];

        var path = ViewContext.HttpContext.Request.Path.ToString().AsSpan();
        if (path.Length <= 1)
        {
            return View(items);
        }

        items.Add(new BreadCrumbsItem("Accueil", "/"));
        path = path[1..];

        StringBuilder routeBuilder = new();

        while (!path.IsEmpty)
        {
            var index = path.IndexOf('/');
            var segment = index == -1 ? path : path[..index];

            routeBuilder.Append('/').Append(segment);

            items.Add(new BreadCrumbsItem(segment.ToString(), routeBuilder.ToString()));
            path = index == -1 ? ReadOnlySpan<char>.Empty : path[(index + 1)..];
        }

        return View(items);
    }
}
