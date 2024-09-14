using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

using Microsoft.Extensions.DependencyInjection;

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

        var config = sender.ServiceProvider.GetRequiredService<BotConfig>();

        await Task.WhenAll(
           sender.SetChannelAsync<DiscordChannel>(config.LogChannelId, nameof(ChannelManager.LogChannel), x => ChannelManager.LogChannel = x),
           sender.SetChannelAsync<DiscordChannel>(config.ErrorChannelId, nameof(ChannelManager.ErrorChannel), x => ChannelManager.ErrorChannel = x),
           sender.SetChannelAsync<DiscordForumChannel>(config.LangForumChannelId, nameof(ChannelManager.LangForumChannel), x => ChannelManager.LangForumChannel = x),
           sender.SetChannelAsync<DiscordChannel>(config.CytrusChannelId, nameof(ChannelManager.CytrusChannel), x => ChannelManager.CytrusChannel = x),
           sender.SetChannelAsync<DiscordChannel>(config.CytrusManifestChannelId, nameof(ChannelManager.CytrusManifestChannel), x => ChannelManager.CytrusManifestChannel = x));

#if !DEBUG
        await MessageManager.SendLogMessage($"{Formatter.Bold(sender.CurrentUser.Username)} started successfully !");
#endif

        _isInitialized = true;
    }
}
