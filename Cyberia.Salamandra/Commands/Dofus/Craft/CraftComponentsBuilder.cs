using Cyberia.Api;
using Cyberia.Api.Data.Crafts;
using Cyberia.Salamandra.Managers;

using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus.Craft;

public static class CraftComponentsBuilder
{
    public static DiscordButtonComponent CraftButtonBuilder(CraftData craftData, int quantity = 1, bool disable = false)
    {
        return new(DiscordButtonStyle.Success, CraftMessageBuilder.GetPacket(craftData.Id, quantity), BotTranslations.Button_Craft, disable);
    }

    public static DiscordSelectComponent CraftsSelectBuilder(int uniqueIndex, IEnumerable<CraftData> craftsData, int quantity = 1, bool disable = false)
    {
        var options = craftsData
            .Take(Constant.MaxSelectOption)
            .Select(x =>
            {
                var itemName = DofusApi.Datacenter.ItemsRepository.GetItemNameById(x.Id);
                return new DiscordSelectComponentOption(
                    itemName.WithMaxLength(100),
                    CraftMessageBuilder.GetPacket(x.Id, quantity),
                    x.Id.ToString());
            });

        return new(InteractionManager.SelectComponentPacketBuilder(uniqueIndex), BotTranslations.Select_Craft_Placeholder, options, disable);
    }
}
