using Cyberia.Api;
using Cyberia.Api.Data.Crafts;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus;

public static class CraftComponentsBuilder
{
    public static DiscordButtonComponent CraftButtonBuilder(CraftData craftData, int qte = 1, bool disable = false)
    {
        return new(ButtonStyle.Success, CraftMessageBuilder.GetPacket(craftData.Id, qte), "Craft", disable);
    }

    public static DiscordSelectComponent CraftsSelectBuilder(int uniqueIndex, IEnumerable<CraftData> craftsData, int qte = 1, bool disable = false)
    {
        var options = craftsData
            .Take(Constant.MAX_SELECT_OPTION)
            .Select(x =>
            {
                var itemName = DofusApi.Datacenter.ItemsData.GetItemNameById(x.Id);
                return new DiscordSelectComponentOption(
                    itemName.WithMaxLength(100),
                    CraftMessageBuilder.GetPacket(x.Id, qte),
                    x.Id.ToString());
            });

        return new(InteractionManager.SelectComponentPacketBuilder(uniqueIndex), "Sélectionne un item pour calculer son craft", options, disable);
    }
}
