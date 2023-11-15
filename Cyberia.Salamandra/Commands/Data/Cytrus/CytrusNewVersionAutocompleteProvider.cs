using Cyberia.Cytrusaurus;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Data
{
    public sealed class CytrusNewVersionAutocompleteProvider : AutocompleteProvider
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

            string? platform = CreateFromOption<string>(ctx, "platform");
            if (string.IsNullOrEmpty(platform))
            {
                return Task.FromResult(Enumerable.Empty<DiscordAutoCompleteChoice>());
            }

            string? release = CreateFromOption<string>(ctx, "new_release");
            if (string.IsNullOrEmpty(release))
            {
                return Task.FromResult(Enumerable.Empty<DiscordAutoCompleteChoice>());
            }

            List<DiscordAutoCompleteChoice> choices = [];

            string? version = CytrusWatcher.Data.Games[game].GetVersionFromPlatformAndRelease(platform, release);
            if (!string.IsNullOrEmpty(version))
            {
                choices.Add(new(version, version));
            }

            return Task.FromResult(choices.AsEnumerable());
        }
    }
}
