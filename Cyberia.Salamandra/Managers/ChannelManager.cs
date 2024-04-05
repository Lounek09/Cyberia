using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace Cyberia.Salamandra.Managers;

public static class ChannelManager
{
    public static DiscordChannel? LogChannel { get; private set; }
    public static DiscordChannel? ErrorChannel { get; private set; }
    public static DiscordForumChannel? LangForumChannel { get; private set; }
    public static DiscordChannel? CytrusChannel { get; private set; }
    public static DiscordChannel? CytrusManifestChannel { get; private set; }

    internal static async Task OnGuildDownloadCompleted(DiscordClient sender, GuildDownloadCompletedEventArgs _)
    {
        await Task.WhenAll(
           sender.SetChannelAsync<DiscordChannel>(Bot.Config.LogChannelId, "log", x => LogChannel = x),
           sender.SetChannelAsync<DiscordChannel>(Bot.Config.ErrorChannelId, "error", x => ErrorChannel = x),
           sender.SetChannelAsync<DiscordForumChannel>(Bot.Config.LangForumChannelId, "lang forum", x => LangForumChannel = x),
           sender.SetChannelAsync<DiscordChannel>(Bot.Config.CytrusChannelId, "cytrus", x => CytrusChannel = x),
           sender.SetChannelAsync<DiscordChannel>(Bot.Config.CytrusManifestChannelId, "cytrus manifest", x => CytrusManifestChannel = x));
    }

    private static async Task SetChannelAsync<T>(this DiscordClient client, ulong id, string name, Action<T> set)
        where T : DiscordChannel
    {
        if (id == 0)
        {
            return;
        }

        try
        {
            var channel = await client.GetChannelAsync(id);
            if (channel is T typedChannel)
            {
                set(typedChannel);
                return;
            }

            Log.Error("The given {ChannelName} channel {ChannelId} is not of type {ChannelType}", name, id, typeof(T).Name);
        }
        catch
        {
            Log.Error("Unknown {ChannelName} channel {ChannelId}", name, id);
        }
    }
}
