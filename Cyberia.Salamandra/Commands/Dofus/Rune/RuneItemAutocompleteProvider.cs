using Cyberia.Api;
using Cyberia.Api.Factories.Effects.Templates;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Dofus;

public sealed class RuneItemAutocompleteProvider : AutocompleteProvider
{
    public override Task<IEnumerable<DiscordAutoCompleteChoice>> Provider(AutocompleteContext ctx)
    {
        var value = ctx.OptionValue.ToString();
        if (value is null || value.Length < MIN_LENGTH_AUTOCOMPLETE)
        {
            return Task.FromResult(Enumerable.Empty<DiscordAutoCompleteChoice>());
        }

        List<DiscordAutoCompleteChoice> choices = [];

        var itemsData = DofusApi.Datacenter.ItemsData.GetItemsDataByName(value)
            .Where(x =>
            {
                var itemStatsData = x.GetItemStatsData();
                return itemStatsData is not null && itemStatsData.Effects.OfType<IRuneGeneratorEffect>().Any();
            })
            .Take(MAX_AUTOCOMPLETE_CHOICE);

        foreach (var itemData in itemsData)
        {
            choices.Add(new($"{itemData.Name.WithMaxLength(90)} ({itemData.Id})", itemData.Id.ToString()));
        }

        return Task.FromResult(choices.AsEnumerable());
    }
}
