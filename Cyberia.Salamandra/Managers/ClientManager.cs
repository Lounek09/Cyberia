using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace Cyberia.Salamandra.Managers;

public static class ClientManager
{
    private static bool _isInitialized = false;

    internal static async Task OnGuildDownloadCompleted(DiscordClient sender, GuildDownloadCompletedEventArgs _)
    {
        if (_isInitialized)
        {
            return;
        }

        await Task.WhenAll(
           sender.SetChannelAsync<DiscordChannel>(Bot.Config.LogChannelId, nameof(ChannelManager.LogChannel), x => ChannelManager.LogChannel = x),
           sender.SetChannelAsync<DiscordChannel>(Bot.Config.ErrorChannelId, nameof(ChannelManager.ErrorChannel), x => ChannelManager.ErrorChannel = x),
           sender.SetChannelAsync<DiscordForumChannel>(Bot.Config.LangForumChannelId, nameof(ChannelManager.LangForumChannel), x => ChannelManager.LangForumChannel = x),
           sender.SetChannelAsync<DiscordChannel>(Bot.Config.CytrusChannelId, nameof(ChannelManager.CytrusChannel), x => ChannelManager.CytrusChannel = x),
           sender.SetChannelAsync<DiscordChannel>(Bot.Config.CytrusManifestChannelId, nameof(ChannelManager.CytrusManifestChannel), x => ChannelManager.CytrusManifestChannel = x));

#if !DEBUG
        await MessageManager.SendLogMessage($"{Formatter.Bold(sender.CurrentUser.Username)} started successfully !");
#endif

        _isInitialized = true;
    }
}
