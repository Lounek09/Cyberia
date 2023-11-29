using Cyberia.Cytrusaurus;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Data;

public sealed class CytrusGameChoiceProvider : IChoiceProvider
{
    public Task<IEnumerable<DiscordApplicationCommandOptionChoice>> Provider()
    {
        List<DiscordApplicationCommandOptionChoice> choices = [];

        foreach (var game in CytrusWatcher.Data.Games)
        {
            choices.Add(new(game.Key.Capitalize(), game.Key));
        }

        return Task.FromResult(choices.AsEnumerable());
    }
}
