using Cyberia.Cytrusaurus;

using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;
using DSharpPlus.Commands.Trees;

namespace Cyberia.Salamandra.Commands.Data.Cytrus;

public sealed class CytrusGameChoiceProvider : IChoiceProvider
{
    public ValueTask<IReadOnlyDictionary<string, object>> ProvideAsync(CommandParameter parameter)
    {
        return new ValueTask<IReadOnlyDictionary<string, object>>(
            CytrusWatcher.Cytrus.Games.ToDictionary(x => x.Key.Capitalize(), x => (object)x.Key));
    }
}
