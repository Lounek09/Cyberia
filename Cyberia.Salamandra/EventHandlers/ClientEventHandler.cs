using Cyberia.Salamandra.Services;

using DSharpPlus;
using DSharpPlus.EventArgs;

namespace Cyberia.Salamandra.EventHandlers;

/// <summary>
/// Represents the handler for events related to the Discord client.
/// </summary>
public sealed class ClientEventHandler : IEventHandler<GuildDownloadCompletedEventArgs>
{
    private readonly ICachedChannelsService _cachedChannelsService;

    private bool _isInitialized;

    /// <summary>
    /// Initializes a new instance of the <see cref="ClientEventHandler"/> class.
    /// </summary>
    /// <param name="cachedChannelsService">The service to get the cached channels from.</param>
    public ClientEventHandler(ICachedChannelsService cachedChannelsService)
    {
        _cachedChannelsService = cachedChannelsService;

        _isInitialized = false;
    }

    public async Task HandleEventAsync(DiscordClient sender, GuildDownloadCompletedEventArgs _)
    {
        if (_isInitialized)
        {
            return;
        }

        await _cachedChannelsService.LoadChannelsAsync();

#if !DEBUG
        await _cachedChannelsService.SendLogMessage($"{Formatter.Bold(sender.CurrentUser.Username)} started successfully !");
#endif

        _isInitialized = true;
    }
}
