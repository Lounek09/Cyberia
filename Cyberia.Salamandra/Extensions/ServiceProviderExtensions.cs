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
    /// <summary>
    /// Registers Salamandra specific events from the services.
    /// </summary>
    /// <param name="provider">The service provider to register the events from.</param>
    /// <returns>The service provider.</returns>
    public static IServiceProvider RegisterSalamandraEvents(this IServiceProvider provider)
    {
        CytrusWatcher.NewCytrusFileDetected += provider.GetRequiredService<CytrusService>().OnNewCytrusDetected;
        LangsWatcher.CheckLangFinished += provider.GetRequiredService<LangsService>().OnCheckLangFinished;

        return provider;
    }

    /// <summary>
    /// Starts the Salamandra discord client from the services.
    /// </summary>
    /// <param name="provider">The service provider to start the discord client from.</param>
    /// <returns>The service provider.</returns>
    public static async Task<IServiceProvider> StartSalamandraAsync(this IServiceProvider provider)
    {
        var discordClient = provider.GetRequiredService<DiscordClient>();

        DiscordActivity activity = new("Dofus Retro", DiscordActivityType.Playing);
        await discordClient.ConnectAsync(activity);

        return provider;
    }
}
