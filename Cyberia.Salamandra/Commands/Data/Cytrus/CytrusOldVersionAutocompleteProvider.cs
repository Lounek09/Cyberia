using Cyberia.Cytrusaurus;
using Cyberia.Salamandra.Extensions.DSharpPlus;
using DSharpPlus.Commands.Processors.SlashCommands;

using System.Collections.ObjectModel;

namespace Cyberia.Salamandra.Commands.Data.Cytrus;

public sealed class CytrusOldVersionAutocompleteProvider : AutoCompleteProvider
{
    private readonly CytrusWatcher _cytrusWatcher;

    public CytrusOldVersionAutocompleteProvider(CytrusWatcher cytrusWatcher)
    {
        _cytrusWatcher = cytrusWatcher;
    }

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

        var release = ctx.GetArgument<string>("old_release");
        if (string.IsNullOrEmpty(release))
        {
            return ReadOnlyDictionary<string, object>.Empty;
        }

        var cytrusGame = _cytrusWatcher.Cytrus.GetGameByName(game);
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
