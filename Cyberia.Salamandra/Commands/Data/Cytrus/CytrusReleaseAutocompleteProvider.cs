using Cyberia.Cytrusaurus;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Data;

public sealed class CytrusReleaseAutocompleteProvider : AutocompleteProvider
{
    public override Task<IEnumerable<DiscordAutoCompleteChoice>> Provider(AutocompleteContext ctx)
    {
        var value = ctx.OptionValue.ToString();
        if (value is null || value.Length < MIN_LENGTH_AUTOCOMPLETE)
        {
            return Task.FromResult(Enumerable.Empty<DiscordAutoCompleteChoice>());
        }

        var game = CreateFromOption<string>(ctx, "game");
        if (string.IsNullOrEmpty(game))
        {
            return Task.FromResult(Enumerable.Empty<DiscordAutoCompleteChoice>());
        }

        var platform = CreateFromOption<string>(ctx, "platform");
        if (string.IsNullOrEmpty(platform))
        {
            return Task.FromResult(Enumerable.Empty<DiscordAutoCompleteChoice>());
        }

        List<DiscordAutoCompleteChoice> choices = [];

        foreach (var release in CytrusWatcher.Data.Games[game].GetReleasesByPlatform(platform))
        {
            choices.Add(new(release.Key.Capitalize(), release.Key));
        }

        return Task.FromResult(choices.AsEnumerable());
    }
}
