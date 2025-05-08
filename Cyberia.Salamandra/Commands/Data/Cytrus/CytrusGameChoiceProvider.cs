using Cyberia.Cytrusaurus;

using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;
using DSharpPlus.Commands.Trees;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Data.Cytrus;

public sealed class CytrusGameChoiceProvider : IChoiceProvider
{
    private readonly ICytrusWatcher _cytrusWatcher;

    public CytrusGameChoiceProvider(ICytrusWatcher cytrusWatcher)
    {
        _cytrusWatcher = cytrusWatcher;
    }

    public ValueTask<IEnumerable<DiscordApplicationCommandOptionChoice>> ProvideAsync(CommandParameter parameter)
    {
        var choices = _cytrusWatcher.Cytrus.Games.Select(x =>
        {
            return new DiscordApplicationCommandOptionChoice(x.Key.Capitalize(), x.Key);
        });

        return ValueTask.FromResult(choices);
    }
}
