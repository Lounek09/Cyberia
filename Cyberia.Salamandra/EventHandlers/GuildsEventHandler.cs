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

    public async Task HandleEventAsync(DiscordClient _, GuildCreatedEventArgs args)
    {
        var owner = await args.Guild.GetGuildOwnerAsync();

        await _cachedChannelsManager.SendLogMessage($"""
            [NEW] {Formatter.Bold(Formatter.Sanitize(args.Guild.Name))} ({args.Guild.Id})
            created on : {args.Guild.CreationTimestamp}
            Owner : {Formatter.Sanitize(owner.Username)} ({owner.Mention})
            """);
    }

    public async Task HandleEventAsync(DiscordClient _, GuildDeletedEventArgs args)
    {
        await _cachedChannelsManager.SendLogMessage($"[LOSE] {Formatter.Bold(Formatter.Sanitize(args.Guild.Name))} ({args.Guild.Id})");
    }
}
