using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

using Salamandra.Bot.Managers;
using Salamandra.Cytrus.Models;

namespace Salamandra.Bot.Commands.Data
{
    [SlashCommandGroup("cytrus", "Cytrus")]
    public sealed class CytrusCommandModule : ApplicationCommandModule
    {
        [SlashCommand("check", "Lance un check de cytrus")]
        public async Task CheckCytrusCommand(InteractionContext ctx)
        {
            await ctx.CreateResponseAsync($"Lancement du check de Cytrus...");
            await DiscordBot.Instance.Cytrus.Launch();
            await ctx.FollowUpAsync(new DiscordFollowupMessageBuilder().WithContent($"Check de Cytrus terminé"));
        }

        [SlashCommand("show", "Affiche les informations du cytrus actuellement en ligne")]
        public async Task ShowCytrusCommand(InteractionContext ctx)
        {
            CytrusData cytrusData = DiscordBot.Instance.Cytrus.CytrusData;

            DiscordEmbedBuilder embed = EmbedManager.BuildDofusEmbed(DofusEmbedCategory.Tools, "Cytrus");

            embed.AddField("Name", cytrusData.Name.Capitalize(), true);
            embed.AddField("Version", cytrusData.Version.ToString(), true);
            embed.AddField(Constant.ZERO_WIDTH_SPACE, Constant.ZERO_WIDTH_SPACE, true);

            foreach (KeyValuePair<string, Game> game in cytrusData.Games.OrderBy(x => x.Value.Order))
            {
                List<string> content = new();

                foreach (KeyValuePair<string, Dictionary<string, string>> platform in game.Value.Platforms)
                {
                    content.Add(Formatter.Underline($"{platform.Key.Capitalize()} :"));

                    foreach (KeyValuePair<string, string> release in game.Value.GetReleasesFromPlatform(platform.Key))
                        content.Add($" - {release.Key.Capitalize()} : {Formatter.InlineCode(release.Value)}");
                }

                embed.AddField($"{game.Value.Name.Capitalize()} ({game.Value.GameId})", string.Join("\n", content));
            }

            await ctx.CreateResponseAsync(embed);
        }

        [SlashCommand("diff", "Liste les différences entre les fichiers de deux versions d'un jeu sur Cytrus")]
        public async Task DiffCytrusCommand(InteractionContext ctx,
            [Option("game", "Nom du jeu")]
            [ChoiceProvider(typeof(CytrusGameChoiceProvider))]
            string game,
            [Option("platform", "Platform")]
            [Autocomplete(typeof(CytrusPlatformAutocompleteProvider))]
            string platform,
            [Option("old_release", "Release de l'ancien client\"")]
            [Autocomplete(typeof(CytrusReleaseAutocompleteProvider))]
            string oldRelease,
            [Option("old_version", "Version de l'ancien client")]
            [Autocomplete(typeof(CytrusOldVersionAutocompleteProvider))]
            string oldVersion,
            [Option("new_release", "Release du nouveau client")]
            [Autocomplete(typeof(CytrusReleaseAutocompleteProvider))]
            string newRelease,
            [Option("new_version", "Version du nouveau client")]
            [Autocomplete(typeof(CytrusNewVersionAutocompleteProvider))]
            string newVersion)
        {
            await ctx.CreateResponseAsync("👷", true);

            await ctx.Channel.SendCytrusManifestDiffMessage(game, platform.ToLower(), oldRelease.ToLower(), oldVersion, newRelease.ToLower(), newVersion);
        }
    }
}
