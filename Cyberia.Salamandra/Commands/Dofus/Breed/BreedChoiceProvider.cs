using Cyberia.Api.DatacenterNS;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public sealed class BreedChoiceProvider : IChoiceProvider
    {
        public Task<IEnumerable<DiscordApplicationCommandOptionChoice>> Provider()
        {
            List<DiscordApplicationCommandOptionChoice> choices = new();

            foreach (BreedData breedData in Bot.Instance.Api.Datacenter.BreedsData.Breeds)
            {
                choices.Add(new(breedData.Name, breedData.Name));
            }

            return Task.FromResult(choices.AsEnumerable());
        }
    }
}
