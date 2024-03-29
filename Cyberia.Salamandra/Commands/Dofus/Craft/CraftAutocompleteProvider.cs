using Cyberia.Api;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Dofus;

public sealed class CraftAutocompleteProvider : AutocompleteProvider
{
    protected override IEnumerable<DiscordAutoCompleteChoice> InternalProvider(AutocompleteContext ctx, string value)
    {
        return DofusApi.Datacenter.CraftsData.GetCraftsDataByItemName(value)
            .Take(Constant.MAX_CHOICE)
            .Select(x =>
            {
                var itemName = DofusApi.Datacenter.ItemsData.GetItemNameById(x.Id);
                return new DiscordAutoCompleteChoice($"{itemName.WithMaxLength(90)} ({x.Id})", x.Id.ToString());
            });
    }
}
