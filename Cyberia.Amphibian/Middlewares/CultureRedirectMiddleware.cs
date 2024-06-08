using Microsoft.Extensions.Options;

using System.Globalization;

namespace Cyberia.Amphibian.Middlewares;

public sealed class CultureRedirectMiddleware
{
    private readonly RequestDelegate _next;
    private readonly RequestLocalizationOptions _requestLocalizationOptions;

    public CultureRedirectMiddleware(RequestDelegate next, IOptions<RequestLocalizationOptions> requestLocalizationOptions)
    {
        _next = next;
        _requestLocalizationOptions = requestLocalizationOptions.Value;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.ToString().AsMemory();
        if (path.Length < 2)
        {
            context.Response.Redirect($"/{CultureInfo.CurrentCulture.Name}");
            return;
        }

        // Handle first slash
        path = path[1..];

        var index = path.Span.IndexOf('/');
        var firstSegment = index == -1 ? path : path[..index];

        if (!_requestLocalizationOptions.SupportedCultures!.Any(x => firstSegment.Span.SequenceEqual(x.Name)))
        {
            context.Response.Redirect($"/{CultureInfo.CurrentCulture.Name}{path.TrimStart(firstSegment.Span)}");
            return;
        }

        await _next(context);
    }
}
