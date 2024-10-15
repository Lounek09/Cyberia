using Cyberia.Api.Data.Items;
using Cyberia.Salamandra.Formatters;

using DSharpPlus.Entities;

using System.Globalization;

namespace Cyberia.Salamandra.Commands.Dofus.Item;

public static class ItemComponentsBuilder
{
    public static DiscordButtonComponent ItemButtonBuilder(ItemData itemData, int quantity, CultureInfo? culture, bool disable = false)
    {
        return new DiscordButtonComponent(
            DiscordButtonStyle.Success,
            ItemMessageBuilder.GetPacket(itemData.Id, quantity),
            itemData.Name.ToString(culture),
            disable);
    }

    public static DiscordSelectComponent ItemsSelectBuilder(int index, IEnumerable<ItemData> itemsData, CultureInfo? culture, bool disable = false)
    {
        var options = itemsData
            .Take(Constant.MaxSelectOption)
            .Select(x =>
            {
                return new DiscordSelectComponentOption(
                    x.Name.ToString(culture).WithMaxLength(100),
                    ItemMessageBuilder.GetPacket(x.Id),
                    x.GetItemTypeName(culture));
            });

        return new DiscordSelectComponent(
            PacketFormatter.Select(index),
            Translation.Get<BotTranslations>("Select.Item.Placeholder", culture),
            options,
            disable);
    }
}
