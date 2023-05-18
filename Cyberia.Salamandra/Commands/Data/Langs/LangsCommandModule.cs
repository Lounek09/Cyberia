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
#pragma warning disable CA1822 // Mark members as static
    [SlashCommandGroup("langs", "Langs")]
    public sealed class LangsCommandModule : ApplicationCommandModule
    {
        [SlashCommand("check", "Lance un check des langs, si aucun language n'est précisé lance pour toutes les langues")]
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
                foreach (Language language in Enum.GetValues<Language>())
                    await Bot.Instance.DofusLangs.LaunchAsync(type, language, force);
                await ctx.FollowUpAsync(new DiscordFollowupMessageBuilder().WithContent($"Check des langs {Formatter.Bold(typeStr)} dans toutes les langues terminé"));
            }
            else
            {
                Language language = Enum.Parse<Language>(languageStr);
                await ctx.CreateResponseAsync($"Lancement du check des langs {Formatter.Bold(typeStr)} en {Formatter.Bold(languageStr)}...");
                await Bot.Instance.DofusLangs.LaunchAsync(type, language, force);
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
            Language language = languageStr is null ? Language.FR : Enum.Parse<Language>(languageStr);

            await new LangsMessageBuilder(type, language).SendInteractionResponse(ctx.Interaction);
        }

        [SlashCommand("get", "Retourne le lang demandé décompilé")]
        public async Task GetLangsCommand(InteractionContext ctx,
            [Option("type", "Type du lang voulu")]
            [ChoiceProvider(typeof(LangTypeChoiceProvider))]
            string typeStr,
            [Option("langue", "Language du lang voulu")]
            [ChoiceProvider(typeof(LanguageChoiceProvider))]
            string languageStr,
            [Option("nom", "Nom du lang voulu")]
            [Autocomplete(typeof(LangNameAutocompleteProvider))]
            string name)
        {
            LangType type = Enum.Parse<LangType>(typeStr);
            Language language = Enum.Parse<Language>(languageStr);

            Lang? lang = Bot.Instance.DofusLangs.GetLangsData(type, language).GetLangByName(name);
            if (lang is null)
            {
                await ctx.CreateResponseAsync("Ce lang n'existe pas ou n'a jamais été décompilé");
                return;
            }

            using FileStream fileStream = File.OpenRead(lang.GetCurrentDecompiledFilePath());
            await ctx.CreateResponseAsync(new DiscordInteractionResponseBuilder().AddFile($"{Path.GetFileNameWithoutExtension(lang.GetFileName())}.txt", fileStream));
        }

        [SlashCommand("diff", "Lance un diff des langs entre différents types")]
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
            Language[] languages = languageStr is null ? Enum.GetValues<Language>() : new Language[] { Enum.Parse<Language>(languageStr) };

            List<Task> tasks = new();
            foreach (Language language in languages)
            {
                tasks.Add(Task.Run(async () =>
                {
                    DiscordThreadChannel thread = await ctx.Channel.CreateThreadAsync($"Diff entre {type} et {typeModel} en {language}", AutoArchiveDuration.Hour, ChannelType.PublicThread);

                    LangsData data = Bot.Instance.DofusLangs.GetLangsData(type, language);
                    LangsData dataModel = Bot.Instance.DofusLangs.GetLangsData(typeModel, language);

                    foreach (Lang lang in data.Langs)
                    {
                        Task rateLimit = Task.Delay(1000);

                        Lang? langModel = dataModel.GetLangByName(lang.Name);

                        DiscordMessageBuilder message = new()
                        {
                            Content = $"Lang {lang.Name}{(langModel is null ? $", non présent dans les langs {typeModel}" : "")}"
                        };

                        string diff = langModel is null ? "" : lang.GenerateDiff(langModel);
                        if (string.IsNullOrEmpty(diff))
                        {
                            message.Content += $"\n{Formatter.BlockCode("Aucune différence")}";
                            await thread.SendMessageAsync(message);
                        }
                        else
                        {
                            using MemoryStream stream = new(Encoding.UTF8.GetBytes(diff));
                            await thread.SendMessageAsync(message.AddFile($"{lang.Name}.as", stream));
                        }

                        await rateLimit;
                    }
                }));
            }

            await Task.WhenAll(tasks);
        }

        [SlashCommand("parse", "Lance le parsing des langs en json")]
        [SlashRequireOwner]
        [SlashRequirePermissions(Permissions.SendMessages)]
        public async Task ParseLangsCommand(InteractionContext ctx)
        {
            await ctx.DeferAsync();

            bool success = LangParser.Launch();

            string content = success ? "Les langs ont été parsées avec succès" : "Une erreur est survenue, veuillez consulter les logs";
            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent(content));
        }
    }
}
