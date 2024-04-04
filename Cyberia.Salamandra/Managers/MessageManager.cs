using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

using System.Collections.Frozen;

namespace Cyberia.Salamandra.Managers;

public static class MessageManager
{
    private static readonly FrozenDictionary<string, string> _hiddenCommands = new Dictionary<string, string>()
    {
        { "tppttp", Path.Join(Bot.OUTPUT_PATH, "onTotProut.mp3") },
        { "fppffp", Path.Join(Bot.OUTPUT_PATH, "onFranckyPassion.mp3") },
        { "wizz", Path.Join(Bot.OUTPUT_PATH, "onWizz.mp3") },
        { "hncabot", Path.Join(Bot.OUTPUT_PATH, "onBelge.mp3") },
        { "foot2rue", Path.Join(Bot.OUTPUT_PATH, "onFoot2Rue.mp4") },
        { "monkeychan", Path.Join(Bot.OUTPUT_PATH, "onMonkeyChan.mp4") }
    }.ToFrozenDictionary();

    public static async Task OnMessageCreated(DiscordClient _, MessageCreateEventArgs e)
    {
        var content = e.Message.Content;

        if (!string.IsNullOrEmpty(content))
        {
            var lastSpaceIndex = content.LastIndexOf(' ');
            var lastWord = lastSpaceIndex > -1
                ? content[(lastSpaceIndex + 1)..].ToString()
                : content;

            if (_hiddenCommands.TryGetValue(lastWord, out var filePath))
            {
                await DeleteMessage(e.Message);
                await SendFile(e.Channel, filePath);
            }
        }
    }

    public static async Task SendMessage(this DiscordChannel channel, DiscordMessageBuilder message)
    {
        var permissions = channel.Guild.CurrentMember.PermissionsIn(channel);

        if (!permissions.HasPermission(Permissions.AccessChannels))
        {
            Log.Error("No permission to access to this channel {ChannelId}", channel.Id);
            return;
        }

        if (!permissions.HasPermission(Permissions.SendMessages))
        {
            Log.Error("No permission to send message in this channel {ChannelId}", channel.Id);
            return;
        }

        if (message.Files.Count > 0 && !permissions.HasPermission(Permissions.AttachFiles))
        {
            Log.Error("No permission to attach files in this channel {ChannelId}", channel.Id);
            return;
        }

        await channel.SendMessageAsync(message);
    }

    public static async Task SendFile(this DiscordChannel channel, string filePath)
    {
        using var fileStream = File.OpenRead(filePath);

        await channel.SendMessage(new DiscordMessageBuilder().AddFile(Path.GetFileName(filePath), fileStream));
    }

    public static async Task SendLogMessage(string content)
    {
        var logChannel = await GetLogChannel();

        if (logChannel is null)
        {
            Log.Error("Unknown log channel {ChannelId}", Bot.Config.LogChannelId);
            return;
        }

        await logChannel.SendMessage(new DiscordMessageBuilder().WithContent(content));
    }

    public static async Task SendCommandErrorMessage(DiscordEmbed embed)
    {
        var commandErrorChannel = await GetCommandErrorChannel();

        if (commandErrorChannel is null)
        {
            Log.Error("Unknown command error channel {ChannelId}", Bot.Config.LogChannelId);
            return;
        }

        await commandErrorChannel.SendMessage(new DiscordMessageBuilder().AddEmbed(embed));
    }

    public static async Task DeleteMessage(this DiscordMessage message)
    {
        var channel = message.Channel;
        if (channel is null)
        {
            Log.Error("Unknown channel for this message {MessageId}", message.Id);
            return;
        }

        var permissions = channel.Guild.CurrentMember.PermissionsIn(channel);

        if (!permissions.HasPermission(Permissions.AccessChannels))
        {
            Log.Error("No permission to access to this channel {ChannelId}", channel.Id);
            return;
        }

        if ((message.Author is not null && message.Author.Id == Bot.Client.CurrentUser.Id) ||
            permissions.HasPermission(Permissions.ManageMessages))
        {
            await message.DeleteAsync();
            return;
        }

        Log.Error("No permission to delete this message {MessageId}", message.Id);
    }

    private static async Task<DiscordChannel?> GetLogChannel()
    {
        try
        {
            return await Bot.Client.GetChannelAsync(Bot.Config.LogChannelId);
        }
        catch
        {
            Log.Error("Unknown log channel ({ChannelId})", Bot.Config.LogChannelId);
            return null;
        }
    }

    private static async Task<DiscordChannel?> GetCommandErrorChannel()
    {
        try
        {
            return await Bot.Client.GetChannelAsync(Bot.Config.CommandErrorChannelId);
        }
        catch
        {
            Log.Error("Unknown command error channel ({ChannelId})", Bot.Config.CommandErrorChannelId);
            return null;
        }
    }
}
