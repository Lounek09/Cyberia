using Cyberia.Api.Parser;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Data
{
    public sealed class LangNameToParseAutocompleteProvider : AutocompleteProvider
    {
        public override Task<IEnumerable<DiscordAutoCompleteChoice>> Provider(AutocompleteContext ctx)
        {
            HashSet<DiscordAutoCompleteChoice> choices = new();

            string? value = ctx.OptionValue.ToString();
            if (value is not null && value.Length >= MIN_LENGTH_AUTOCOMPLETE)
            {
                foreach (string lang in LangParser.LangsToParse.FindAll(x => value.All(x.RemoveDiacritics().Contains)).Take(25))
                    choices.Add(new(lang, lang));
            }

            return Task.FromResult(choices.AsEnumerable());
        }
    }
}
