using Cyberia.Api;
using Cyberia.Api.Managers;
using Cyberia.Langzilla;
using Cyberia.Langzilla.Enums;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Commands;
using DSharpPlus.Commands.ContextChecks;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;
using DSharpPlus.Commands.Processors.SlashCommands.Metadata;
using DSharpPlus.Entities;

using System.ComponentModel;
using System.Diagnostics;

namespace Cyberia.Salamandra.Commands.Data.Cytrus;

[Command("langs")]
[InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall)]
[InteractionAllowedContexts(DiscordInteractionContextType.Guild)]
public sealed class LangsCommandModule
{
    [Command("check"), Description("[Owner] Launch a check to see if there is a new version of the langs")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    [RequireApplicationOwner]
    public static async Task CheckExecuteAsync(SlashCommandContext ctx,
        [Parameter("type"), Description("The type to check")]
        LangType type,
        [Parameter("language"), Description("The language to check; if empty, check all languages simultaneously")]
        LangLanguage? language = null,
        [Parameter("force"), Description("Force the check")]
        bool force = false)
    {
        var typeStr = type.ToStringFast();

        if (language is null)
        {
            await ctx.RespondAsync($"Starting the check of {Formatter.Bold(typeStr)} langs in all languages...");

            await Task.WhenAll(
                Enum.GetValues<LangLanguage>()
                    .Select(x =>
                    {
                        var repository = LangsWatcher.LangRepositories[(type, x)];
                        return LangsWatcher.CheckAsync(repository, force);
                    }));

            await ctx.EditResponseAsync($"Check of {Formatter.Bold(typeStr)} langs in all language completed.");
            return;
        }

        var languageStr = language.Value.ToStringFast();

        await ctx.RespondAsync($"Starting the check of {Formatter.Bold(typeStr)} langs in {Formatter.Bold(languageStr)}...");

        var repository = LangsWatcher.LangRepositories[(type, language.Value)];
        await LangsWatcher.CheckAsync(repository, force);

        await ctx.EditResponseAsync($"Check of {Formatter.Bold(typeStr)} langs in {Formatter.Bold(languageStr)} completed.");
    }

    [Command("diff"), Description("[Owner] List the differences between to type of langs")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    [RequireApplicationOwner]
    public static async Task DiffExecuteAsync(SlashCommandContext ctx,
        [Parameter("type"), Description("The type to diff")]
        LangType type,
        [Parameter("model_type"), Description("The type of the model langs")]
        LangType modelType,
        [Parameter("language"), Description("The language to diff; if empty, diff all language simultaneously")]
        LangLanguage? language = null)
    {
        if (ChannelManager.LangForumChannel is null)
        {
            await ctx.RespondAsync("The lang forum channel is not defined.");
            return;
        }

        await ctx.DeferResponseAsync();

        if (language is null)
        {
            await Task.WhenAll(Enum.GetValues<LangLanguage>()
                                   .Select(x => LangManager.LaunchManualDiff(type, modelType, x)));
        }
        else
        {
            await LangManager.LaunchManualDiff(type, modelType, language.Value);
        }

        await ctx.EditResponseAsync("Diff completed");
    }

    [Command("parse"), Description("[Owner] Launch the parsing of the langs into JSON")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    [RequireApplicationOwner]
    public static async Task ParseExecuteAsync(SlashCommandContext ctx,
        [Parameter("type"), Description("The type to parse")]
        LangType type,
        [Parameter("language"), Description("The language to parse; if empty, parse all language")]
        LangLanguage? language = null)
    {
        await ctx.DeferResponseAsync();

        var startTime = Stopwatch.GetTimestamp();

        var success = language is null
            ? LangParserManager.ParseAll(type)
            : LangParserManager.Parse(type, language.Value);

        var elapsedTime = Stopwatch.GetElapsedTime(startTime);

        var response = success
            ? $"Langs successfully parsed in {elapsedTime:s\\,ffff}s."
            : "An error occurred, please check the logs.";

        await ctx.EditResponseAsync(response);
    }

    [Command("show"), Description("Display the information of the currently online langs")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    public static async Task ShowExecuteAsync(SlashCommandContext ctx,
        [Parameter("type"), Description("The type to display")]
        LangType type = LangType.Official,
        [Parameter("language"), Description("The language to display")]
        LangLanguage language = LangLanguage.fr)
    {
        await ctx.RespondAsync(await new LangsMessageBuilder(type, language).GetMessageAsync<DiscordInteractionResponseBuilder>());
    }

    [Command("get"), Description("Returns the requested decompiled lang")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    public static async Task GetExecuteAsync(SlashCommandContext ctx,
        [Parameter("type"), Description("The requested type")]
        LangType type,
        [Parameter("language"), Description("The requested language")]
        LangLanguage language,
        [Parameter("name"), Description("The name of the requested lang")]
        [SlashAutoCompleteProvider<LangNameAutocompleteProvider>]
        string name)
    {
        var langRepository = LangsWatcher.LangRepositories[(type, language)];

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
