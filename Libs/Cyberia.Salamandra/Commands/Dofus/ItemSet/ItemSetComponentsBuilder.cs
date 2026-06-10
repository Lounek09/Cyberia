using Cyberia.Api.Data.ItemSets;
using Cyberia.Salamandra.Formatters;

using DSharpPlus.Entities;

using System.Globalization;

namespace Cyberia.Salamandra.Commands.Dofus.ItemSet;

public static class ItemSetComponentsBuilder
{
    public static DiscordButtonComponent ItemSetButtonBuilder(ItemSetData itemSetData, CultureInfo? culture, bool disable = false)
    {
        return new DiscordButtonComponent(
            DiscordButtonStyle.Success,
            ItemSetMessageBuilder.GetPacket(itemSetData.Id, itemSetData.Effects.Count),
            itemSetData.Name.ToString(culture),
            disable);
    }

    public static DiscordSelectComponent ItemSetsSelectBuilder(int uniqueIndex, IEnumerable<ItemSetData> itemSetsData, CultureInfo? culture, bool disable = false)
    {
        var options = itemSetsData
            .Take(Constant.MaxSelectOption)
            .Select(x =>
            {
                return new DiscordSelectComponentOption(
                    x.Name.ToString(culture).WithMaxLength(100),
                    ItemSetMessageBuilder.GetPacket(x.Id, x.Effects.Count),
                    x.Id.ToString());
            });

        return new DiscordSelectComponent(
            PacketFormatter.Select(uniqueIndex),
            Translation.Get<BotTranslations>("Select.ItemSet.Placeholder", culture),
            options,
            disable);
    }
}
