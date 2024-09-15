using Cyberia.Api;
using Cyberia.Salamandra.Services;

using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;

namespace Cyberia.Salamandra.Commands.Dofus.Incarnation;

public sealed class IncarnationAutocompleteProvider : IAutoCompleteProvider
{
    private readonly CultureService _cultureService;

    public IncarnationAutocompleteProvider(CultureService cultureService)
    {
        _cultureService = cultureService;
    }

    public async ValueTask<IReadOnlyDictionary<string, object>> AutoCompleteAsync(AutoCompleteContext ctx)
    {
        using CultureScope scope = new(await _cultureService.GetCultureAsync(ctx.Interaction));

        return DofusApi.Datacenter.IncarnationsRepository.GetIncarnationsDataByItemName(ctx.UserInput)
            .Take(Constant.MaxChoice)
           .ToDictionary(x =>
               {
                   var itemName = DofusApi.Datacenter.ItemsRepository.GetItemNameById(x.Id);
                   return $"{itemName.WithMaxLength(90)} ({x.Id})";
               },
               x => (object)x.Id.ToString());
    }
}
