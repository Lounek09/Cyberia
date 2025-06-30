using Cyberia.Cytrusaurus;

using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Data.Cytrus;

public sealed class CytrusGameAutoCompleteProvider : IAutoCompleteProvider
{
    private readonly ICytrusWatcher _cytrusWatcher;

    public CytrusGameAutoCompleteProvider(ICytrusWatcher cytrusWatcher)
    {
        _cytrusWatcher = cytrusWatcher;
    }

    public ValueTask<IEnumerable<DiscordAutoCompleteChoice>> AutoCompleteAsync(AutoCompleteContext context)
    {
        var choices = _cytrusWatcher.Cytrus.Games.Select(x =>
        {
            return new DiscordAutoCompleteChoice(x.Key.Capitalize(), x.Key);
        });

        return ValueTask.FromResult(choices);
    }
}
