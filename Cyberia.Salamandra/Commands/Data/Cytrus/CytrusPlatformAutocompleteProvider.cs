using Cyberia.Cytrusaurus;
using Cyberia.Salamandra.DSharpPlus;

using DSharpPlus.Commands.Processors.SlashCommands;

namespace Cyberia.Salamandra.Commands.Data.Cytrus;

public sealed class CytrusPlatformAutoCompleteProvider : AutoCompleteProvider
{
    protected override IReadOnlyDictionary<string, object> InternalAutoComplete(AutoCompleteContext ctx)
    {
        var game = ctx.GetArgument<string>("game");
        if (string.IsNullOrEmpty(game))
        {
            return s_empty;
        }

        var cytrusGame = CytrusWatcher.Cytrus.GetGameByName(game);
        if (cytrusGame is null)
        {
            return s_empty;
        }

        return cytrusGame.Platforms
            .Take(Constant.MaxChoice)
            .ToDictionary(x => x.Key.Capitalize(), x => (object)x.Key);
    }
}
