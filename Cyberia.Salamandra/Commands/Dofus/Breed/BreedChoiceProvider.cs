using Cyberia.Api;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Dofus;

public sealed class BreedChoiceProvider : IChoiceProvider
{
    public Task<IEnumerable<DiscordApplicationCommandOptionChoice>> Provider()
    {
        return Task.FromResult(
            DofusApi.Datacenter.BreedsData.Breeds.Values
                .Select(x => new DiscordApplicationCommandOptionChoice(x.Name, x.Name)));
    }
}
