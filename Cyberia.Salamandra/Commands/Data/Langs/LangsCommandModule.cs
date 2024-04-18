using Cyberia.Api;
using Cyberia.Api.Parser;
using Cyberia.Langzilla;
using Cyberia.Langzilla.Enums;

using DSharpPlus;
using DSharpPlus.Commands;
using DSharpPlus.Commands.ContextChecks;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;
using DSharpPlus.Commands.Processors.SlashCommands.Metadata;
using DSharpPlus.Entities;

using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace Cyberia.Salamandra.Commands.Data;

[Command("langs")]
public sealed class LangsCommandModule
{
    [Command("check"), Description("[Owner] Lance un check des langs, si aucun language n'est précisé lance pour toutes les langues")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    [InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall)]
    [InteractionAllowedContexts(DiscordInteractionContextType.Guild)]
    [RequireApplicationOwner]
    public static async Task CheckExecuteAsync(SlashCommandContext ctx,
        [Parameter("type"), Description("Type des langs à check")]
        LangType type,
        [Parameter("langue"), Description("Language des langs à check, si vide lance pour toutes les langues")]
        [SlashChoiceProvider<LangLanguageChoiceProvider>]
        string? languageStr = null,
        [Parameter("force"), Description("Force le check")]
        bool force = false)
    {
        DiscordFollowupMessageBuilder message = new();

        if (languageStr is null)
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

        var language = Enum.Parse<LangLanguage>(languageStr);

        await ctx.RespondAsync($"Lancement du check des langs {Formatter.Bold(type.ToString())} en {Formatter.Bold(languageStr)}...");

        var repository = LangsWatcher.LangRepositories[(type, language)];
        await LangsWatcher.CheckAsync(repository, force);

        await ctx.FollowupAsync(message.WithContent($"Check des langs {Formatter.Bold(type.ToString())} en {Formatter.Bold(languageStr)} terminé"));
    }

    [Command("diff"), Description("[Owner] Lance un diff des langs entre différents types")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    [InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall)]
    [InteractionAllowedContexts(DiscordInteractionContextType.Guild)]
    [RequireApplicationOwner]
    public static async Task DiffExecuteAsync(SlashCommandContext ctx,
        [Parameter("type"), Description("Type des langs à diff")]
        LangType type,
        [Parameter("type_model"), Description("Type des langs models")]
        LangType typeModel,
        [Parameter("langue"), Description("Language des langs à diff, si vide lance pour toutes les langues")]
        [SlashChoiceProvider<LangLanguageChoiceProvider>]
        string? languageStr = null)
    {
        if (type == typeModel)
        {
            await ctx.RespondAsync("Impossible de diff le même type");
            return;
        }

        await ctx.DeferResponseAsync();

        async Task diff(LangLanguage language)
        {
            var thread = await ctx.Channel.CreateThreadAsync($"Diff entre {type} et {typeModel} en {language}", DiscordAutoArchiveDuration.Hour, DiscordChannelType.PublicThread);

            var langRepository = LangsWatcher.LangRepositories[(type, language)];
            var langRepositoryModel = LangsWatcher.LangRepositories[(typeModel, language)];

            foreach (var lang in langRepository.Langs)
            {
                var rateLimit = Task.Delay(1000);

                var langModel = langRepositoryModel.GetByName(lang.Name);

                var message = new DiscordMessageBuilder()
                {
                    Content = $"{(lang.New ? $"{Formatter.Bold("New")} lang" : "Lang")} " +
                        $"{Formatter.Bold(lang.Name)} version {Formatter.Bold(lang.Version.ToString())}" +
                        langModel is null ? $", non présent dans les langs {typeModel}" : string.Empty
                };

                var diff = lang.Diff(langModel);
                if (string.IsNullOrEmpty(diff))
                {
                    await thread.SendMessageAsync(message);
                }
                else
                {
                    using var stream = new MemoryStream(Encoding.UTF8.GetBytes(diff));
                    await thread.SendMessageAsync(message.AddFile($"{lang.Name}.as", stream));
                }

                await rateLimit;
            }
        }

        if (languageStr is null)
        {
            await Task.WhenAll(Enum.GetValues<LangLanguage>().Select(diff));
        }
        else
        {
            var language = Enum.Parse<LangLanguage>(languageStr);
            await diff(language);
        }

        await ctx.EditResponseAsync("Done");
    }

    [Command("parse"), Description("[Owner] Lance le parsing des langs en json")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    [InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall)]
    [InteractionAllowedContexts(DiscordInteractionContextType.Guild)]
    [RequireApplicationOwner]
    public static async Task ParseExecuteAsync(SlashCommandContext ctx)
    {
        await ctx.DeferResponseAsync();

        var type = DofusApi.Config.Temporis
            ? LangType.Temporis
            : LangType.Official;

        var startTime = Stopwatch.GetTimestamp();
        var success = LangParser.Launch(type, LangLanguage.FR);
        var elapsedTime = Stopwatch.GetElapsedTime(startTime);

        var content = success
            ? $"Les langs ont été parsées avec succès en {elapsedTime:s\\,ffff}s"
            : "Une erreur est survenue, veuillez consulter les logs";

        await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent(content));
    }

    [Command("show"), Description("Affiche les informations des langs actuellement en ligne")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    [InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall)]
    [InteractionAllowedContexts(DiscordInteractionContextType.Guild)]
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
    [InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall)]
    [InteractionAllowedContexts(DiscordInteractionContextType.Guild)]
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
