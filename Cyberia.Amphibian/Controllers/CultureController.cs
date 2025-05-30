using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Cyberia.Amphibian.Controllers;

public sealed class CultureController : Controller
{
    private readonly RequestLocalizationOptions _requestLocalizationOptions;

    public CultureController(IOptions<RequestLocalizationOptions> requestLocalizationOptions)
    {
        _requestLocalizationOptions = requestLocalizationOptions.Value;
    }

    public IActionResult SetCookie(string culture, string returnPath)
    {
        if (string.IsNullOrWhiteSpace(culture) ||
            !_requestLocalizationOptions.SupportedUICultures!.Any(x => x.TwoLetterISOLanguageName.Equals(culture)))
        {
            return BadRequest();
        }

        HttpContext.Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions { Expires = DateTimeOffset.MaxValue });

        if (returnPath.Length < 2)
        {
            return LocalRedirect($"/{culture}");
        }

        var path = returnPath.AsSpan();
        path = path[0] == '/' ? path[1..] : path;

        var index = path.IndexOf('/');
        if (index == -1)
        {
            return LocalRedirect($"/{culture}");
        }

        return LocalRedirect($"/{culture}{path[index..]}");
    }
}
