using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using System.Globalization;

namespace Cyberia.Amphibian.ViewComponents;

public readonly record struct CultureOption(CultureInfo CultureInfo, string? Selected);

public sealed class CultureFormViewComponent : ViewComponent
{
    private readonly RequestLocalizationOptions _requestLocalizationOptions;

    public CultureFormViewComponent(IOptions<RequestLocalizationOptions> requestLocalizationOptions)
    {
        _requestLocalizationOptions = requestLocalizationOptions.Value;
    }

    public IViewComponentResult Invoke()
    {
        List<CultureOption> items = [];

        foreach (var cultureInfo in _requestLocalizationOptions.SupportedCultures!)
        {
            items.Add(new CultureOption(cultureInfo, cultureInfo.Name.Equals(CultureInfo.CurrentCulture.Name) ? "selected" : null));
        }

        return View(items);
    }
}
