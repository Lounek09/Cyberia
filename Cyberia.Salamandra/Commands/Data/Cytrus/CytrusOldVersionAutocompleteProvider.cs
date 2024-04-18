using Cyberia.Cytrusaurus;
using Cyberia.Salamandra.DsharpPlus;

using DSharpPlus.Commands.Processors.SlashCommands;

namespace Cyberia.Salamandra.Commands.Data;

public sealed class CytrusOldVersionAutocompleteProvider : AutoCompleteProvider
{
    protected override IReadOnlyDictionary<string, object> InternalAutoComplete(AutoCompleteContext ctx)
    {
        var game = ctx.GetArgument<string>("game");
        if (string.IsNullOrEmpty(game))
        {
            return s_empty;
        }

        var platform = ctx.GetArgument<string>("platform");
        if (string.IsNullOrEmpty(platform))
        {
            return s_empty;
        }

        var release = ctx.GetArgument<string>("old_release");
        if (string.IsNullOrEmpty(release))
        {
            return s_empty;
        }

        var cytrusGame = CytrusWatcher.Cytrus.GetGameByName(game);
        if (cytrusGame is null)
        {
            return s_empty;
        }

        var version = cytrusGame.GetVersionByPlatformNameAndReleaseName(platform, release);
        if (string.IsNullOrEmpty(version))
        {
            return s_empty;
        }

        return new Dictionary<string, object>
        {
            { version, version }
        };
    }
}
