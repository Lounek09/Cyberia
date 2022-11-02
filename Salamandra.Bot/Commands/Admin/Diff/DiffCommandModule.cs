using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;

using System.Text.Encodings.Web;

namespace Salamandra.Bot.Commands.Admin
{
    public sealed class DiffCommandModule : ApplicationCommandModule
    {
        [SlashCommand("Diff", "Permet de diff deux versions clients du launcher")]
        [SlashRequirePermissions(Permissions.SendMessages)]
        public async Task Command(InteractionContext ctx,
            [Option("jeu", "Nom du jeu sur cytrus")]
            string game,
            [Option("platform", "Platform de la release")]
            [Choice("windows", "windows")]
            [Choice("darwin", "darwin")]
            [Choice("linux", "linux")]
            string platform,
            [Option("release_1", "Type de release du client 1")]
            [Choice("main", "main")]
            [Choice("beta", "beta")]
            string release1,
            [Option("version_1", "Version du client 1")]
            string version1,
            [Option("release_2", "Type de release du client 2")]
            [Choice("main", "main")]
            [Choice("beta", "beta")]
            string release2,
            [Option("version_2", "Version du client 2")]
            string version2)
        {
            await ctx.CreateResponseAsync("👷", true);

            HttpClient httpClient = new();


            //Client 1
            string url1 = Constant.GetClientJsonUrl(game, release1, platform, version1);
            JsonNode? client1;
            try
            {
                client1 = JsonNode.Parse(await httpClient.GetStringAsync(url1)); ;
            }
            catch (HttpRequestException)
            {
                await ctx.FollowUpAsync(new DiscordFollowupMessageBuilder().WithContent("Client 1 introuvable !"));
                return;
            }
            if (client1 is null)
            {
                await ctx.FollowUpAsync(new DiscordFollowupMessageBuilder().WithContent("Erreur lors de la désérialization du client 1"));
                return;
            }
            client1.RemoveFieldsRecursively("packs");

            //Client 2
            string url2 = Constant.GetClientJsonUrl(game, release2, platform, version2);
            JsonNode? client2;
            try
            {
                client2 = JsonNode.Parse(await httpClient.GetStringAsync(url2)); ;
            }
            catch (HttpRequestException)
            {
                await ctx.FollowUpAsync(new DiscordFollowupMessageBuilder().WithContent("Client 2 introuvable !"));
                return;
            }
            if (client2 is null)
            {
                await ctx.FollowUpAsync(new DiscordFollowupMessageBuilder().WithContent("Erreur lors de la désérialization du client 2"));
                return;
            }
            client2.RemoveFieldsRecursively("packs");

            //Diff
            JsonSerializerOptions options = new()
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            };
            string diff = client2.Diff(client1).ToJsonString(options);
            if (diff.Equals("{}"))
            {
                await ctx.FollowUpAsync(new DiscordFollowupMessageBuilder().WithContent("Aucune différence détectée !"));
                return;
            }

            string diffFilePath = $"{Constant.TEMP_PATH}/client_diff.json";

            File.WriteAllText(diffFilePath, diff);

            using (FileStream fileStream = File.OpenRead(diffFilePath))
            {
                DiscordFollowupMessageBuilder message = new DiscordFollowupMessageBuilder()
                                                        .WithContent($"- <{url1}>\n- <{url2}>")
                                                        .AddFile(Path.GetFileName(diffFilePath), fileStream);

                await ctx.FollowUpAsync(message);
            }
        }
    }
}
