using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace Cyberia.Amphibian.Controllers;

public sealed class CultureController : Controller
{
    public IActionResult SetCookie(string culture, string returnPath)
    {
        HttpContext.Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions { Expires = DateTimeOffset.MaxValue });

        var path = returnPath.AsSpan()[1..];
        var index = path.IndexOf('/');

        if (index == -1)
        {
            return LocalRedirect($"/{culture}");
        }

        return LocalRedirect($"/{culture}{path[index..]}");
    }
}
