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

    internal static async Task SetChannelAsync<T>(this DiscordClient client, ulong id, string name, Action<T> set)
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

            Log.Error("The {Name} channel with ID {ChannelId} is not of type {ChannelType}", name, id, typeof(T).Name);
        }
        catch
        {
            Log.Error("Failed to get the {Name} channel with ID {ChannelId}", name, id);
        }
    }
}
