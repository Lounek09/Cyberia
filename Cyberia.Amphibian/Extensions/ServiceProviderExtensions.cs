namespace Cyberia.Amphibian.Extensions;

/// <summary>
/// Provides extension methods for the <see cref="IServiceProvider"/> interface.
/// </summary>
public static class ServiceProviderExtensions
{
#pragma warning disable IDE0052 // Remove unread private members
    private static Task? _webAppTask = null;
#pragma warning restore IDE0052 // Remove unread private members

    /// <summary>
    /// Starts the Amphibian web application from the services.
    /// </summary>
    /// <param name="provider">The service provider to get the web application from.</param>
    /// <returns>The service provider.</returns>
    public static IServiceProvider StartAmphibian(this IServiceProvider provider)
    {
        var webApp = provider.GetRequiredService<WebApplication>();

        _webAppTask ??= webApp.RunAsync();

        return provider;
    }

    /// <summary>
    /// Stops the Amphibian web application from the services.
    /// </summary>
    /// <param name="provider">The service provider to get the web application from.</param>
    /// <returns>The service provider.</returns>
    public static async Task<IServiceProvider> StopAmphibianAsync(this IServiceProvider provider)
    {
        var webApp = provider.GetRequiredService<WebApplication>();

        if (_webAppTask is not null)
        {
            await webApp.StopAsync();
            _webAppTask = null;
        }

        return provider;
    }
}
