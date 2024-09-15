using Cyberia.Api;
using Cyberia.Salamandra.EventHandlers;

using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;

namespace Cyberia.Salamandra.Commands.Dofus.Craft;

public sealed class CraftAutocompleteProvider : IAutoCompleteProvider
{
    private readonly CultureService _cultureService;

    public CraftAutocompleteProvider(CultureService cultureService)
    {
        _cultureService = cultureService;
    }

    public async ValueTask<IReadOnlyDictionary<string, object>> AutoCompleteAsync(AutoCompleteContext ctx)
    {
        await _cultureService.SetCultureAsync(ctx.Interaction);

        return DofusApi.Datacenter.CraftsRepository.GetCraftsDataByItemName(ctx.UserInput)
           .Take(Constant.MaxChoice)
           .ToDictionary(x =>
               {
                   var itemName = DofusApi.Datacenter.ItemsRepository.GetItemNameById(x.Id);
                   return $"{itemName.WithMaxLength(90)} ({x.Id})";
               },
               x => (object)x.Id.ToString());
    }
}
