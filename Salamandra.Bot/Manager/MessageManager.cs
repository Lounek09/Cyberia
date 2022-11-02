using DSharpPlus;
using DSharpPlus.Entities;

namespace Salamandra.Bot.Manager
{
    public static class MessageManager
    {
        public static async Task SendMessage(DiscordChannel channel, DiscordMessageBuilder message, params FileStream?[] fileStreams)
        {
            Permissions permissions = channel.Guild.CurrentMember.PermissionsIn(channel);

            if (!permissions.HasPermission(Permissions.AccessChannels))
            {
                DiscordBot.Instance.Logger.Error($"No permission to access to this channel (id:{channel.Id})");
                return;
            }

            if (!permissions.HasPermission(Permissions.SendMessages))
            {
                DiscordBot.Instance.Logger.Error($"No permission to send message in this channel (id:{channel.Id})");
                return;
            }

            if (message.Files.Count > 0 && !permissions.HasPermission(Permissions.AttachFiles))
            {
                DiscordBot.Instance.Logger.Error($"No permission to attach files in this channel (id:{channel.Id})");
                return;
            }

            await channel.SendMessageAsync(message);

            foreach (Stream? fileStream in fileStreams)
                fileStream?.Dispose();
        }

        public static async Task SendFile(DiscordChannel channel, string filePath)
        {
            using (FileStream fileStream = File.OpenRead(filePath))
                await SendMessage(channel, new DiscordMessageBuilder().WithFile(Path.GetFileName(filePath), fileStream));
        }

        public static async Task SendLogMessage(string content)
        {
            DiscordChannel? logChannel = await DiscordBot.Instance.Config.GetLogChannel();

            if (logChannel is null)
            {
                DiscordBot.Instance.Logger.Error($"Unknown log channel (id:{DiscordBot.Instance.Config.LogChannelId})");
                return;
            }

            await SendMessage(logChannel, new DiscordMessageBuilder().WithContent(content));
        }

        public static async Task SendCommandErrorMessage(string content)
        {
            DiscordChannel? commandErrorChannel = await DiscordBot.Instance.Config.GetCommandErrorChannel();

            if (commandErrorChannel is null)
            {
                DiscordBot.Instance.Logger.Error("Unknown command error channel (id:" + DiscordBot.Instance.Config.LogChannelId + ")");
                return;
            }

            await SendMessage(commandErrorChannel, new DiscordMessageBuilder().WithContent(content));
        }

        public static async Task DeleteMessage(DiscordMessage message)
        {
            DiscordChannel channel = message.Channel;
            Permissions permissions = channel.Guild.CurrentMember.PermissionsIn(channel);

            if (!permissions.HasPermission(Permissions.AccessChannels))
            {
                DiscordBot.Instance.Logger.Error($"No permission to access to this channel (id:{channel.Id})");
                return;
            }

            if (message.Author.Id != DiscordBot.Instance.Client.CurrentUser.Id && !permissions.HasPermission(Permissions.ManageMessages))
            {
                DiscordBot.Instance.Logger.Error($"No permission to delete this message (id:{message.Id})");
                return;
            }

            await message.DeleteAsync();
        }
    }
}
