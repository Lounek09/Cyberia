using Cyberia.Cytrusaurus;
using Cyberia.Cytrusaurus.Models;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Data
{
    public sealed class CytrusGameChoiceProvider : IChoiceProvider
    {
        public Task<IEnumerable<DiscordApplicationCommandOptionChoice>> Provider()
        {
            HashSet<DiscordApplicationCommandOptionChoice> choices = new();

            foreach (KeyValuePair<string, Game> game in CytrusWatcher.Data.Games)
            {
                choices.Add(new(game.Key.Capitalize(), game.Key));
            }

            return Task.FromResult(choices.AsEnumerable());
        }
    }
}
