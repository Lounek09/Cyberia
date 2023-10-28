using Cyberia.Langzilla;
using Cyberia.Langzilla.Enums;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Data
{
    public sealed class LangNameAutocompleteProvider : AutocompleteProvider
    {
        public override Task<IEnumerable<DiscordAutoCompleteChoice>> Provider(AutocompleteContext ctx)
        {
            string? value = ctx.OptionValue.ToString();
            if (value is null || value.Length < MIN_LENGTH_AUTOCOMPLETE)
            {
                return Task.FromResult(Enumerable.Empty<DiscordAutoCompleteChoice>());
            }

            string? typeStr = CreateFromOption<string>(ctx, "typeStr");
            if (string.IsNullOrEmpty(typeStr))
            {
                return Task.FromResult(Enumerable.Empty<DiscordAutoCompleteChoice>());
            }

            string? languageStr = CreateFromOption<string>(ctx, "languageStr");
            if (string.IsNullOrEmpty(languageStr))
            {
                return Task.FromResult(Enumerable.Empty<DiscordAutoCompleteChoice>());
            }

            HashSet<DiscordAutoCompleteChoice> choices = new();

            LangType type = Enum.Parse<LangType>(typeStr);
            Language language = Enum.Parse<Language>(languageStr);

            foreach (Lang lang in LangsWatcher.GetLangsByType(type).GetLangsByLanguage(language).GetLangsByName(value).Take(MAX_AUTOCOMPLETE_CHOICE))
            {
                choices.Add(new(lang.Name, lang.Name));
            }

            return Task.FromResult(choices.AsEnumerable());
        }
    }
}
