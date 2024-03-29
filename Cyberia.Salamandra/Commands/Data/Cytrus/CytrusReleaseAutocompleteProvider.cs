using Cyberia.Cytrusaurus;
using Cyberia.Salamandra.DsharpPlus;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Data;

public sealed class CytrusReleaseAutocompleteProvider : AutocompleteProvider
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

        var cytrusGame = CytrusWatcher.Cytrus.GetGameByName(game);
        if (cytrusGame is null)
        {
            return Enumerable.Empty<DiscordAutoCompleteChoice>();
        }

        return cytrusGame.GetReleasesByPlatformName(platform)
            .Take(Constant.MAX_CHOICE)
            .Select(x => new DiscordAutoCompleteChoice(x.Key.Capitalize(), x.Key));
    }
}
