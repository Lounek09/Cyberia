using Cyberia.Cytrusaurus;
using Cyberia.Salamandra.DSharpPlus;

using DSharpPlus.Commands.Processors.SlashCommands;

using System.Collections.ObjectModel;

namespace Cyberia.Salamandra.Commands.Data.Cytrus;

public sealed class CytrusReleaseAutoCompleteProvider : AutoCompleteProvider
{
    protected override IReadOnlyDictionary<string, object> InternalAutoComplete(AutoCompleteContext ctx)
    {
        var game = ctx.GetArgument<string>("game");
        if (string.IsNullOrEmpty(game))
        {
            return ReadOnlyDictionary<string, object>.Empty;
        }

        var platform = ctx.GetArgument<string>("platform");
        if (string.IsNullOrEmpty(platform))
        {
            return ReadOnlyDictionary<string, object>.Empty;
        }

        var cytrusGame = CytrusWatcher.Cytrus.GetGameByName(game);
        if (cytrusGame is null)
        {
            return ReadOnlyDictionary<string, object>.Empty;
        }

        return cytrusGame.GetReleasesByPlatformName(platform)
           .Take(Constant.MaxChoice)
           .ToDictionary(x => x.Key.Capitalize(), x => (object)x.Key);
    }
}
