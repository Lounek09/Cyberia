using Cyberia.Cytrusaurus;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Data;

public sealed class CytrusPlatformAutocompleteProvider : AutocompleteProvider
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

        List<DiscordAutoCompleteChoice> choices = [];

        foreach (var platform in CytrusWatcher.Data.Games[game].Platforms)
        {
            choices.Add(new(platform.Key.Capitalize(), platform.Key));
        }

        return Task.FromResult(choices.AsEnumerable());
    }
}
