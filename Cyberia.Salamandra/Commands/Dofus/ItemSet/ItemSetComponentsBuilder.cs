using Cyberia.Api.Data.ItemSets;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus;

public static class ItemSetComponentsBuilder
{
    public static DiscordButtonComponent ItemSetButtonBuilder(ItemSetData itemSetData, bool disable = false)
    {
        return new(ButtonStyle.Success, ItemSetMessageBuilder.GetPacket(itemSetData.Id, itemSetData.Effects.Count), itemSetData.Name, disable);
    }

    public static DiscordSelectComponent ItemSetsSelectBuilder(int uniqueIndex, IEnumerable<ItemSetData> itemSetsData, bool disable = false)
    {
        var options = itemSetsData
            .Take(Constant.MAX_SELECT_OPTION)
            .Select(x =>
            {
                return new DiscordSelectComponentOption(
                    x.Name.WithMaxLength(100),
                    ItemSetMessageBuilder.GetPacket(x.Id, x.Effects.Count),
                    x.Id.ToString());
            });

        return new(InteractionManager.SelectComponentPacketBuilder(uniqueIndex), "Sélectionne une panoplie pour l'afficher", options, disable);
    }
}
