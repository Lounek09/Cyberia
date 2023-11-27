using Cyberia.Api;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Dofus;

public sealed class BreedChoiceProvider : IChoiceProvider
{
    public Task<IEnumerable<DiscordApplicationCommandOptionChoice>> Provider()
    {
        List<DiscordApplicationCommandOptionChoice> choices = [];

        foreach (var breedData in DofusApi.Datacenter.BreedsData.Breeds.Values)
        {
            choices.Add(new(breedData.Name, breedData.Name));
        }

        return Task.FromResult(choices.AsEnumerable());
    }
}
