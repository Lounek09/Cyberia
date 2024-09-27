using Cyberia.Api;
using Cyberia.Api.Data.Items;
using Cyberia.Salamandra.Formatters;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus.Item;

public static class ItemComponentsBuilder
{
    public static DiscordButtonComponent ItemButtonBuilder(ItemData itemData, int quantity = 1, bool disable = false)
    {
        return new(DiscordButtonStyle.Success, ItemMessageBuilder.GetPacket(itemData.Id, quantity), itemData.Name, disable);
    }

    public static DiscordSelectComponent ItemsSelectBuilder(int index, IEnumerable<ItemData> itemsData, bool disable = false)
    {
        var options = itemsData
            .Take(Constant.MaxSelectOption)
            .Select(x =>
            {
                return new DiscordSelectComponentOption(
                    StringExtensions.WithMaxLength(x.Name, 100),
                    ItemMessageBuilder.GetPacket(x.Id),
                    DofusApi.Datacenter.ItemsRepository.GetItemTypeNameById(x.ItemTypeId));
            });

        return new(PacketFormatter.Select(index), BotTranslations.Select_Item_Placeholder, options, disable);
    }
}
