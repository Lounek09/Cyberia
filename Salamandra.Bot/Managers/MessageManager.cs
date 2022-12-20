using DSharpPlus;
using DSharpPlus.Entities;

using Google.FlatBuffers;
using Salamandra.Cytrus.Models.FlatBuffers;
using Salamandra.Cytrus.Models;
using System.Diagnostics;

namespace Salamandra.Bot.Managers
{
    public static class MessageManager
    {
        public static async Task SendMessage(this DiscordChannel channel, DiscordMessageBuilder message, params FileStream?[] fileStreams)
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

        public static async Task SendFile(this DiscordChannel channel, string filePath)
        {
            using (FileStream fileStream = System.IO.File.OpenRead(filePath))
                await channel.SendMessage(new DiscordMessageBuilder().AddFile(Path.GetFileName(filePath), fileStream));
        }

        public static async Task SendLogMessage(string content)
        {
            DiscordChannel? logChannel = await DiscordBot.Instance.Config.GetLogChannel();

            if (logChannel is null)
            {
                DiscordBot.Instance.Logger.Error($"Unknown log channel (id:{DiscordBot.Instance.Config.LogChannelId})");
                return;
            }

            await logChannel.SendMessage(new DiscordMessageBuilder().WithContent(content));
        }

        public static async Task SendCommandErrorMessage(string content)
        {
            DiscordChannel? commandErrorChannel = await DiscordBot.Instance.Config.GetCommandErrorChannel();

            if (commandErrorChannel is null)
            {
                DiscordBot.Instance.Logger.Error("Unknown command error channel (id:" + DiscordBot.Instance.Config.LogChannelId + ")");
                return;
            }

            await commandErrorChannel.SendMessage(new DiscordMessageBuilder().WithContent(content));
        }

        public static async Task Delete(this DiscordMessage message)
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

        public static async Task SendCytrusManifestDiffMessage(this DiscordChannel channel, string game, string platform, string oldRelease, string oldVersion, string newRelease, string newVersion)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            HttpClient httpClient = new();
            DiscordMessageBuilder message = new();

            string url1 = CytrusData.GetGameManifestUrl(game, platform, oldRelease, oldVersion);
            Manifest client1;
            try
            {
                byte[] metafile = await httpClient.GetByteArrayAsync(url1);
                ByteBuffer buffer = new(metafile);
                client1 = Manifest.GetRootAsManifest(buffer);
            }
            catch (HttpRequestException)
            {
                await channel.SendMessage(message.WithContent($"Nouveau client introuvable"));
                return;
            }
            
            string url2 = CytrusData.GetGameManifestUrl(game, platform, newRelease, newVersion);
            Manifest client2;
            try
            {
                byte[] metafile = await httpClient.GetByteArrayAsync(url2);
                ByteBuffer buffer = new(metafile);
                client2 = Manifest.GetRootAsManifest(buffer);
            }
            catch (HttpRequestException)
            {
                await channel.SendMessage(message.WithContent($"Nouveau client introuvable"));
                return;
            }

            client2.DiffFiles(client1, out string outputPath);

            stopwatch.Stop();

            string mainContent = $"""
                                  Diff de {Formatter.Bold(game.Capitalize())} sur {Formatter.Bold(platform)} effectué en {stopwatch.ElapsedMilliseconds}ms
                                  {Formatter.InlineCode(oldVersion)} ({oldRelease}) ➜ {Formatter.InlineCode(newVersion)} ({newRelease})
                                  """;

            string diffContent = Formatter.BlockCode(File.ReadAllText(outputPath), "diff");

            if (mainContent.Length + diffContent.Length < 2000)
                await channel.SendMessage(message.WithContent($"{mainContent}\n{diffContent}"));
            else
            {
                using (FileStream fileStream = File.OpenRead(outputPath))
                    await channel.SendMessage(message.WithContent(mainContent), fileStream);
            }

        }
    }
}
