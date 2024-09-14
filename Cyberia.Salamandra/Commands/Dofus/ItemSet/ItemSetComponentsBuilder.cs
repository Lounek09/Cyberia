using Cyberia.Api.Data.ItemSets;
using Cyberia.Salamandra.Managers;

using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus.ItemSet;

public static class ItemSetComponentsBuilder
{
    public static DiscordButtonComponent ItemSetButtonBuilder(ItemSetData itemSetData, bool disable = false)
    {
        return new(DiscordButtonStyle.Success, ItemSetMessageBuilder.GetPacket(itemSetData.Id, itemSetData.Effects.Count), itemSetData.Name, disable);
    }

    public static DiscordSelectComponent ItemSetsSelectBuilder(int uniqueIndex, IEnumerable<ItemSetData> itemSetsData, bool disable = false)
    {
        var options = itemSetsData
            .Take(Constant.MaxSelectOption)
            .Select(x =>
            {
                return new DiscordSelectComponentOption(
                    StringExtensions.WithMaxLength(x.Name, 100),
                    ItemSetMessageBuilder.GetPacket(x.Id, x.Effects.Count),
                    x.Id.ToString());
            });

        return new(InteractionManager.SelectComponentPacketBuilder(uniqueIndex), BotTranslations.Select_ItemSet_Placeholder, options, disable);
    }
}
