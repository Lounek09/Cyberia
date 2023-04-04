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
            HashSet<DiscordAutoCompleteChoice> choices = new();

            string? value = ctx.OptionValue.ToString();
            if (value is not null && value.Length >= MIN_LENGTH_AUTOCOMPLETE)
            {
                string? typeStr = GetValueFromOption<string>(ctx, "typeStr");
                string? languageStr = GetValueFromOption<string>(ctx, "languageStr");

                if (!string.IsNullOrEmpty(typeStr) && !string.IsNullOrEmpty(languageStr))
                {
                    LangType type = Enum.Parse<LangType>(typeStr);
                    Language language = Enum.Parse<Language>(languageStr);

                    foreach (Lang lang in Bot.Instance.DofusLangs.GetLangsData(type, language).GetLangsByName(value).Take(25))
                        choices.Add(new(lang.Name, lang.Name));
                }
            }

            return Task.FromResult(choices.AsEnumerable());
        }
    }
}
