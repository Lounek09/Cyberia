﻿using Cyberia.Langs.Enums;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Data
{
    [SlashCommandGroup("langs", "Langs")]
    public sealed class LangsCommandModule : ApplicationCommandModule
    {
        [SlashCommand("check", "Lance un check des langs, si aucun language n'est précisé lance pour toutes les langues")]
        public async Task CheckLangCommand(InteractionContext ctx,
            [Option("type", "Type des langs à check")]
            [ChoiceProvider(typeof(LangTypeChoiceProvider))]
            string typeStr,
            [Option("langue", "Langue des langs à check, si vide lance pour toutes les langues")]
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
                    await Bot.Instance.Langs.LaunchAsync(type, language, force);
                await ctx.FollowUpAsync(new DiscordFollowupMessageBuilder().WithContent($"Check des langs {Formatter.Bold(typeStr)} dans toutes les langues terminé"));
            }
            else
            {
                Language language = Enum.Parse<Language>(languageStr);
                await ctx.CreateResponseAsync($"Lancement du check des langs {Formatter.Bold(typeStr)} en {Formatter.Bold(languageStr)}...");
                await Bot.Instance.Langs.LaunchAsync(type, language, force);
                await ctx.FollowUpAsync(new DiscordFollowupMessageBuilder().WithContent($"Check des langs {Formatter.Bold(typeStr)} en {Formatter.Bold(languageStr)} terminé"));
            }
        }
    }
}