using Cyberia.Cytrusaurus;
using Cyberia.Langzilla;
using Cyberia.Salamandra.Services;

using DSharpPlus;
using DSharpPlus.Entities;

using Microsoft.Extensions.DependencyInjection;

namespace Cyberia.Salamandra.Extensions;

/// <summary>
/// Provides extension methods for the <see cref="IServiceProvider"/> interface.
/// </summary>
public static class ServiceProviderExtensions
{
    extension(IServiceProvider provider)
    {
        /// <summary>
        /// Registers Salamandra specific events from the service provider.
        /// </summary>
        /// <returns>The service provider.</returns>
        public IServiceProvider RegisterSalamandraEvents()
        {
            var cytrusService = provider.GetRequiredService<ICytrusService>();
            var cytrusWatcher = provider.GetRequiredService<ICytrusWatcher>();

            cytrusWatcher.NewCytrusFileDetected += cytrusService.OnNewCytrusFileDetected;
            cytrusWatcher.CytrusErrored += cytrusService.OnCytrusErrored;

            var langsService = provider.GetRequiredService<ILangsService>();
            var langsWatcher = provider.GetRequiredService<ILangsWatcher>();

            langsWatcher.NewLangFilesDetected += langsService.OnNewLangFilesDetected;

            return provider;
        }

        /// <summary>
        /// Starts the Salamandra discord client from the service provider.
        /// </summary>
        /// <returns>The service provider.</returns>
        public async Task<IServiceProvider> StartSalamandraAsync()
        {
            var discordClient = provider.GetRequiredService<DiscordClient>();
            var emojisService = provider.GetRequiredService<IEmojisService>();

            DiscordActivity activity = new("Dofus Retro", DiscordActivityType.Playing);
            await discordClient.ConnectAsync(activity);

            await emojisService.LoadEmojisAsync();

            return provider;
        }

        /// <summary>
        /// Stops the Salamandra discord client from the service provider.
        /// </summary>
        /// <param name="provider">The service provider to get the discord client from.</param>
        /// <returns>The service provider.</returns>
        public async Task<IServiceProvider> StopSalamandraAsync()
        {
            var discordClient = provider.GetRequiredService<DiscordClient>();

            await discordClient.DisconnectAsync();

            return provider;
        }
    }
}
