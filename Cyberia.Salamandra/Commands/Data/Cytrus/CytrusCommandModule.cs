using Cyberia.Api;
using Cyberia.Cytrusaurus;
using Cyberia.Salamandra.DsharpPlus;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Commands;
using DSharpPlus.Commands.ContextChecks;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;
using DSharpPlus.Commands.Processors.SlashCommands.Metadata;
using DSharpPlus.Entities;

using System.ComponentModel;
using System.Reflection.Metadata;
using System.Text;

namespace Cyberia.Salamandra.Commands.Data;

[Command("cytrus")]
public sealed class CytrusCommandModule
{
    [Command("check"), Description("[Owner] Lance un check de cytrus")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    [InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall)]
    [InteractionAllowedContexts(DiscordInteractionContextType.Guild)]
    [RequireApplicationOwner]
    public static async Task CheckExecuteAsync(SlashCommandContext ctx)
    {
        await ctx.RespondAsync("Lancement du check de Cytrus...");

        await CytrusWatcher.CheckAsync();

        await ctx.FollowupAsync("Check de Cytrus terminé");
    }

    [Command("show"), Description("Affiche les informations du cytrus actuellement en ligne")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    [InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall)]
    [InteractionAllowedContexts(DiscordInteractionContextType.Guild)]
    public static async Task ShowExecuteAsync(SlashCommandContext ctx)
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

        await ctx.RespondAsync(embed);
    }

    [Command("diff"), Description("Liste les différences entre les fichiers de deux versions d'un jeu sur Cytrus")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    [InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall)]
    [InteractionAllowedContexts(DiscordInteractionContextType.Guild)]
    public static async Task DiffExecuteAsync(SlashCommandContext ctx,
        [Parameter("game"), Description("Nom du jeu")]
        [SlashChoiceProvider<CytrusGameChoiceProvider>]
        string game,
        [Parameter("platform"), Description("Platform")]
        [SlashAutoCompleteProvider<CytrusPlatformAutoCompleteProvider>]
        string platform,
        [Parameter("old_release"), Description("Release de l'ancien client")]
        [SlashAutoCompleteProvider<CytrusReleaseAutoCompleteProvider>]
        string oldRelease,
        [Parameter("old_version"), Description("Version de l'ancien client")]
        [SlashAutoCompleteProvider<CytrusOldVersionAutocompleteProvider>]
        string oldVersion,
        [Parameter("new_release"), Description("Release du nouveau client")]
        [SlashAutoCompleteProvider<CytrusReleaseAutoCompleteProvider>]
        string newRelease,
        [Parameter("new_version"), Description("Version du nouveau client")]
        [SlashAutoCompleteProvider<CytrusNewVersionAutocompleteProvider>]
        string newVersion)
    {
        await ctx.RespondAsync(new DiscordInteractionResponseBuilder()
            .WithContent("👷")
            .AsEphemeral());

        await ctx.Channel.SendCytrusManifestDiffMessageAsync(game, platform.ToLower(), oldRelease.ToLower(), oldVersion, newRelease.ToLower(), newVersion);
    }
}
