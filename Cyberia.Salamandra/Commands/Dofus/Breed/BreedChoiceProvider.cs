using Cyberia.Api;

using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;
using DSharpPlus.Commands.Trees;

namespace Cyberia.Salamandra.Commands.Dofus.Breed;

public sealed class BreedChoiceProvider : IChoiceProvider
{
    public ValueTask<IReadOnlyDictionary<string, object>> ProvideAsync(CommandParameter parameter)
    {
        return new ValueTask<IReadOnlyDictionary<string, object>>(
            DofusApi.Datacenter.BreedsRepository.Breeds.Values.ToDictionary(x => x.Name, x => (object)x.Id));
    }
}
