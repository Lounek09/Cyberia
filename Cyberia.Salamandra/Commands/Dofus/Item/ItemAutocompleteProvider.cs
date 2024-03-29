using Cyberia.Api;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Dofus;

public sealed class ItemAutocompleteProvider : AutocompleteProvider
{
    protected override IEnumerable<DiscordAutoCompleteChoice> InternalProvider(AutocompleteContext ctx, string value)
    {
        return DofusApi.Datacenter.ItemsData.GetItemsDataByName(value)
            .Take(Constant.MAX_CHOICE)
            .Select(x => new DiscordAutoCompleteChoice($"{x.Name.WithMaxLength(90)} ({x.Id})", x.Id.ToString()));
    }
}
