using Cyberia.Cytrusaurus;
using Cyberia.Salamandra.Extensions.DSharpPlus;

using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Data.Cytrus;

public sealed class CytrusNewVersionAutocompleteProvider : IAutoCompleteProvider
{
    private readonly CytrusWatcher _cytrusWatcher;

    public CytrusNewVersionAutocompleteProvider(CytrusWatcher cytrusWatcher)
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

        var platform = ctx.GetArgument<string>("platform");
        if (string.IsNullOrEmpty(platform))
        {
            return ValueTask.FromResult(Enumerable.Empty<DiscordAutoCompleteChoice>());
        }

        var release = ctx.GetArgument<string>("new_release");
        if (string.IsNullOrEmpty(release))
        {
            return ValueTask.FromResult(Enumerable.Empty<DiscordAutoCompleteChoice>());
        }

        var cytrusGame = _cytrusWatcher.Cytrus.GetGameByName(game);
        if (cytrusGame is null)
        {
            return ValueTask.FromResult(Enumerable.Empty<DiscordAutoCompleteChoice>());
        }

        var version = cytrusGame.GetVersionByPlatformNameAndReleaseName(platform, release);
        if (string.IsNullOrEmpty(version))
        {
            return ValueTask.FromResult(Enumerable.Empty<DiscordAutoCompleteChoice>());
        }

        var choice = new DiscordAutoCompleteChoice(version, version);
        return ValueTask.FromResult(Enumerable.Repeat(choice, 1));
    }
}
