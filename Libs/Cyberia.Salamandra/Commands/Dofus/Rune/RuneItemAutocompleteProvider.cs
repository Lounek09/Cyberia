using Cyberia.Api.Data;
using Cyberia.Api.Factories.Effects.Interfaces;
using Cyberia.Salamandra.Services;

using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus.Rune;

public sealed class RuneItemAutocompleteProvider : IAutoCompleteProvider
{
    private readonly ICultureService _cultureService;
    private readonly DofusDatacenter _dofusDatacenter;

    public RuneItemAutocompleteProvider(ICultureService cultureService, DofusDatacenter dofusDatacenter)
    {
        _cultureService = cultureService;
        _dofusDatacenter = dofusDatacenter;
    }

    public ValueTask<IEnumerable<DiscordAutoCompleteChoice>> AutoCompleteAsync(AutoCompleteContext ctx)
    {
        var culture = _cultureService.GetCulture(ctx.Interaction);

        var choices = _dofusDatacenter.ItemsRepository.GetItemsDataByName(ctx.UserInput ?? string.Empty, culture)
            .Where(x =>
            {
                var itemStatsData = x.GetItemStatsData();
                return itemStatsData is not null && itemStatsData.Effects.Any(x => x is IRuneGeneratorEffect);
            })
            .Take(Constant.MaxChoice)
            .Select(x =>
            {
                return new DiscordAutoCompleteChoice($"{x.Name.ToString(culture).WithMaxLength(90)} ({x.Id})", x.Id.ToString());
            });

        return ValueTask.FromResult(choices);
    }
}
