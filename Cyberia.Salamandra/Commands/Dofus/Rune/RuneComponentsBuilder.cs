using Cyberia.Api;
using Cyberia.Api.Data.Items;
using Cyberia.Salamandra.Managers;

using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus.Rune;

public static class RuneComponentsBuilder
{
    public static DiscordButtonComponent RuneItemButtonBuilder(ItemData itemData, int quantity = 1, bool disable = false)
    {
        return new(DiscordButtonStyle.Success, RuneItemMessageBuilder.GetPacket(itemData.Id, quantity), BotTranslations.Button_RuneItem, disable);
    }

    public static DiscordSelectComponent ItemsSelectBuilder(int index, IEnumerable<ItemData> itemsData, int quantity, bool disable = false)
    {
        var options = itemsData
            .Take(Constant.MaxSelectOption)
            .Select(x =>
            {
                return new DiscordSelectComponentOption(
                    StringExtensions.WithMaxLength(x.Name, 100),
                    RuneItemMessageBuilder.GetPacket(x.Id, quantity),
                    DofusApi.Datacenter.ItemsRepository.GetItemTypeNameById(x.ItemTypeId));
            });

        return new(PacketManager.SelectComponentBuilder(index), BotTranslations.Select_RuneItem_Splaceholder, options, disable);
    }
}
