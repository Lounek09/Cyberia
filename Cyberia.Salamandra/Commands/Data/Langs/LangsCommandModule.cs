using Cyberia.Langzilla;
using Cyberia.Langzilla.Enums;
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
    private readonly CachedChannelsService _cachedChannelsService;
    private readonly EmbedBuilderService _embedBuilderService;
    private readonly LangsParser _langsParser;
    private readonly LangsWatcher _langsWatcher;
    private readonly LangsService _langsService;

    public LangsCommandModule(CachedChannelsService cachedChannelsService, EmbedBuilderService embedBuilderService, LangsParser langsParser, LangsWatcher langsWatcher, LangsService langsService)
    {
        _cachedChannelsService = cachedChannelsService;
        _embedBuilderService = embedBuilderService;
        _langsParser = langsParser;
        _langsWatcher = langsWatcher;
        _langsService = langsService;
    }

    [Command("check"), Description("[Owner] Launch a check to see if there is a new version of the langs")]
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
        var typeStr = type.ToStringFast();

        if (language is null)
        {
            await ctx.RespondAsync($"Starting the check of {Formatter.Bold(typeStr)} langs in all languages...");

            await Task.WhenAll(
                Enum.GetValues<Language>()
                    .Select(x =>
                    {
                        var repository = _langsWatcher.GetRepository(type, x);
                        return _langsWatcher.CheckAsync(repository, force);
                    }));

            await ctx.EditResponseAsync($"Check of {Formatter.Bold(typeStr)} langs in all language completed.");
            return;
        }

        var languageStr = language.Value.ToStringFast();

        await ctx.RespondAsync($"Starting the check of {Formatter.Bold(typeStr)} langs in {Formatter.Bold(languageStr)}...");

        var repository = _langsWatcher.GetRepository(type, language.Value);
        await _langsWatcher.CheckAsync(repository, force);

        await ctx.EditResponseAsync($"Check of {Formatter.Bold(typeStr)} langs in {Formatter.Bold(languageStr)} completed.");
    }

    [Command("diff"), Description("[Owner] List the differences between to type of langs")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    [RequireApplicationOwner]
    public async Task DiffExecuteAsync(SlashCommandContext ctx,
        [Parameter("type"), Description("The type to diff")]
        LangType type,
        [Parameter("model_type"), Description("The type of the model langs")]
        LangType modelType,
        [Parameter("language"), Description("The language to diff; if empty, diff all language simultaneously")]
        Language? language = null)
    {
        if (_cachedChannelsService.LangsForumChannel is null)
        {
            await ctx.RespondAsync("The lang forum channel is not defined.");
            return;
        }

        await ctx.DeferResponseAsync();

        if (language is null)
        {
            await Task.WhenAll(Enum.GetValues<Language>().Select(x => _langsService.LaunchManualDiff(type, modelType, x)));
        }
        else
        {
            await _langsService.LaunchManualDiff(type, modelType, language.Value);
        }

        await ctx.EditResponseAsync("Diff completed");
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
            : await _langsParser.ParseAsync(type, language.Value);

        var elapsedTime = Stopwatch.GetElapsedTime(startTime);

        var response = success
            ? $"Langs successfully parsed in {elapsedTime:s\\,ffff}s."
            : "An error occurred, please check the logs.";

        await ctx.EditResponseAsync(response);
    }

    [Command("show"), Description("Display the information of the currently online langs")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    public async Task ShowExecuteAsync(SlashCommandContext ctx,
        [Parameter("type"), Description("The type to display")]
        LangType type = LangType.Official,
        [Parameter("language"), Description("The language to display")]
        Language language = Language.fr)
    {
        var repository = _langsWatcher.GetRepository(type, language);

        await ctx.RespondAsync(await new LangsMessageBuilder(_embedBuilderService, repository).BuildAsync<DiscordInteractionResponseBuilder>());
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
        string name)
    {
        var langRepository = _langsWatcher.GetRepository(type, language);

        var lang = langRepository.GetByName(name);
        if (lang is null)
        {
            await ctx.RespondAsync("This lang does not exist.");
            return;
        }

        var currentDecompiledFilePath = lang.CurrentDecompiledFilePath;
        if (string.IsNullOrEmpty(currentDecompiledFilePath))
        {
            await ctx.RespondAsync("This lang has never been decompiled.");
            return;
        }

        using var fileStream = File.OpenRead(currentDecompiledFilePath);
        await ctx.RespondAsync(new DiscordInteractionResponseBuilder()
            .AddFile($"{lang.FileName}.as", fileStream));
    }
}
