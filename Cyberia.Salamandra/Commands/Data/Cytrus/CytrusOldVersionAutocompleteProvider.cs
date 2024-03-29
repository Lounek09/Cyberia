using Cyberia.Cytrusaurus;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Data;

public sealed class CytrusOldVersionAutocompleteProvider : AutocompleteProvider
{
    public override Task<IEnumerable<DiscordAutoCompleteChoice>> Provider(AutocompleteContext ctx)
    {
        var value = ctx.OptionValue.ToString();
        if (value is null)
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

        var release = CreateFromOption<string>(ctx, "old_release");
        if (string.IsNullOrEmpty(release))
        {
            return Task.FromResult(Enumerable.Empty<DiscordAutoCompleteChoice>());
        }

        List<DiscordAutoCompleteChoice> choices = [];

        var version = CytrusWatcher.OldCytrus.Games[game].GetVersionByPlatformNameAndReleaseName(platform, release);
        if (!string.IsNullOrEmpty(version))
        {
            choices.Add(new(version, version));
        }

        return Task.FromResult(choices.AsEnumerable());
    }
}
