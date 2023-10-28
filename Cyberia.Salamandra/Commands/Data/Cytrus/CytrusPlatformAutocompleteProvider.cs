using Cyberia.Cytrusaurus;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Data
{
    public sealed class CytrusPlatformAutocompleteProvider : AutocompleteProvider
    {
        public override Task<IEnumerable<DiscordAutoCompleteChoice>> Provider(AutocompleteContext ctx)
        {
            string? value = ctx.OptionValue.ToString();
            if (value is null || value.Length < MIN_LENGTH_AUTOCOMPLETE)
            {
                return Task.FromResult(Enumerable.Empty<DiscordAutoCompleteChoice>());
            }

            string? game = CreateFromOption<string>(ctx, "game");
            if (string.IsNullOrEmpty(game))
            {
                return Task.FromResult(Enumerable.Empty<DiscordAutoCompleteChoice>());
            }

            List<DiscordAutoCompleteChoice> choices = new();

            foreach (KeyValuePair<string, Dictionary<string, string>> platform in CytrusWatcher.Data.Games[game].Platforms)
            {
                choices.Add(new(platform.Key.Capitalize(), platform.Key));
            }

            return Task.FromResult(choices.AsEnumerable());
        }
    }
}
