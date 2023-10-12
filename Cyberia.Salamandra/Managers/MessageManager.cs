using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace Cyberia.Salamandra.Managers
{
    public static class MessageManager
    {
        private static readonly Dictionary<string, string> _hiddenCommands = new()
        {
            { "tppttp", "onTotProut.mp3" },
            { "fppffp", "onFranckyPassion.mp3" },
            { "wizz", "onWizz.mp3" },
            { "hncabot", "onBelge.mp3" },
            { "foot2rue", "onFoot2Rue.mp4" },
            { "monkeychan", "onMonkeyChan.mp4" }
        };

        public static async Task OnMessageCreated(DiscordClient _, MessageCreateEventArgs e)
        {
            foreach (KeyValuePair<string, string> hiddenCommand in _hiddenCommands)
            {
                if (e.Message.Content.EndsWith(hiddenCommand.Key))
                {
                    await DeleteMessage(e.Message);
                    await SendFile(e.Channel, $"{Bot.OUTPUT_PATH}/{hiddenCommand.Value}");
                }
            }
        }

        public static async Task SendMessage(this DiscordChannel channel, DiscordMessageBuilder message)
        {
            Permissions permissions = channel.Guild.CurrentMember.PermissionsIn(channel);

            if (!permissions.HasPermission(Permissions.AccessChannels))
            {
                Log.Error("No permission to access to this channel (id:{id})", channel.Id);
                return;
            }

            if (!permissions.HasPermission(Permissions.SendMessages))
            {
                Log.Error("No permission to send message in this channel (id:{id})", channel.Id);
                return;
            }

            if (message.Files.Count > 0 && !permissions.HasPermission(Permissions.AttachFiles))
            {
                Log.Error("No permission to attach files in this channel (id:{id})", channel.Id);
                return;
            }

            await channel.SendMessageAsync(message);
        }

        public static async Task SendFile(this DiscordChannel channel, string filePath)
        {
            using FileStream fileStream = File.OpenRead(filePath);

            await channel.SendMessage(new DiscordMessageBuilder().AddFile(Path.GetFileName(filePath), fileStream));
        }

        public static async Task SendLogMessage(string content)
        {
            DiscordChannel? logChannel = await GetLogChannel();

            if (logChannel is null)
            {
                Log.Error("Unknown log channel (id:{id})", Bot.Instance.Config.LogChannelId);
                return;
            }

            await logChannel.SendMessage(new DiscordMessageBuilder().WithContent(content));
        }

        public static async Task SendCommandErrorMessage(string content)
        {
            DiscordChannel? commandErrorChannel = await GetCommandErrorChannel();

            if (commandErrorChannel is null)
            {
                Log.Error("Unknown command error channel (id:{id})", Bot.Instance.Config.LogChannelId);
                return;
            }

            await commandErrorChannel.SendMessage(new DiscordMessageBuilder().WithContent(content));
        }

        public static async Task DeleteMessage(this DiscordMessage message)
        {
            DiscordChannel channel = message.Channel;
            Permissions permissions = channel.Guild.CurrentMember.PermissionsIn(channel);

            if (!permissions.HasPermission(Permissions.AccessChannels))
            {
                Log.Error("No permission to access to this channel (id:{id})", channel.Id);
                return;
            }

            if (message.Author.Id != Bot.Instance.Client.CurrentUser.Id && !permissions.HasPermission(Permissions.ManageMessages))
            {
                Log.Error("No permission to delete this message (id:{id})", message.Id);
                return;
            }

            await message.DeleteAsync();
        }

        private static async Task<DiscordChannel?> GetLogChannel()
        {
            try
            {
                return await Bot.Instance.Client.GetChannelAsync(Bot.Instance.Config.LogChannelId);
            }
            catch
            {
                Log.Error("Unknown log channel (id:{id})", Bot.Instance.Config.LogChannelId);
                return null;
            }
        }

        private static async Task<DiscordChannel?> GetCommandErrorChannel()
        {
            try
            {
                return await Bot.Instance.Client.GetChannelAsync(Bot.Instance.Config.CommandErrorChannelId);
            }
            catch
            {
                Log.Error("Unknown command error channel (id:{id})", Bot.Instance.Config.CommandErrorChannelId);
                return null;
            }
        }
    }
}
