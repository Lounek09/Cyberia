using Cyberia.Api;
using Cyberia.Api.Data.Items;
using Cyberia.Salamandra.Managers;

using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus.Item;

public static class ItemComponentsBuilder
{
    public static DiscordButtonComponent ItemButtonBuilder(ItemData itemData, int craftQte = 1, bool disable = false)
    {
        return new(DiscordButtonStyle.Success, ItemMessageBuilder.GetPacket(itemData.Id, craftQte), itemData.Name, disable);
    }

    public static DiscordSelectComponent ItemsSelectBuilder(int index, IEnumerable<ItemData> itemsData, bool disable = false)
    {
        var options = itemsData
            .Take(Constant.MaxSelectOption)
            .Select(x =>
            {
                return new DiscordSelectComponentOption(
                    x.Name.WithMaxLength(100),
                    ItemMessageBuilder.GetPacket(x.Id),
                    DofusApi.Datacenter.ItemsRepository.GetItemTypeNameById(x.ItemTypeId));
            });

        return new(InteractionManager.SelectComponentPacketBuilder(index), "Sélectionne un item pour l'afficher", options, disable);
    }
}
