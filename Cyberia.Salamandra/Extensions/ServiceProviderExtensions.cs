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
    /// Starts the Salamandra discord client from the service provider.
    /// </summary>
    /// <param name="provider">The service provider to get the discord client from.</param>
    /// <returns>The service provider.</returns>
    public static async Task<IServiceProvider> StartSalamandraAsync(this IServiceProvider provider)
    {
        var discordClient = provider.GetRequiredService<DiscordClient>();

        DiscordActivity activity = new("Dofus Retro", DiscordActivityType.Playing);
        await discordClient.ConnectAsync(activity);

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
