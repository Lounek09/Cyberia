using Cyberia.Cytrusaurus;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Data;

public sealed class CytrusGameChoiceProvider : IChoiceProvider
{
    public Task<IEnumerable<DiscordApplicationCommandOptionChoice>> Provider()
    {
        return Task.FromResult(
            CytrusWatcher.Cytrus.Games
                .Select(x => new DiscordApplicationCommandOptionChoice(x.Key.Capitalize(), x.Key)));
    }
}
