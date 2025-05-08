using Cyberia.Api.Data;
using Cyberia.Salamandra.Services;

using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus.ItemSet;

public sealed class ItemSetAutocompleteProvider : IAutoCompleteProvider
{
    private readonly ICultureService _cultureService;
    private readonly DofusDatacenter _dofusDatacenter;

    public ItemSetAutocompleteProvider(ICultureService cultureService, DofusDatacenter dofusDatacenter)
    {
        _cultureService = cultureService;
        _dofusDatacenter = dofusDatacenter;
    }

    public async ValueTask<IEnumerable<DiscordAutoCompleteChoice>> AutoCompleteAsync(AutoCompleteContext ctx)
    {
        var culture = await _cultureService.GetCultureAsync(ctx.Interaction);

        return _dofusDatacenter.ItemSetsRepository.GetItemSetsDataByName(ctx.UserInput ?? string.Empty, culture)
            .Take(Constant.MaxChoice)
            .Select(x =>
            {
                return new DiscordAutoCompleteChoice($"{x.Name.ToString(culture).WithMaxLength(90)} ({x.Id})", x.Id.ToString());
            });
    }
}
