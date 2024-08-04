using Cyberia.Cytrusaurus;

using DSharpPlus.Commands.Trees;

namespace Cyberia.Salamandra.Commands.Data.Cytrus;

public sealed class CytrusGameChoiceProvider : ChoiceProvider
{
    protected override IReadOnlyDictionary<string, object> InternalProvide(CommandParameter parameter)
    {
        return CytrusWatcher.Cytrus.Games.ToDictionary(x => x.Key.Capitalize(), x => (object)x.Key);
    }
}
