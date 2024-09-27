using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.EventArgs;

namespace Cyberia.Salamandra.EventHandlers;

public sealed class GuildsEventHandler : IEventHandler<GuildCreatedEventArgs>, IEventHandler<GuildDeletedEventArgs>
{
    private readonly CachedChannelsManager _cachedChannelsManager;

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
