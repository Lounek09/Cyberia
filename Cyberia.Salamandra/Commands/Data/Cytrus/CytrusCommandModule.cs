using Cyberia.Cytrusaurus;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Services;

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

namespace Cyberia.Salamandra.Commands.Data.Cytrus;

[Command("cytrus")]
[InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall)]
[InteractionAllowedContexts(DiscordInteractionContextType.Guild)]
public sealed class CytrusCommandModule
{
    private readonly ICultureService _cultureService;
    private readonly ICytrusService _cytrusService;
    private readonly ICytrusWatcher _cytrusWatcher;
    private readonly IEmbedBuilderService _embedBuilderService;

    public CytrusCommandModule(ICultureService cultureService, ICytrusService cytrusService, ICytrusWatcher cytrusWatcher, IEmbedBuilderService embedBuilderService)
    {
        _cultureService = cultureService;
        _cytrusService = cytrusService;
        _cytrusWatcher = cytrusWatcher;
        _embedBuilderService = embedBuilderService;
    }

    [Command("check"), Description("[Owner] Launch a check to see if there is a new version of Cytrus")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    [RequireApplicationOwner]
    public async Task CheckExecuteAsync(SlashCommandContext ctx)
    {
        await ctx.RespondAsync("Starting the check of Cytrus...");

        await _cytrusWatcher.CheckAsync();

        await ctx.EditResponseAsync("Cytrus check completed");
    }

    [Command("show"), Description("Display the information of the currently online Cytrus")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    public async Task ShowExecuteAsync(SlashCommandContext ctx)
    {
        var culture = await _cultureService.GetCultureAsync(ctx.Interaction);

        var embed = _embedBuilderService.CreateEmbedBuilder(EmbedCategory.Tools, "Cytrus", culture)
            .WithDescription(CytrusWatcher.CytrusUrl)
            .AddField("Name", _cytrusWatcher.Cytrus.Name.Capitalize(), true)
            .AddField("Version", _cytrusWatcher.Cytrus.Version.ToString(), true)
            .AddField("Last Modified", _cytrusWatcher.LastModified.ToLocalTime().ToString("g", culture), true);

        foreach (var game in _cytrusWatcher.Cytrus.Games.OrderBy(x => x.Value.Order))
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

    [Command("diff"), Description("List the differences between the files of two versions of a game on Cytrus")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    public async Task DiffExecuteAsync(SlashCommandContext ctx,
        [Parameter("game"), Description("Name of the game")]
        [SlashAutoCompleteProvider<CytrusGameAutoCompleteProvider>]
        string game,
        [Parameter("platform"), Description("Platform")]
        [SlashAutoCompleteProvider<CytrusPlatformAutoCompleteProvider>]
        string platform,
        [Parameter("old_release"), Description("Release of the old client")]
        [SlashAutoCompleteProvider<CytrusReleaseAutoCompleteProvider>]
        string oldRelease,
        [Parameter("old_version"), Description("Version of the old client")]
        [SlashAutoCompleteProvider<CytrusOldVersionAutocompleteProvider>]
        string oldVersion,
        [Parameter("new_release"), Description("Release of the new client")]
        [SlashAutoCompleteProvider<CytrusReleaseAutoCompleteProvider>]
        string newRelease,
        [Parameter("new_version"), Description("Version of the new client")]
        [SlashAutoCompleteProvider<CytrusNewVersionAutocompleteProvider>]
        string newVersion)
    {
        await ctx.DeferResponseAsync();

        var mainContent = $"""
                     Diff of {Formatter.Bold(game.Capitalize())} on {Formatter.Bold(platform)}
                     {Formatter.InlineCode(oldVersion)} ({oldRelease}) ➜ {Formatter.InlineCode(newVersion)} ({newRelease})
                     """;

        var diff = await _cytrusService.GetManifestDiffAsync(game, platform, oldRelease, oldVersion, newRelease, newVersion);

        if (mainContent.Length + diff.Length + 10 > Constant.MaxMessageSize) // 10 for the block code formatting
        {
            using MemoryStream stream = new(Encoding.UTF8.GetBytes(diff));

            var message = new DiscordInteractionResponseBuilder()
                .WithContent(mainContent)
                .AddFile($"{game}_{platform}_{oldRelease}_{oldVersion}_{newRelease}_{newVersion}.diff", stream);

            await ctx.EditResponseAsync(message);
            return;
        }

        await ctx.EditResponseAsync($"{mainContent}\n{Formatter.BlockCode(diff, "diff")}");
    }
}
