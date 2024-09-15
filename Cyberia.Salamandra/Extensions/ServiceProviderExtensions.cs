using Cyberia.Cytrusaurus;
using Cyberia.Langzilla;
using Cyberia.Salamandra.Managers;
using Cyberia.Salamandra.Services;

using DSharpPlus;
using DSharpPlus.Entities;

using Microsoft.Extensions.DependencyInjection;

namespace Cyberia.Salamandra.Extensions;

/// <summary>
/// Provides extension methods for <see cref="IServiceProvider"/>.
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
        LangsWatcher.CheckLangFinished += LangManager.OnCheckLangFinished;

        return provider;
    }

    /// <summary>
    /// Starts the Salamandra discord client from the services.
    /// </summary>
    /// <param name="provider">The service provider to start the discord client from.</param>
    /// <returns>The service provider.</returns>
    public static async Task<IServiceProvider> StartSalamandraAsync(this IServiceProvider provider)
    {
        DiscordActivity activity = new("Dofus Retro", DiscordActivityType.Playing);
        await provider.GetRequiredService<DiscordClient>().ConnectAsync(activity);

        return provider;
    }
}
