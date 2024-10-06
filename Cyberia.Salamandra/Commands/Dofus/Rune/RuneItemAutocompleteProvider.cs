using Cyberia.Api;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Salamandra.Services;

using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus.Rune;

public sealed class RuneItemAutocompleteProvider : IAutoCompleteProvider
{
    private readonly CultureService _cultureService;

    public RuneItemAutocompleteProvider(CultureService cultureService)
    {
        _cultureService = cultureService;
    }

    public async ValueTask<IEnumerable<DiscordAutoCompleteChoice>> AutoCompleteAsync(AutoCompleteContext ctx)
    {
        using CultureScope scope = new(await _cultureService.GetCultureAsync(ctx.Interaction));

        return DofusApi.Datacenter.ItemsRepository.GetItemsDataByName(ctx.UserInput ?? string.Empty)
            .Where(x =>
            {
                var itemStatsData = x.GetItemStatsData();
                return itemStatsData is not null && itemStatsData.Effects.Any(x => x is IRuneGeneratorEffect);
            })
            .Take(Constant.MaxChoice)
            .Select(x =>
            {
                return new DiscordAutoCompleteChoice($"{StringExtensions.WithMaxLength(x.Name, 90)} ({x.Id})", x.Id.ToString());
            })
           .ToList();
    }
}
