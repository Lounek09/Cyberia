using Cyberia.Api;
using Cyberia.Api.Data.Items;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus;

public static class ItemComponentsBuilder
{
    public static DiscordButtonComponent ItemButtonBuilder(ItemData itemData, int craftQte = 1, bool disable = false)
    {
        return new(ButtonStyle.Success, ItemMessageBuilder.GetPacket(itemData.Id, craftQte), itemData.Name, disable);
    }

    public static DiscordSelectComponent ItemsSelectBuilder(int index, IEnumerable<ItemData> itemsData, bool disable = false)
    {
        var options = itemsData
            .Take(Constant.MAX_SELECT_OPTION)
            .Select(x =>
            {
                return new DiscordSelectComponentOption(
                    x.Name.WithMaxLength(100),
                    ItemMessageBuilder.GetPacket(x.Id),
                    DofusApi.Datacenter.ItemsData.GetItemTypeNameById(x.ItemTypeId));
            });

        return new(InteractionManager.SelectComponentPacketBuilder(index), "Sélectionne un item pour l'afficher", options, disable);
    }
}
