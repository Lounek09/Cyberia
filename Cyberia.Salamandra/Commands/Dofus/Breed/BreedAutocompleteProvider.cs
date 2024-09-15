using Cyberia.Api;
using Cyberia.Salamandra.EventHandlers;

using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;

namespace Cyberia.Salamandra.Commands.Dofus.Breed;

public sealed class BreedAutocompleteProvider : IAutoCompleteProvider
{
    private readonly CultureService _cultureService;

    public BreedAutocompleteProvider(CultureService cultureService)
    {
        _cultureService = cultureService;
    }

    public async ValueTask<IReadOnlyDictionary<string, object>> AutoCompleteAsync(AutoCompleteContext ctx)
    {
        await _cultureService.SetCultureAsync(ctx.Interaction);

        return DofusApi.Datacenter.BreedsRepository.Breeds.Values.ToDictionary(x => x.Name.ToString(), x => (object)x.Id);
    }
}
