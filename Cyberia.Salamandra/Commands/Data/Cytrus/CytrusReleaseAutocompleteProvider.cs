using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Data
{
    public sealed class CytrusReleaseAutocompleteProvider : AutocompleteProvider
    {
        public override Task<IEnumerable<DiscordAutoCompleteChoice>> Provider(AutocompleteContext ctx)
        {
            string? value = ctx.OptionValue.ToString();
            if (value is null || value.Length < MIN_LENGTH_AUTOCOMPLETE)
                return Task.FromResult(Enumerable.Empty<DiscordAutoCompleteChoice>());

            string? game = GetValueFromOption<string>(ctx, "game");
            if (string.IsNullOrEmpty(game))
                return Task.FromResult(Enumerable.Empty<DiscordAutoCompleteChoice>());

            string? platform = GetValueFromOption<string>(ctx, "platform");
            if (string.IsNullOrEmpty(platform))
                return Task.FromResult(Enumerable.Empty<DiscordAutoCompleteChoice>());

            HashSet<DiscordAutoCompleteChoice> choices = new();

            foreach (KeyValuePair<string, string> release in Bot.Instance.CytrusWatcher.CytrusData.Games[game].GetReleasesFromPlatform(platform))
                choices.Add(new(release.Key.Capitalize(), release.Key));

            return Task.FromResult(choices.AsEnumerable());
        }
    }
}
