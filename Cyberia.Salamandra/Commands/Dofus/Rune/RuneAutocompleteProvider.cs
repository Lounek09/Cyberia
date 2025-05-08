using Cyberia.Api.Data;
using Cyberia.Salamandra.Services;

using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus.Rune;

public sealed class RuneAutocompleteProvider : IAutoCompleteProvider
{
    private readonly CultureService _cultureService;
    private readonly DofusDatacenter _dofusDatacenter;

    public RuneAutocompleteProvider(CultureService cultureService, DofusDatacenter dofusDatacenter)
    {
        _cultureService = cultureService;
        _dofusDatacenter = dofusDatacenter;
    }

    public async ValueTask<IEnumerable<DiscordAutoCompleteChoice>> AutoCompleteAsync(AutoCompleteContext ctx)
    {
        var culture = await _cultureService.GetCultureAsync(ctx.Interaction);

        return _dofusDatacenter.RunesRepository.GetRunesDataByItemName(ctx.UserInput ?? string.Empty)
            .Take(Constant.MaxChoice)
            .Select(x =>
            {
                var itemName = _dofusDatacenter.ItemsRepository.GetItemNameById(x.BaRuneItemId, culture);
                return new DiscordAutoCompleteChoice(itemName.WithMaxLength(100), x.Id);
            });
    }
}
