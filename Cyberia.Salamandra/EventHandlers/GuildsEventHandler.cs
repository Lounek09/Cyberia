﻿using Cyberia.Salamandra.Services;

using DSharpPlus;
using DSharpPlus.EventArgs;

namespace Cyberia.Salamandra.EventHandlers;

/// <summary>
/// Represents the handler for events related to guilds.
/// </summary>
public sealed class GuildsEventHandler : IEventHandler<GuildCreatedEventArgs>, IEventHandler<GuildDeletedEventArgs>
{
    private readonly ICachedChannelsManager _cachedChannelsManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="GuildsEventHandler"/> class.
    /// </summary>
    /// <param name="cachedChannelsManager">The service to get the cached channels from.</param>
    public GuildsEventHandler(ICachedChannelsManager cachedChannelsManager)
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
