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
    [Command("check"), Description("[Owner] Lance un check des langs, si aucun language n'est précisé lance pour toutes les langues")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    [RequireApplicationOwner]
    public static async Task CheckExecuteAsync(SlashCommandContext ctx,
        [Parameter("type"), Description("Type des langs à check")]
        LangType type,
        [Parameter("langue"), Description("Language des langs à check, si vide lance pour toutes les langues")]
        LangLanguage? language = null,
        [Parameter("force"), Description("Force le check")]
        bool force = false)
    {
        DiscordFollowupMessageBuilder message = new();

        if (language is null)
        {
            await ctx.RespondAsync($"Lancement du check des langs {Formatter.Bold(type.ToString())} dans toutes les langues...");

            await Task.WhenAll(
                Enum.GetValues<LangLanguage>()
                    .Select(x =>
                    {
                        var repository = LangsWatcher.LangRepositories[(type, x)];
                        return LangsWatcher.CheckAsync(repository, force);
                    }));

            await ctx.FollowupAsync(message.WithContent($"Check des langs {Formatter.Bold(type.ToString())} dans toutes les langues terminé"));
            return;
        }

        await ctx.RespondAsync($"Lancement du check des langs {Formatter.Bold(type.ToString())} en {Formatter.Bold(language.Value.ToString())}...");

        var repository = LangsWatcher.LangRepositories[(type, language.Value)];
        await LangsWatcher.CheckAsync(repository, force);

        await ctx.FollowupAsync(message.WithContent($"Check des langs {Formatter.Bold(type.ToString())} en {Formatter.Bold(language.Value.ToString())} terminé"));
    }

    [Command("diff"), Description("[Owner] Lance un diff des langs entre différents types")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    [RequireApplicationOwner]
    public static async Task DiffExecuteAsync(SlashCommandContext ctx,
        [Parameter("type"), Description("Type des langs à diff")]
        LangType type,
        [Parameter("type_model"), Description("Type des langs models")]
        LangType modelType,
        [Parameter("langue"), Description("Language des langs à diff, si vide lance pour toutes les langues")]
        LangLanguage? language = null)
    {
        if (ChannelManager.LangForumChannel is null)
        {
            await ctx.RespondAsync("Le channel des langs n'est pas défini");
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

        await ctx.EditResponseAsync("Done");
    }

    [Command("parse"), Description("[Owner] Lance le parsing des langs en json")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    [RequireApplicationOwner]
    public static async Task ParseExecuteAsync(SlashCommandContext ctx,
        [Parameter("type"), Description("Type des langs à parse")]
        LangType type,
        [Parameter("langue"), Description("Language des langs à parse, si vide lance pour toutes les langues")]
        LangLanguage? language = null)
    {
        await ctx.DeferResponseAsync();

        var startTime = Stopwatch.GetTimestamp();

        var success = language is null
            ? LangParserManager.ParseAll(type)
            : LangParserManager.Parse(type, language.Value);

        var elapsedTime = Stopwatch.GetElapsedTime(startTime);

        var content = success
            ? $"Les langs ont été parsées avec succès en {elapsedTime:s\\,ffff}s."
            : "Une erreur est survenue, veuillez consulter les logs.";

        await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent(content));
    }

    [Command("show"), Description("Affiche les informations des langs actuellement en ligne")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    public static async Task ShowExecuteAsync(SlashCommandContext ctx,
        [Parameter("type"), Description("Type des langs à afficher")]
        LangType type = LangType.Official,
        [Parameter("langue"), Description("Language des langs à afficher")]
        LangLanguage language = LangLanguage.FR)
    {
        await ctx.RespondAsync(await new LangsMessageBuilder(type, language)
            .GetMessageAsync<DiscordInteractionResponseBuilder>());
    }

    [Command("get"), Description("Retourne le lang demandé décompilé")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    public static async Task GetExecuteAsync(SlashCommandContext ctx,
        [Parameter("type"), Description("Type du lang voulu")]
        LangType type,
        [Parameter("langue"), Description("Language du lang voulu")]
        LangLanguage language,
        [Parameter("nom"), Description("Nom du lang voulu")]
        [SlashAutoCompleteProvider<LangNameAutocompleteProvider>]
        string name)
    {
        var langRepository = LangsWatcher.LangRepositories[(type, language)];
        var lang = langRepository.GetByName(name);
        var currentDecompiledFilePath = lang?.CurrentDecompiledFilePath;

        if (lang is null || string.IsNullOrEmpty(currentDecompiledFilePath))
        {
            await ctx.RespondAsync("Ce lang n'existe pas ou n'a jamais été décompilé");
            return;
        }

        using var fileStream = File.OpenRead(currentDecompiledFilePath);
        await ctx.RespondAsync(new DiscordInteractionResponseBuilder()
            .AddFile($"{lang.FileName}.as", fileStream));
    }
}
