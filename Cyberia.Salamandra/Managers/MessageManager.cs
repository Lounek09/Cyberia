using Cyberia.Salamandra.DsharpPlus;

using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Managers;

public static class MessageManager
{
    public static async Task SendLogMessage(string content)
    {
        var channel = ChannelManager.LogChannel;
        if (channel is null)
        {
            return;
        }

        await channel.SendMessageSafe(new DiscordMessageBuilder().WithContent(content));
    }

    public static async Task SendErrorMessage(DiscordEmbed embed)
    {
        var channel = ChannelManager.ErrorChannel;
        if (channel is null)
        {
            return;
        }

        await channel.SendMessageSafe(new DiscordMessageBuilder().AddEmbed(embed));
    }
}
