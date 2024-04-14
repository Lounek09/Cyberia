using Cyberia.Api;
using Cyberia.Api.Parser;
using Cyberia.Langzilla;
using Cyberia.Langzilla.Enums;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;

using System.Diagnostics;
using System.Text;

namespace Cyberia.Salamandra.Commands.Data;

[SlashCommandGroup("langs", "Langs")]
public sealed class LangsCommandModule : ApplicationCommandModule
{
    [SlashCommand("check", "[Owner] Lance un check des langs, si aucun language n'est précisé lance pour toutes les langues")]
    [SlashRequireOwner]
    public async Task CheckLangsCommand(InteractionContext ctx,
        [Option("type", "Type des langs à check")]
        [ChoiceProvider(typeof(LangTypeChoiceProvider))]
        string typeStr,
        [Option("langue", "Language des langs à check, si vide lance pour toutes les langues")]
        [ChoiceProvider(typeof(LanguageChoiceProvider))]
        string? languageStr = null,
        [Option("force", "Force le check")]
        bool force = false)
    {
        DiscordFollowupMessageBuilder message = new();
        var type = Enum.Parse<LangType>(typeStr);

        if (languageStr is null)
        {
            await ctx.CreateResponseAsync($"Lancement du check des langs {Formatter.Bold(typeStr)} dans toutes les langues...");

            await Task.WhenAll(
                Enum.GetValues<LangLanguage>()
                    .Select(x =>
                    {
                        var repository = LangsWatcher.LangRepositories[(type, x)];
                        return LangsWatcher.CheckAsync(repository, force);
                    }));

            await ctx.FollowUpAsync(message.WithContent($"Check des langs {Formatter.Bold(typeStr)} dans toutes les langues terminé"));
            return;
        }

        var language = Enum.Parse<LangLanguage>(languageStr);

        await ctx.CreateResponseAsync($"Lancement du check des langs {Formatter.Bold(typeStr)} en {Formatter.Bold(languageStr)}...");

        var repository = LangsWatcher.LangRepositories[(type, language)];
        await LangsWatcher.CheckAsync(repository, force);

        await ctx.FollowUpAsync(message.WithContent($"Check des langs {Formatter.Bold(typeStr)} en {Formatter.Bold(languageStr)} terminé"));
    }

    [SlashCommand("show", "Affiche les informations des langs actuellement en ligne")]
    public async Task ShowLangsCommand(InteractionContext ctx,
        [Option("type", "Type des langs à afficher")]
        [ChoiceProvider(typeof(LangTypeChoiceProvider))]
        string? typeStr = null,
        [Option("langue", "Language des langs à afficher")]
        [ChoiceProvider(typeof(LanguageChoiceProvider))]
        string? languageStr = null)
    {
        var type = typeStr is null
            ? LangType.Official
            : Enum.Parse<LangType>(typeStr);

        var language = languageStr is null
            ? LangLanguage.FR
            : Enum.Parse<LangLanguage>(languageStr);

        var message = await new LangsMessageBuilder(type, language)
            .GetMessageAsync<DiscordInteractionResponseBuilder>();

        await ctx.CreateResponseAsync(message);
    }

    [SlashCommand("get", "Retourne le lang demandé décompilé")]
    public async Task GetLangsCommand(InteractionContext ctx,
        [Option("type", "Type du lang voulu")]
        [ChoiceProvider(typeof(LangTypeChoiceProvider))]
        string typeStr,
        [Option("langue", "Language du lang voulu")]
        [ChoiceProvider(typeof(LanguageChoiceProvider))]
        string languageStr,
        [Option("nom", "Nom du lang voulu", true)]
        [Autocomplete(typeof(LangNameAutocompleteProvider))]
        string name)
    {
        var type = Enum.Parse<LangType>(typeStr);
        var language = Enum.Parse<LangLanguage>(languageStr);
        var langRepository = LangsWatcher.LangRepositories[(type, language)];
        var lang = langRepository.GetByName(name);
        var currentDecompiledFilePath = lang?.CurrentDecompiledFilePath;

        if (lang is null || string.IsNullOrEmpty(currentDecompiledFilePath))
        {
            await ctx.CreateResponseAsync("Ce lang n'existe pas ou n'a jamais été décompilé");
            return;
        }

        using var fileStream = File.OpenRead(currentDecompiledFilePath);
        var message = new DiscordInteractionResponseBuilder()
            .AddFile($"{lang.FileName}.as", fileStream);

        await ctx.CreateResponseAsync(message);
    }

    [SlashCommand("diff", "[Owner] Lance un diff des langs entre différents types")]
    [SlashRequireOwner]
    public async Task DiffLangsCommand(InteractionContext ctx,
        [Option("type", "Type des langs à diff")]
        [ChoiceProvider(typeof(LangTypeChoiceProvider))]
        string typeStr,
        [Option("type_model", "Type des langs models")]
        [ChoiceProvider(typeof(LangTypeChoiceProvider))]
        string typeModelStr,
        [Option("langue", "Language des langs à diff, si vide lance pour toutes les langues")]
        [ChoiceProvider(typeof(LanguageChoiceProvider))]
        string? languageStr = null)
    {
        if (typeStr.Equals(typeModelStr))
        {
            await ctx.CreateResponseAsync($"Impossible de diff le même type");
            return;
        }

        await ctx.CreateResponseAsync("👷", true);

        var type = Enum.Parse<LangType>(typeStr);
        var typeModel = Enum.Parse<LangType>(typeModelStr);

        async Task diff(LangLanguage language)
        {
            var thread = await ctx.Channel.CreateThreadAsync($"Diff entre {type} et {typeModel} en {language}", AutoArchiveDuration.Hour, ChannelType.PublicThread);

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
            return;
        }

        await diff(Enum.Parse<LangLanguage>(languageStr));
    }

    [SlashCommand("parse", "[Owner] Lance le parsing des langs en json")]
    [SlashRequireOwner]
    public async Task ParseLangsCommand(InteractionContext ctx)
    {
        await ctx.DeferAsync();

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
}
