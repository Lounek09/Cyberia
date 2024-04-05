using Cyberia.Cytrusaurus;
using Cyberia.Salamandra.DsharpPlus;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;

using System.Text;

namespace Cyberia.Salamandra.Commands.Data;

[SlashCommandGroup("cytrus", "Cytrus")]
public sealed class CytrusCommandModule : ApplicationCommandModule
{
    [SlashCommand("check", "[Owner] Lance un check de cytrus")]
    [SlashRequireOwner]
    public async Task CheckCytrusCommand(InteractionContext ctx)
    {
        await ctx.CreateResponseAsync($"Lancement du check de Cytrus...");

        await CytrusWatcher.CheckAsync();

        await ctx.FollowUpAsync(new DiscordFollowupMessageBuilder().WithContent($"Check de Cytrus terminé"));
    }

    [SlashCommand("show", "Affiche les informations du cytrus actuellement en ligne")]
    public async Task ShowCytrusCommand(InteractionContext ctx)
    {
        var embed = EmbedManager.CreateEmbedBuilder(EmbedCategory.Tools, "Cytrus");

        embed.AddField("Name", CytrusWatcher.Cytrus.Name.Capitalize(), true);
        embed.AddField("Version", CytrusWatcher.Cytrus.Version.ToString(), true);
        embed.AddEmptyField(true);

        foreach (var game in CytrusWatcher.Cytrus.Games.OrderBy(x => x.Value.Order))
        {
            StringBuilder fieldContentBuilder = new();

            foreach (var platform in game.Value.Platforms)
            {
                fieldContentBuilder.Append(Formatter.Underline($"{platform.Key.Capitalize()} :"));
                fieldContentBuilder.Append('\n');

                foreach (var release in game.Value.GetReleasesByPlatformName(platform.Key))
                {
                    fieldContentBuilder.Append("- ");
                    fieldContentBuilder.Append(release.Key.Capitalize());
                    fieldContentBuilder.Append(" : ");
                    fieldContentBuilder.Append(Formatter.InlineCode(release.Value));
                    fieldContentBuilder.Append('\n');
                }
            }

            embed.AddField($"{game.Value.Name.Capitalize()} ({game.Value.GameId})", fieldContentBuilder.ToString());
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
