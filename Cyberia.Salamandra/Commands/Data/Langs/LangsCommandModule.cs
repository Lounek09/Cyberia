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
    private readonly ICachedChannelsManager _cachedChannelsManager;
    private readonly ICultureService _cultureService;
    private readonly IEmbedBuilderService _embedBuilderService;
    private readonly ILangsParser _langsParser;
    private readonly ILangsService _langsService;
    private readonly ILangsWatcher _langsWatcher;

    public LangsCommandModule(ICachedChannelsManager cachedChannelsManager, ICultureService cultureService, IEmbedBuilderService embedBuilderService, ILangsParser langsParser, ILangsService langsService, ILangsWatcher langsWatcher)
    {
        _cachedChannelsManager = cachedChannelsManager;
        _cultureService = cultureService;
        _embedBuilderService = embedBuilderService;
        _langsParser = langsParser;
        _langsService = langsService;
        _langsWatcher = langsWatcher;
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
                        LangsIdentifier identifier = new(type, x);
                        var repository = _langsWatcher.GetRepository(identifier);

                        return _langsWatcher.CheckAsync(repository, force);
                    }));

            await ctx.EditResponseAsync($"Check of {Formatter.Bold(typeStr)} langs in all language completed.");
            return;
        }

        var languageStr = language.Value.ToStringFast();

        await ctx.RespondAsync($"Starting the check of {Formatter.Bold(typeStr)} langs in {Formatter.Bold(languageStr)}...");

        LangsIdentifier identifier = new(type, language.Value);
        var repository = _langsWatcher.GetRepository(identifier);

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
        if (_cachedChannelsManager.LangsForumChannel is null)
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
            : await _langsParser.ParseAsync(new LangsIdentifier(type, language.Value));

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
        var culture = await _cultureService.GetCultureAsync(ctx.Interaction);

        LangsIdentifier identifier = new(type, language);
        var repository = _langsWatcher.GetRepository(identifier);

        await ctx.RespondAsync(await new LangsMessageBuilder(_embedBuilderService, repository, culture)
            .BuildAsync<DiscordInteractionResponseBuilder>());
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
        LangsIdentifier identifier = new(type, language);
        var langRepository = _langsWatcher.GetRepository(identifier);

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

        if (!File.Exists(currentDecompiledFilePath))
        {
            await ctx.RespondAsync("The decompiled file of this lang is missing.");
            return;
        }

        using var fileStream = File.OpenRead(currentDecompiledFilePath);
        await ctx.RespondAsync(new DiscordInteractionResponseBuilder()
            .AddFile($"{lang.FileName}.as", fileStream));
    }
}
