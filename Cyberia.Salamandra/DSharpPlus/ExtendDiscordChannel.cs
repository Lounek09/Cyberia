using DSharpPlus.Entities;

namespace Cyberia.Salamandra.DSharpPlus;

/// <summary>
/// Provides extension methods for <see cref="DiscordChannel"/>.
/// </summary>
public static class ExtendDiscordChannel
{
    /// <summary>
    /// Sends a message to the channel if the bot has the necessary permissions.
    /// </summary>
    /// <param name="channel">The channel to send the message to.</param>
    /// <param name="message">The message to send.</param>
    public static async Task SendMessageSafeAsync(this DiscordChannel channel, DiscordMessageBuilder message)
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
