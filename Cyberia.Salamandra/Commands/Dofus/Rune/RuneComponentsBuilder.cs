using Cyberia.Api;
using Cyberia.Api.Data.Items;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus;

public static class RuneComponentsBuilder
{
    public static DiscordButtonComponent RuneItemButtonBuilder(ItemData itemData, int qte = 1, bool disable = false)
    {
        return new(ButtonStyle.Success, RuneItemMessageBuilder.GetPacket(itemData.Id, qte), "Calculateur de runes", disable);
    }

    public static DiscordSelectComponent ItemsSelectBuilder(int index, List<ItemData> itemsData, int qte, bool disable = false)
    {
        var options = itemsData.Select(x =>
            {
                return new DiscordSelectComponentOption(x.Name.WithMaxLength(100),
                    RuneItemMessageBuilder.GetPacket(x.Id, qte),
                    DofusApi.Datacenter.ItemsData.GetItemTypeNameById(x.ItemTypeId));
            });

        return new(InteractionManager.SelectComponentPacketBuilder(index),
            "Sélectionne un item pour calculer les runes obtenable",
            options,
            disable);
    }
}
