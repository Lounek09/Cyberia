using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using System.Globalization;

namespace Cyberia.Amphibian.Middlewares;

public sealed class CultureRedirectMiddleware
{
    private readonly RequestDelegate _next;
    private readonly HashSet<string> _supportedUICultures;
    private readonly HashSet<string>.AlternateLookup<ReadOnlySpan<char>> _supportedUICulturesLookup;

    public CultureRedirectMiddleware(RequestDelegate next, IOptions<RequestLocalizationOptions> requestLocalizationOptions)
    {
        _next = next;
        _supportedUICultures = requestLocalizationOptions.Value.SupportedUICultures is null
            ? []
            : requestLocalizationOptions.Value.SupportedUICultures
                .Select(culture => culture.TwoLetterISOLanguageName)
                .ToHashSet(StringComparer.Ordinal);
        _supportedUICulturesLookup = _supportedUICultures.GetAlternateLookup<ReadOnlySpan<char>>();
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        if (endpoint?.Metadata.GetMetadata<ControllerAttribute>() is not null)
        {
            await _next(context);
            return;
        }

        var pathString = context.Request.Path;
        if (pathString.Value?.Length < 2)
        {
            context.Response.Redirect($"/{CultureInfo.CurrentUICulture.TwoLetterISOLanguageName}");
            return;
        }

        var pathSpan = pathString.Value.AsSpan(1);
        var slashIndex = pathSpan.IndexOf('/');
        var firstSegment = slashIndex == -1 ? pathSpan : pathSpan[..slashIndex];

        if (!_supportedUICulturesLookup.Contains(firstSegment))
        {
            context.Response.Redirect($"/{CultureInfo.CurrentUICulture.TwoLetterISOLanguageName}{pathString}");
            return;
        }

        await _next(context);
    }
}
