namespace Cyberia.Amphibian.Extensions;

/// <summary>
/// Provides extension methods for the <see cref="IApplicationBuilder"/> interface.
/// </summary>
public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Adds the HTTPS redirection middleware to the application pipeline if needed.
    /// </summary>
    /// <param name="builder">The application builder.</param>
    /// <param name="urls">The URLs to check for HTTPS.</param>
    /// <returns>The updated application builder.</returns>
    public static IApplicationBuilder UseHttpsRedirectionIfNeeded(this IApplicationBuilder builder, params string[] urls)
    {
        var hasHttps = urls.Any(url => url.StartsWith("https://"));
        var hasHttp = urls.Any(url => url.StartsWith("http://"));

        if (hasHttps && hasHttp)
        {
            builder.UseHttpsRedirection();
        }

        return builder;
    }
}
