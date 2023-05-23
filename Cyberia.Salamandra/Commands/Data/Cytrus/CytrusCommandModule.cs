using Cyberia.Cytrusaurus.Models;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;

namespace Cyberia.Salamandra.Commands.Data
{
#pragma warning disable CA1822 // Mark members as static
    [SlashCommandGroup("cytrus", "Cytrus")]
    public sealed class CytrusCommandModule : ApplicationCommandModule
    {
        [SlashCommand("check", "[Owner] Lance un check de cytrus")]
        [SlashRequireOwner]
        public async Task CheckCytrusCommand(InteractionContext ctx)
        {
            await ctx.CreateResponseAsync($"Lancement du check de Cytrus...");
            await Bot.Instance.AnkamaCytrus.LaunchAsync();
            await ctx.FollowUpAsync(new DiscordFollowupMessageBuilder().WithContent($"Check de Cytrus terminé"));
        }

        [SlashCommand("show", "Affiche les informations du cytrus actuellement en ligne")]
        public async Task ShowCytrusCommand(InteractionContext ctx)
        {
            CytrusData cytrusData = Bot.Instance.AnkamaCytrus.CytrusData;

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
            [Option("platform", "Platform", true)]
            [Autocomplete(typeof(CytrusPlatformAutocompleteProvider))]
            string platform,
            [Option("old_release", "Release de l'ancien client", true)]
            [Autocomplete(typeof(CytrusReleaseAutocompleteProvider))]
            string oldRelease,
            [Option("old_version", "Version de l'ancien client", true)]
            [Autocomplete(typeof(CytrusOldVersionAutocompleteProvider))]
            string oldVersion,
            [Option("new_release", "Release du nouveau client", true)]
            [Autocomplete(typeof(CytrusReleaseAutocompleteProvider))]
            string newRelease,
            [Option("new_version", "Version du nouveau client", true)]
            [Autocomplete(typeof(CytrusNewVersionAutocompleteProvider))]
            string newVersion)
        {
            await ctx.CreateResponseAsync("👷", true);

            await ctx.Channel.SendCytrusManifestDiffMessageAsync(game, platform.ToLower(), oldRelease.ToLower(), oldVersion, newRelease.ToLower(), newVersion);
        }
    }
}
