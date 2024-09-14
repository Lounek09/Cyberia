using Cyberia.Cytrusaurus;

using DSharpPlus.Commands.Trees;

namespace Cyberia.Salamandra.Commands.Data.Cytrus;

public sealed class CytrusGameChoiceProvider : ChoiceProvider
{
    private readonly CytrusWatcher _cytrusWatcher;

    public CytrusGameChoiceProvider(CytrusWatcher cytrusWatcher)
    {
        _cytrusWatcher = cytrusWatcher;
    }

    protected override IReadOnlyDictionary<string, object> InternalProvide(CommandParameter parameter)
    {
        return _cytrusWatcher.Cytrus.Games.ToDictionary(x => x.Key.Capitalize(), x => (object)x.Key);
    }
}
