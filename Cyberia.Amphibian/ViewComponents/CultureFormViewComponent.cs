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
            CultureOption option = new(cultureInfo, cultureInfo.TwoLetterISOLanguageName.Equals(CultureInfo.CurrentUICulture.TwoLetterISOLanguageName) ? "selected" : null);
            items.Add(option);
        }

        return View(items);
    }
}
