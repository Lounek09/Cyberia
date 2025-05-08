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
    /// Registers Salamandra specific events from the service provider.
    /// </summary>
    /// <param name="provider">The service provider to register the events from.</param>
    /// <returns>The service provider.</returns>
    public static IServiceProvider RegisterSalamandraEvents(this IServiceProvider provider)
    {
        provider.GetRequiredService<ICytrusWatcher>().NewCytrusFileDetected += provider.GetRequiredService<ICytrusService>().OnNewCytrusFileDetected;
        provider.GetRequiredService<ILangsWatcher>().CheckLangsFinished += provider.GetRequiredService<ILangsService>().OnCheckLangsFinished;

        return provider;
    }


    /// <summary>
    /// Starts the Salamandra discord client from the service provider.
    /// </summary>
    /// <param name="provider">The service provider to get the discord client from.</param>
    /// <returns>The service provider.</returns>
    public static async Task<IServiceProvider> StartSalamandraAsync(this IServiceProvider provider)
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
    public static async Task<IServiceProvider> StopSalamandraAsync(this IServiceProvider provider)
    {
        var discordClient = provider.GetRequiredService<DiscordClient>();

        await discordClient.DisconnectAsync();

        return provider;
    }
}
