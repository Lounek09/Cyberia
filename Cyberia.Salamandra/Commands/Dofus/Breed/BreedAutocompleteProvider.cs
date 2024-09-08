using Cyberia.Api;

using DSharpPlus.Commands.Processors.SlashCommands;

namespace Cyberia.Salamandra.Commands.Dofus.Breed;

public sealed class BreedAutocompleteProvider : CultureAutoCompleteProvider
{
    protected override IReadOnlyDictionary<string, object> InternalAutoComplete(AutoCompleteContext ctx)
    {
        return DofusApi.Datacenter.BreedsRepository.Breeds.Values.ToDictionary(x => x.Name.ToString(), x => (object)x.Id);
    }
}
