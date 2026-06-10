using Cyberia.Database.Repositories;
using Cyberia.Langzilla;
using Cyberia.Langzilla.Extensions;
using Cyberia.Langzilla.Primitives;
using Cyberia.Salamandra.Services;

using DSharpPlus;
using DSharpPlus.Commands;
using DSharpPlus.Commands.ContextChecks;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;
using DSharpPlus.Commands.Processors.SlashCommands.Metadata;
using DSharpPlus.Entities;

using System.ComponentModel;
using System.Diagnostics;

namespace Cyberia.Salamandra.Commands.Data.Langs;

[Command("langs")]
[InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall)]
[InteractionAllowedContexts(DiscordInteractionContextType.Guild)]
public sealed class LangsCommandModule
{
    private readonly ICachedChannelsManager _cachedChannelsManager;
    private readonly ILangsParser _langsParser;
    private readonly ILangRepository _langRepository;
    private readonly ILangsService _langsService;
    private readonly ILangsWatcher _langsWatcher;

    public LangsCommandModule(
        ICachedChannelsManager cachedChannelsManager,
        ILangsParser langsParser,
        ILangRepository langRepository,
        ILangsService langsService,
        ILangsWatcher langsWatcher)
    {
        _cachedChannelsManager = cachedChannelsManager;
        _langsParser = langsParser;
        _langRepository = langRepository;
        _langsService = langsService;
        _langsWatcher = langsWatcher;
    }

    [Command("check"), Description("[Owner] Launch a check to see if there is a new version of Langs")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    [RequireApplicationOwner]
    public async Task CheckExecuteAsync(SlashCommandContext ctx,
        [Parameter("type"), Description("The type to check")]
        LangType type,
        [Parameter("language"), Description("The language to check; if empty, check all languages simultaneously")]
        Language? language = null,
        [Parameter("force"), Description("Force the check")]
        bool force = false)
    {
        await ctx.DeferResponseAsync();

        if (language is null)
        {
            await Task.WhenAll(Enum.GetValues<Language>().Select(x => _langsWatcher.CheckAsync(new LangsIdentifier(type, x), force)));

            await ctx.EditResponseAsync($"Check of {type.ToStringFast()} langs in all language completed.");
            return;
        }

        await _langsWatcher.CheckAsync(new(type, language.Value), force);

        await ctx.EditResponseAsync($"Check of {Formatter.Bold(type.ToStringFast())} langs in {Formatter.Bold(language.Value.ToStringFast())} completed.");
    }

    [Command("diff"), Description("[Owner] List the differences between to type of langs")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    [RequireApplicationOwner]
    public async Task DiffExecuteAsync(SlashCommandContext ctx,
        [Parameter("type"), Description("The type of the langs to diff")]
        LangType type,
        [Parameter("model_type"), Description("The type of the model langs")]
        LangType modelType,
        [Parameter("language"), Description("The language to diff; if empty, diff all language simultaneously")]
        Language? language = null)
    {
        if (_cachedChannelsManager.LangsForumChannel is null)
        {
            await ctx.RespondAsync("The lang forum channel is not define or does not exist.");
            return;
        }

        await ctx.DeferResponseAsync();

        if (language is null)
        {
            await Task.WhenAll(Enum.GetValues<Language>().Select(x => _langsService.LaunchManualDiff(type, modelType, x)));

            await ctx.EditResponseAsync($"Diff of {Formatter.Bold(type.ToStringFast())} compared to {Formatter.Bold(modelType.ToStringFast())}" +
                $" in all language completed.");
            return;
        }

        await _langsService.LaunchManualDiff(type, modelType, language.Value);

        await ctx.EditResponseAsync($"Diff of {Formatter.Bold(type.ToStringFast())} compared to {Formatter.Bold(modelType.ToStringFast())}" +
            $" in {Formatter.Bold(language.Value.ToStringFast())} completed.");
    }

    [Command("parse"), Description("[Owner] Launch the parsing of the langs into JSON")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    [RequireApplicationOwner]
    public async Task ParseExecuteAsync(SlashCommandContext ctx,
        [Parameter("type"), Description("The type to parse")]
        LangType type,
        [Parameter("language"), Description("The language to parse; if empty, parse all language")]
        Language? language = null)
    {
        await ctx.DeferResponseAsync();

        var startTime = Stopwatch.GetTimestamp();

        var success = language is null
            ? await _langsParser.ParseAllAsync(type)
            : await _langsParser.ParseAsync(new LangsIdentifier(type, language.Value));

        var elapsedTime = Stopwatch.GetElapsedTime(startTime);

        await ctx.EditResponseAsync(success
            ? $"Langs successfully parsed in {Formatter.Bold(elapsedTime.ToString("s\\,ffff"))}s."
            : "An error occurred, check the logs for more information.");
    }

    [Command("get"), Description("Returns the requested decompiled lang")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    public async Task GetExecuteAsync(SlashCommandContext ctx,
        [Parameter("type"), Description("The requested type")]
        LangType type,
        [Parameter("language"), Description("The requested language")]
        Language language,
        [Parameter("name"), Description("The name of the requested lang")]
        [SlashAutoCompleteProvider<LangNameAutocompleteProvider>]
        int id)
    {
        await ctx.DeferResponseAsync();

        var lang = _langRepository.Get(id);
        if (lang is null || (lang.Type != type && lang.Language != language))
        {
            await ctx.EditResponseAsync($"This {Formatter.Bold(type.ToStringFast())} lang in {Formatter.Bold(language.ToStringFast())} does not exist.");
            return;
        }

        var decompiledFilePath = lang.GetDecompiledFilePath();
        if (!File.Exists(decompiledFilePath))
        {
            await ctx.EditResponseAsync($"This {Formatter.Bold(type.ToStringFast())} lang in {Formatter.Bold(language.ToStringFast())} has never been decompiled.");
            return;
        }

        using var fileStream = File.OpenRead(decompiledFilePath);
        await ctx.EditResponseAsync(new DiscordInteractionResponseBuilder().AddFile($"{lang.GetFileName}.as", fileStream));
    }
}
