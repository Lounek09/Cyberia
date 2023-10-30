using Cyberia.Api;
using Cyberia.Api.Parser;
using Cyberia.Langzilla;
using Cyberia.Langzilla.Enums;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;

using System.Text;

namespace Cyberia.Salamandra.Commands.Data
{
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
            LangType type = Enum.Parse<LangType>(typeStr);

            if (languageStr is null)
            {
                await ctx.CreateResponseAsync($"Lancement du check des langs {Formatter.Bold(typeStr)} dans toutes les langues...");
                foreach (LangLanguage language in Enum.GetValues<LangLanguage>())
                {
                    await LangsWatcher.CheckAsync(type, language, force);
                }

                await ctx.FollowUpAsync(new DiscordFollowupMessageBuilder().WithContent($"Check des langs {Formatter.Bold(typeStr)} dans toutes les langues terminé"));
            }
            else
            {
                LangLanguage language = Enum.Parse<LangLanguage>(languageStr);

                await ctx.CreateResponseAsync($"Lancement du check des langs {Formatter.Bold(typeStr)} en {Formatter.Bold(languageStr)}...");
                await LangsWatcher.CheckAsync(type, language, force);

                await ctx.FollowUpAsync(new DiscordFollowupMessageBuilder().WithContent($"Check des langs {Formatter.Bold(typeStr)} en {Formatter.Bold(languageStr)} terminé"));
            }
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
            LangType type = typeStr is null ? LangType.Official : Enum.Parse<LangType>(typeStr);
            LangLanguage language = languageStr is null ? LangLanguage.FR : Enum.Parse<LangLanguage>(languageStr);

            await ctx.CreateResponseAsync(await new LangsMessageBuilder(type, language).GetMessageAsync<DiscordInteractionResponseBuilder>());
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
            LangType type = Enum.Parse<LangType>(typeStr);
            LangLanguage language = Enum.Parse<LangLanguage>(languageStr);

            LangData? langData = LangsWatcher.GetLangsByType(type).GetLangsByLanguage(language).GetLangByName(name);
            if (langData is null)
            {
                await ctx.CreateResponseAsync("Ce lang n'existe pas ou n'a jamais été décompilé");
                return;
            }

            using FileStream fileStream = File.OpenRead(langData.GetCurrentDecompiledFilePath());
            await ctx.CreateResponseAsync(new DiscordInteractionResponseBuilder().AddFile($"{Path.GetFileNameWithoutExtension(langData.GetFileName())}.as", fileStream));
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

            LangType type = Enum.Parse<LangType>(typeStr);
            LangType typeModel = Enum.Parse<LangType>(typeModelStr);
            LangLanguage[] languages = languageStr is null ? Enum.GetValues<LangLanguage>() : new LangLanguage[] { Enum.Parse<LangLanguage>(languageStr) };

            List<Task> tasks = new();
            foreach (LangLanguage language in languages)
            {
                tasks.Add(Task.Run(async () =>
                {
                    DiscordThreadChannel thread = await ctx.Channel.CreateThreadAsync($"Diff entre {type} et {typeModel} en {language}", AutoArchiveDuration.Hour, ChannelType.PublicThread);

                    LangDataCollection langDataCollection = LangsWatcher.GetLangsByType(type).GetLangsByLanguage(language);
                    LangDataCollection langDataCollectionModel = LangsWatcher.GetLangsByType(typeModel).GetLangsByLanguage(language);

                    foreach (LangData langData in langDataCollection)
                    {
                        Task rateLimit = Task.Delay(1000);

                        LangData? langDataModel = langDataCollectionModel.GetLangByName(langData.Name);

                        DiscordMessageBuilder message = new()
                        {
                            Content = $"Lang {langData.Name}{(langDataModel is null ? $", non présent dans les langs {typeModel}" : "")}"
                        };

                        string diff = langDataModel is null ? "" : langData.GenerateDiff(langDataModel);
                        if (string.IsNullOrEmpty(diff))
                        {
                            message.Content += $"\n{Formatter.BlockCode("Aucune différence")}";
                            await thread.SendMessageAsync(message);
                        }
                        else
                        {
                            using MemoryStream stream = new(Encoding.UTF8.GetBytes(diff));
                            await thread.SendMessageAsync(message.AddFile($"{langData.Name}.as", stream));
                        }

                        await rateLimit;
                    }
                }));
            }

            await Task.WhenAll(tasks);
        }

        [SlashCommand("parse", "[Owner] Lance le parsing des langs en json")]
        [SlashRequireOwner]
        public async Task ParseLangsCommand(InteractionContext ctx)
        {
            await ctx.DeferAsync();

            LangType type = DofusApi.Config.Temporis ? LangType.Temporis : LangType.Official;
            bool success = LangParser.Launch(type, LangLanguage.FR);

            string content = success ? "Les langs ont été parsées avec succès" : "Une erreur est survenue, veuillez consulter les logs";
            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent(content));
        }
    }
}
