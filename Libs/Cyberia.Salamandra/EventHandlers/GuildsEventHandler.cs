using Cyberia.Salamandra.Services;

using DSharpPlus;
using DSharpPlus.EventArgs;

namespace Cyberia.Salamandra.EventHandlers;

/// <summary>
/// Represents the handler for events related to guilds.
/// </summary>
public sealed class GuildsEventHandler : IEventHandler<GuildDownloadCompletedEventArgs>
{
    private readonly ICachedChannelsManager _cachedChannelsManager;

    private bool _isInitialized;

    /// <summary>
    /// Initializes a new instance of the <see cref="GuildsEventHandler"/> class.
    /// </summary>
    /// <param name="cachedChannelsManager">The service to get the cached channels from.</param>
    public GuildsEventHandler(ICachedChannelsManager cachedChannelsManager)
    {
        _cachedChannelsManager = cachedChannelsManager;
    }

    public async Task HandleEventAsync(DiscordClient sender, GuildDownloadCompletedEventArgs _)
    {
        if (!_isInitialized)
        {
            await _cachedChannelsManager.LoadChannelsAsync();

#if !DEBUG
        await _cachedChannelsManager.SendLogMessage($"{Formatter.Bold(sender.CurrentUser.Username)} started successfully !");
#endif

            _isInitialized = true;
        }
    }
}
