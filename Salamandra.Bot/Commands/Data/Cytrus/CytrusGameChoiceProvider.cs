using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

using Salamandra.Cytrus.Models;

namespace Salamandra.Bot.Commands.Data
{
    public sealed class CytrusGameChoiceProvider : IChoiceProvider
    {
        public Task<IEnumerable<DiscordApplicationCommandOptionChoice>> Provider()
        {
            HashSet<DiscordApplicationCommandOptionChoice> choices = new();

            foreach (KeyValuePair<string, Game> game in DiscordBot.Instance.Cytrus.CytrusData.Games)
                choices.Add(new(game.Key.Capitalize(), game.Key));

            return Task.FromResult(choices.AsEnumerable());
        }
    }
}
