using Cyberia.Api.Data.Items;
using Cyberia.Salamandra.Formatters;

using DSharpPlus.Entities;

using System.Globalization;

namespace Cyberia.Salamandra.Commands.Dofus.Rune;

public static class RuneComponentsBuilder
{
    public static DiscordButtonComponent RuneItemButtonBuilder(ItemData itemData, int quantity, CultureInfo? culture, bool disable = false)
    {
        return new DiscordButtonComponent(
            DiscordButtonStyle.Success,
            RuneItemMessageBuilder.GetPacket(itemData.Id, quantity),
            Translation.Get<BotTranslations>("Button.RuneItem", culture),
            disable);
    }

    public static DiscordSelectComponent ItemsSelectBuilder(int index, IEnumerable<ItemData> itemsData, int quantity, CultureInfo? culture, bool disable = false)
    {
        var options = itemsData
            .Take(Constant.MaxSelectOption)
            .Select(x =>
            {
                return new DiscordSelectComponentOption(
                    x.Name.ToString(culture).WithMaxLength(100),
                    RuneItemMessageBuilder.GetPacket(x.Id, quantity),
                    x.GetItemTypeName(culture));
            });

        return new DiscordSelectComponent(
            PacketFormatter.Select(index),
            Translation.Get<BotTranslations>("Select.RuneItem.Placeholder", culture),
            options,
            disable);
    }
}
