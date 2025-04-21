using Cyberia.Salamandra.Services;

using DSharpPlus;
using DSharpPlus.Commands;
using DSharpPlus.Commands.ContextChecks;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;
using DSharpPlus.Commands.Processors.SlashCommands.Metadata;
using DSharpPlus.Entities;

using System.ComponentModel;

namespace Cyberia.Salamandra.Commands.Admin.Emoji;

[Command("emoji")]
[InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall)]
[InteractionAllowedContexts(DiscordInteractionContextType.Guild)]
public sealed class EmojiCommandModule
{
    private readonly EmojisService _emojisService;

    public EmojiCommandModule(EmojisService emojisService)
    {
        _emojisService = emojisService;
    }

    [Command("create"), Description("[Owner] Create the emojis")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    [RequireApplicationOwner]
    public async Task UpdateExecuteAsync(SlashCommandContext ctx)
    {
        await ctx.DeferResponseAsync();

        await _emojisService.CreateEmojisAsync();

        await ctx.RespondAsync("Emojis created.");
    }

    [Command("delete"), Description("[Owner] Delete an emoji by its name")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    [RequireApplicationOwner]
    public async Task DeleteExecuteAsync(SlashCommandContext ctx,
        [Parameter("name"), Description("The name of the emoji")]
        [SlashAutoCompleteProvider<EmojiAutocompleteProvider>]
        string name)
    {
        var success = await _emojisService.DeleteEmojiAsync(name);

        await ctx.RespondAsync(success ? $"Emoji {Formatter.Bold(name)} deleted." : $"Emoji {Formatter.Bold(name)} not found.");
    }
}

