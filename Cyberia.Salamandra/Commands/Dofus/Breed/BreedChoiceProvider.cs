using Cyberia.Api;

using DSharpPlus.Commands.Trees;

namespace Cyberia.Salamandra.Commands.Dofus.Breed;

public sealed class BreedChoiceProvider : ChoiceProvider
{
    protected override IReadOnlyDictionary<string, object> InternalProvide(CommandParameter parameter)
    {
        return DofusApi.Datacenter.BreedsRepository.Breeds.Values.ToDictionary(x => x.Name.ToString(), x => (object)x.Id);
    }
}
