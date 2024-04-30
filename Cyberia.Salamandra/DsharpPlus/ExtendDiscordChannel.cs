using DSharpPlus.Entities;

namespace Cyberia.Salamandra.DsharpPlus;

public static class ExtendDiscordChannel
{
    public static async Task SendMessageSafe(this DiscordChannel channel, DiscordMessageBuilder message)
    {
        var permissions = channel.Guild.CurrentMember.PermissionsIn(channel);

        if (!permissions.HasPermission(DiscordPermissions.AccessChannels))
        {
            Log.Error("No permission to access to this channel {ChannelId}", channel.Id);
            return;
        }

        if (!permissions.HasPermission(DiscordPermissions.SendMessages))
        {
            Log.Error("No permission to send message in this channel {ChannelId}", channel.Id);
            return;
        }

        if (message.Files.Count > 0 && !permissions.HasPermission(DiscordPermissions.AttachFiles))
        {
            Log.Error("No permission to attach files in this channel {ChannelId}", channel.Id);
            return;
        }

        await channel.SendMessageAsync(message);
    }
}
