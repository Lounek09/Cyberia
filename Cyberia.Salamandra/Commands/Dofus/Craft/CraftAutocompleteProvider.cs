using Cyberia.Api.Data;
using Cyberia.Salamandra.Services;

using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus.Craft;

public sealed class CraftAutocompleteProvider : IAutoCompleteProvider
{
    private readonly ICultureService _cultureService;
    private readonly DofusDatacenter _dofusDatacenter;

    public CraftAutocompleteProvider(ICultureService cultureService, DofusDatacenter dofusDatacenter)
    {
        _cultureService = cultureService;
        _dofusDatacenter = dofusDatacenter;        
    }

    public async ValueTask<IEnumerable<DiscordAutoCompleteChoice>> AutoCompleteAsync(AutoCompleteContext ctx)
    {
        var culture = await _cultureService.GetCultureAsync(ctx.Interaction);

        return _dofusDatacenter.CraftsRepository.GetCraftsDataByItemName(ctx.UserInput ?? string.Empty, culture)
           .Take(Constant.MaxChoice)
           .Select(x =>
           {
               var itemName = _dofusDatacenter.ItemsRepository.GetItemNameById(x.Id, culture);
               return new DiscordAutoCompleteChoice($"{itemName.WithMaxLength(90)} ({x.Id})", x.Id.ToString());
           });
    }
}
