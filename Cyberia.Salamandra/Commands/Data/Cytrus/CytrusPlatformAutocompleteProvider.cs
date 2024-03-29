using Cyberia.Cytrusaurus;
using Cyberia.Salamandra.DsharpPlus;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Data;

public sealed class CytrusPlatformAutocompleteProvider : AutocompleteProvider
{
    protected override IEnumerable<DiscordAutoCompleteChoice> InternalProvider(AutocompleteContext ctx, string value)
    {
        var game = ctx.GetOption<string>("game");
        if (string.IsNullOrEmpty(game))
        {
            return Enumerable.Empty<DiscordAutoCompleteChoice>();
        }

        var cytrusGame = CytrusWatcher.Cytrus.GetGameByName(game);
        if (cytrusGame is null)
        {
            return Enumerable.Empty<DiscordAutoCompleteChoice>();
        }

        return cytrusGame.Platforms
            .Take(Constant.MAX_CHOICE)
            .Select(x => new DiscordAutoCompleteChoice(x.Key.Capitalize(), x.Key));
    }
}
