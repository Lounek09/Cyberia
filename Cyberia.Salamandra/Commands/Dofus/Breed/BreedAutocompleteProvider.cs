using Cyberia.Api;
using Cyberia.Salamandra.Services;

using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus.Breed;

public sealed class BreedAutocompleteProvider : IAutoCompleteProvider //TODO: Use a IChoiceProvider with localized names
{
    private readonly CultureService _cultureService;

    public BreedAutocompleteProvider(CultureService cultureService)
    {
        _cultureService = cultureService;
    }

    public async ValueTask<IEnumerable<DiscordAutoCompleteChoice>> AutoCompleteAsync(AutoCompleteContext ctx)
    {
        using CultureScope scope = new(await _cultureService.GetCultureAsync(ctx.Interaction));

        return DofusApi.Datacenter.BreedsRepository.Breeds.Values.Select(x =>
        {
            return new DiscordAutoCompleteChoice(x.Name, x.Id);
        })
        .ToList();
    }
}
