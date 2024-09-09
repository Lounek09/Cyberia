using Cyberia.Cytrusaurus;
using Cyberia.Salamandra.DSharpPlus;

using DSharpPlus.Commands.Processors.SlashCommands;

using System.Collections.ObjectModel;

namespace Cyberia.Salamandra.Commands.Data.Cytrus;

public sealed class CytrusNewVersionAutocompleteProvider : AutoCompleteProvider
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

        var release = ctx.GetArgument<string>("new_release");
        if (string.IsNullOrEmpty(release))
        {
            return ReadOnlyDictionary<string, object>.Empty;
        }

        var cytrusGame = CytrusWatcher.Cytrus.GetGameByName(game);
        if (cytrusGame is null)
        {
            return ReadOnlyDictionary<string, object>.Empty;
        }

        var version = cytrusGame.GetVersionByPlatformNameAndReleaseName(platform, release);
        if (string.IsNullOrEmpty(version))
        {
            return ReadOnlyDictionary<string, object>.Empty;
        }

        return new Dictionary<string, object>
        {
            { version, version }
        };
    }
}
