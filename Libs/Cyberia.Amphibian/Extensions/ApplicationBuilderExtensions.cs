namespace Cyberia.Amphibian.Extensions;

/// <summary>
/// Provides extension methods for the <see cref="IApplicationBuilder"/> interface.
/// </summary>
public static class ApplicationBuilderExtensions
{
    extension(IApplicationBuilder builder)
    {
        /// <summary>
        /// Adds the HTTPS redirection middleware to the application pipeline if needed.
        /// </summary>
        /// <param name="urls">The URLs to check for HTTPS.</param>
        /// <returns>The updated application builder.</returns>
        public IApplicationBuilder UseHttpsRedirectionIfNeeded(params IReadOnlyList<string> urls)
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
}
