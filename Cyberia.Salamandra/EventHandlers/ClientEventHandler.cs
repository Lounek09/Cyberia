using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace Cyberia.Salamandra.EventHandlers;

public sealed class ClientEventHandler : IEventHandler<GuildDownloadCompletedEventArgs>
{
    private readonly BotConfig _botConfig;
    private bool _isInitialized;

    public ClientEventHandler(BotConfig botConfig)
    {
        _botConfig = botConfig;
        _isInitialized = false;
    }

    public async Task HandleEventAsync(DiscordClient sender, GuildDownloadCompletedEventArgs _)
    {
        if (_isInitialized)
        {
            return;
        }

        await Task.WhenAll(
           sender.SetChannelAsync<DiscordChannel>(_botConfig.LogChannelId, nameof(ChannelManager.LogChannel), x => ChannelManager.LogChannel = x),
           sender.SetChannelAsync<DiscordChannel>(_botConfig.ErrorChannelId, nameof(ChannelManager.ErrorChannel), x => ChannelManager.ErrorChannel = x),
           sender.SetChannelAsync<DiscordForumChannel>(_botConfig.LangForumChannelId, nameof(ChannelManager.LangForumChannel), x => ChannelManager.LangForumChannel = x),
           sender.SetChannelAsync<DiscordChannel>(_botConfig.CytrusChannelId, nameof(ChannelManager.CytrusChannel), x => ChannelManager.CytrusChannel = x),
           sender.SetChannelAsync<DiscordChannel>(_botConfig.CytrusManifestChannelId, nameof(ChannelManager.CytrusManifestChannel), x => ChannelManager.CytrusManifestChannel = x));

#if !DEBUG
        await MessageManager.SendLogMessage($"{Formatter.Bold(sender.CurrentUser.Username)} started successfully !");
#endif

        _isInitialized = true;
    }
}
