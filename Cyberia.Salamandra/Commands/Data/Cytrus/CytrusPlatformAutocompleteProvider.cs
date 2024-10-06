using Cyberia.Cytrusaurus;
using Cyberia.Salamandra.Extensions.DSharpPlus;

using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Data.Cytrus;

public sealed class CytrusPlatformAutoCompleteProvider : IAutoCompleteProvider
{
    private readonly CytrusWatcher _cytrusWatcher;

    public CytrusPlatformAutoCompleteProvider(CytrusWatcher cytrusWatcher)
    {
        _cytrusWatcher = cytrusWatcher;
    }

    public ValueTask<IEnumerable<DiscordAutoCompleteChoice>> AutoCompleteAsync(AutoCompleteContext ctx)
    {
        var game = ctx.GetArgument<string>("game");
        if (string.IsNullOrEmpty(game))
        {
            return ValueTask.FromResult(Enumerable.Empty<DiscordAutoCompleteChoice>());
        }

        var cytrusGame = _cytrusWatcher.Cytrus.GetGameByName(game);
        if (cytrusGame is null)
        {
            return ValueTask.FromResult(Enumerable.Empty<DiscordAutoCompleteChoice>());
        }

        var choices = cytrusGame.Platforms
            .Take(Constant.MaxChoice)
            .Select(x => new DiscordAutoCompleteChoice(x.Key.Capitalize(), x.Key));

        return ValueTask.FromResult(choices);
    }
}
