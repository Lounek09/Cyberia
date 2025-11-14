namespace Cyberia.Amphibian.Extensions;

/// <summary>
/// Provides extension methods for the <see cref="IServiceProvider"/> interface.
/// </summary>
public static class ServiceProviderExtensions
{
    private static Task? s_webAppTask = null;

    extension(IServiceProvider provider)
    {
        /// <summary>
        /// Starts the Amphibian web application from the services.
        /// </summary>
        /// <returns>The service provider.</returns>
        public IServiceProvider StartAmphibian()
        {
            var webApp = provider.GetRequiredService<WebApplication>();

            s_webAppTask ??= webApp.RunAsync();

            return provider;
        }

        /// <summary>
        /// Stops the Amphibian web application from the services.
        /// </summary>
        /// <returns>The service provider.</returns>
        public async Task<IServiceProvider> StopAmphibianAsync()
        {
            var webApp = provider.GetRequiredService<WebApplication>();

            if (s_webAppTask is not null)
            {
                await webApp.StopAsync();
                s_webAppTask = null;
            }

            return provider;
        }
    }
}
