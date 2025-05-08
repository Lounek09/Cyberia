using Cyberia.Api.Data;
using Cyberia.Salamandra.Services;

using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus.Breed;

public sealed class BreedAutocompleteProvider : IAutoCompleteProvider //TODO: Use a IChoiceProvider with localized names
{
    private readonly CultureService _cultureService;
    private readonly DofusDatacenter _dofusDatacenter;

    public BreedAutocompleteProvider(CultureService cultureService, DofusDatacenter dofusDatacenter)
    {
        _cultureService = cultureService;
        _dofusDatacenter = dofusDatacenter;
    }

    public async ValueTask<IEnumerable<DiscordAutoCompleteChoice>> AutoCompleteAsync(AutoCompleteContext ctx)
    {
        var culture = await _cultureService.GetCultureAsync(ctx.Interaction);

        return _dofusDatacenter.BreedsRepository.Breeds.Values.Select(x =>
        {
            return new DiscordAutoCompleteChoice(x.Name.ToString(culture), x.Id);
        });
    }
}
