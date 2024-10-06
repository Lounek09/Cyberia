using Cyberia.Api;
using Cyberia.Salamandra.Services;

using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus.Craft;

public sealed class CraftAutocompleteProvider : IAutoCompleteProvider
{
    private readonly CultureService _cultureService;

    public CraftAutocompleteProvider(CultureService cultureService)
    {
        _cultureService = cultureService;
    }

    public async ValueTask<IEnumerable<DiscordAutoCompleteChoice>> AutoCompleteAsync(AutoCompleteContext ctx)
    {
        using CultureScope scope = new(await _cultureService.GetCultureAsync(ctx.Interaction));

        return DofusApi.Datacenter.CraftsRepository.GetCraftsDataByItemName(ctx.UserInput ?? string.Empty)
           .Take(Constant.MaxChoice)
           .Select(x =>
           {
               var itemName = DofusApi.Datacenter.ItemsRepository.GetItemNameById(x.Id);
               return new DiscordAutoCompleteChoice($"{itemName.WithMaxLength(90)} ({x.Id})", x.Id.ToString());
           })
           .ToList();
    }
}
