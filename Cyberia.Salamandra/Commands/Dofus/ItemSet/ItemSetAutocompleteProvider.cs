using Cyberia.Api;
using Cyberia.Salamandra.Services;

using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;

namespace Cyberia.Salamandra.Commands.Dofus.ItemSet;

public sealed class ItemSetAutocompleteProvider : IAutoCompleteProvider
{
    private readonly CultureService _cultureService;

    public ItemSetAutocompleteProvider(CultureService cultureService)
    {
        _cultureService = cultureService;
    }

    public async ValueTask<IReadOnlyDictionary<string, object>> AutoCompleteAsync(AutoCompleteContext ctx)
    {
        await _cultureService.SetCultureAsync(ctx.Interaction);

        return DofusApi.Datacenter.ItemSetsRepository.GetItemSetsDataByName(ctx.UserInput)
            .Take(Constant.MaxChoice)
            .ToDictionary(x => $"{ExtendString.WithMaxLength(x.Name, 90)} ({x.Id})", x => (object)x.Id.ToString());
    }
}
