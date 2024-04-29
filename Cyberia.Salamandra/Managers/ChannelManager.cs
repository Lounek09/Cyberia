using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Managers;

public static class ChannelManager
{
    public static DiscordChannel? LogChannel { get; internal set; }
    public static DiscordChannel? ErrorChannel { get; internal set; }
    public static DiscordForumChannel? LangForumChannel { get; internal set; }
    public static DiscordChannel? CytrusChannel { get; internal set; }
    public static DiscordChannel? CytrusManifestChannel { get; internal set; }

    public static async Task SetChannelAsync<T>(this DiscordClient client, ulong id, string name, Action<T> set)
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
            }
            else
            {
                Log.Error("Channel ID {ChannelId} is not of type {ChannelType}, expected for {Identifier}", id, typeof(T).Name, name);
            }
        }
        catch
        {
            Log.Error("Failed to set channel with ID {ChannelId} for {Identifier}", id, name);
        }
    }
}
