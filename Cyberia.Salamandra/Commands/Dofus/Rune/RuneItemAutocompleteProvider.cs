using Cyberia.Api;
using Cyberia.Api.Factories.Effects.Templates;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Dofus;

public sealed class RuneItemAutocompleteProvider : AutocompleteProvider
{
    protected override IEnumerable<DiscordAutoCompleteChoice> InternalProvider(AutocompleteContext ctx, string value)
    {
        return DofusApi.Datacenter.ItemsData.GetItemsDataByName(value)
            .Where(x =>
            {
                var itemStatsData = x.GetItemStatsData();
                return itemStatsData is not null && itemStatsData.Effects.Any(x => x is IRuneGeneratorEffect);
            })
            .Take(Constant.MAX_CHOICE)
            .Select(x => new DiscordAutoCompleteChoice($"{x.Name.WithMaxLength(90)} ({x.Id})", x.Id.ToString()));
    }
}
