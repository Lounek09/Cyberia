using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.EventArgs;

namespace Cyberia.Salamandra.EventHandlers;

public sealed class ClientEventHandler : IEventHandler<GuildDownloadCompletedEventArgs>
{
    private readonly CachedChannelsManager _cachedChannelsManager;

    private bool _isInitialized;

    public ClientEventHandler(CachedChannelsManager cachedChannelsManager)
    {
        _cachedChannelsManager = cachedChannelsManager;

        _isInitialized = false;
    }

    public async Task HandleEventAsync(DiscordClient sender, GuildDownloadCompletedEventArgs _)
    {
        if (_isInitialized)
        {
            return;
        }

        await _cachedChannelsManager.LoadChannelsAsync();

#if !DEBUG
        await _cachedChannelsManager.SendLogMessage($"{Formatter.Bold(sender.CurrentUser.Username)} started successfully !");
#endif

        _isInitialized = true;
    }
}
