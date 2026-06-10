using Cyberia.Cytrusaurus;
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
    private readonly ICytrusService _cytrusService;
    private readonly ICytrusWatcher _cytrusWatcher;

    public CytrusCommandModule(ICytrusService cytrusService, ICytrusWatcher cytrusWatcher)
    {
        _cytrusService = cytrusService;
        _cytrusWatcher = cytrusWatcher;
    }

    [Command("check"), Description("[Owner] Launch a check to see if there is a new version of Cytrus")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    [RequireApplicationOwner]
    public async Task CheckExecuteAsync(SlashCommandContext ctx)
    {
        await ctx.DeferResponseAsync();

        await _cytrusWatcher.CheckAsync();

        await ctx.EditResponseAsync("Cytrus check completed.");
    }

    [Command("diff"), Description("List the differences between two versions of a game on Cytrus")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    public async Task DiffExecuteAsync(SlashCommandContext ctx,
        [Parameter("game"), Description("Name of the game")]
        [SlashAutoCompleteProvider<CytrusGameAutoCompleteProvider>]
        string game,
        [Parameter("platform"), Description("Platform")]
        [SlashAutoCompleteProvider<CytrusPlatformAutoCompleteProvider>]
        string platform,
        [Parameter("current_release"), Description("Release of the current client to diff")]
        [SlashAutoCompleteProvider<CytrusReleaseAutoCompleteProvider>]
        string currentRelease,
        [Parameter("current_version"), Description("Version of the current client to diff")]
        [SlashAutoCompleteProvider<CytrusCurrentVersionAutocompleteProvider>]
        string currentVersion,
        [Parameter("model_release"), Description("Release of the model client")]
        [SlashAutoCompleteProvider<CytrusReleaseAutoCompleteProvider>]
        string modelRelease,
        [Parameter("model_version"), Description("Version of the model client")]
        [SlashAutoCompleteProvider<CytrusModelVersionAutocompleteProvider>]
        string modelVersion)
    {
        await ctx.DeferResponseAsync();

        var mainContent =
        $"""
        Diff of {Formatter.Bold(game.Capitalize())} on {Formatter.Bold(platform)}
        {Formatter.InlineCode(currentVersion)} ({currentRelease}) compared to {Formatter.InlineCode(modelVersion)} ({modelRelease})
        """;

        var diff = await _cytrusService.GetManifestDiffAsync(game, platform, currentRelease, currentVersion, modelRelease, modelVersion);

        if (mainContent.Length + diff.Length + 10 > Constant.MaxMessageSize) // 10 for the block code formatting
        {
            using MemoryStream stream = new(Encoding.UTF8.GetBytes(diff));

            var message = new DiscordInteractionResponseBuilder()
                .WithContent(mainContent)
                .AddFile($"{game}_{platform}_{currentRelease}_{currentVersion}_{modelRelease}_{modelVersion}.diff", stream);

            await ctx.EditResponseAsync(message);
            return;
        }

        await ctx.EditResponseAsync($"{mainContent}\n{Formatter.BlockCode(diff, "diff")}");
    }
}
