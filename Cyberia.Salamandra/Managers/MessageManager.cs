using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

using System.Collections.Frozen;
using System.Threading.Channels;

namespace Cyberia.Salamandra.Managers;

public static class MessageManager
{
    private static readonly FrozenDictionary<string, string> s_hiddenCommands = new Dictionary<string, string>()
    {
        { "tppttp", Path.Join(Bot.OutputPath, "onTotProut.mp3") },
        { "fppffp", Path.Join(Bot.OutputPath, "onFranckyPassion.mp3") },
        { "wizz", Path.Join(Bot.OutputPath, "onWizz.mp3") },
        { "hncabot", Path.Join(Bot.OutputPath, "onBelge.mp3") },
        { "foot2rue", Path.Join(Bot.OutputPath, "onFoot2Rue.mp4") },
        { "monkeychan", Path.Join(Bot.OutputPath, "onMonkeyChan.mp4") }
    }.ToFrozenDictionary();

    public static async Task OnMessageCreated(DiscordClient _, MessageCreateEventArgs args)
    {
        if (args.Author.IsBot || args.Channel.IsPrivate)
        {
            return;
        }

        var content = args.Message.Content;
        if (string.IsNullOrWhiteSpace(content))
        {
            return;
        }

        var lastSpaceIndex = content.LastIndexOf(' ');
        var lastWord = lastSpaceIndex > -1
            ? content[(lastSpaceIndex + 1)..].ToString()
            : content;

        if (s_hiddenCommands.TryGetValue(lastWord, out var filePath))
        {
            var permissions = args.Channel.Guild.CurrentMember.PermissionsIn(args.Channel);
            if (permissions.HasPermission(Permissions.ManageMessages))
            {
                await args.Message.DeleteAsync();
            }

            using var fileStream = File.OpenRead(filePath);
            await args.Channel.SendMessage(new DiscordMessageBuilder().AddFile(Path.GetFileName(filePath), fileStream));
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

    public static async Task SendLogMessage(string content)
    {
        var channel = ChannelManager.LogChannel;
        if (channel is null)
        {
            return;
        }

        await channel.SendMessage(new DiscordMessageBuilder().WithContent(content));
    }

    public static async Task SendErrorMessage(DiscordEmbed embed)
    {
        var channel = ChannelManager.ErrorChannel;
        if (channel is null)
        {
            return;
        }

        await channel.SendMessage(new DiscordMessageBuilder().AddEmbed(embed));
    }
}
