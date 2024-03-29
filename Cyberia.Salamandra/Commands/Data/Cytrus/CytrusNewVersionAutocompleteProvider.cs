using Cyberia.Cytrusaurus;
using Cyberia.Salamandra.DsharpPlus;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Data;

public sealed class CytrusNewVersionAutocompleteProvider : AutocompleteProvider
{
    protected override IEnumerable<DiscordAutoCompleteChoice> InternalProvider(AutocompleteContext ctx, string value)
    {
        var game = ctx.GetOption<string>("game");
        if (string.IsNullOrEmpty(game))
        {
            return Enumerable.Empty<DiscordAutoCompleteChoice>();
        }

        var platform = ctx.GetOption<string>("platform");
        if (string.IsNullOrEmpty(platform))
        {
            return Enumerable.Empty<DiscordAutoCompleteChoice>();
        }

        var release = ctx.GetOption<string>("new_release");
        if (string.IsNullOrEmpty(release))
        {
            return Enumerable.Empty<DiscordAutoCompleteChoice>();
        }

        var cytrusGame = CytrusWatcher.Cytrus.GetGameByName(game);
        if (cytrusGame is null)
        {
            return Enumerable.Empty<DiscordAutoCompleteChoice>();
        }

        var version = cytrusGame.GetVersionByPlatformNameAndReleaseName(platform, release);
        if (string.IsNullOrEmpty(version))
        {
            return Enumerable.Empty<DiscordAutoCompleteChoice>();
        }

        return Enumerable.Repeat(new DiscordAutoCompleteChoice(version, version), 1);
    }
}
