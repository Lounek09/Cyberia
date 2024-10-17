using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.EventArgs;

namespace Cyberia.Salamandra.EventHandlers;

/// <summary>
/// Represents the handler for events related to guilds.
/// </summary>
public sealed class GuildsEventHandler : IEventHandler<GuildCreatedEventArgs>, IEventHandler<GuildDeletedEventArgs>
{
    private readonly CachedChannelsManager _cachedChannelsManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="GuildsEventHandler"/> class.
    /// </summary>
    /// <param name="cachedChannelsManager">The manager to get the cached channels from.</param>
    public GuildsEventHandler(CachedChannelsManager cachedChannelsManager)
    {
        _cachedChannelsManager = cachedChannelsManager;
    }

    public async Task HandleEventAsync(DiscordClient _, GuildCreatedEventArgs eventArgs)
    {
        var guild = eventArgs.Guild;
        var owner = await guild.GetGuildOwnerAsync();

        await _cachedChannelsManager.SendLogMessage($"""
            [NEW] {Formatter.Bold(Formatter.Sanitize(guild.Name))} ({guild.Id})
            created on : {guild.CreationTimestamp}
            Owner : {Formatter.Sanitize(owner.Username)} ({owner.Mention})
            """);
    }

    public async Task HandleEventAsync(DiscordClient _, GuildDeletedEventArgs eventArgs)
    {
        var guild = eventArgs.Guild;

        await _cachedChannelsManager.SendLogMessage($"[LOSE] {Formatter.Bold(Formatter.Sanitize(guild.Name))} ({guild.Id})");
    }
}
