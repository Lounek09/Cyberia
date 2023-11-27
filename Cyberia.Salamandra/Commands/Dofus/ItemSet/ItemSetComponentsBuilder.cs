using Cyberia.Api.Data;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus;

public static class ItemSetComponentsBuilder
{
    public static DiscordButtonComponent ItemSetButtonBuilder(ItemSetData itemSetData, bool disable = false)
    {
        return new(ButtonStyle.Success, ItemSetMessageBuilder.GetPacket(itemSetData.Id), itemSetData.Name, disable);
    }

    public static DiscordSelectComponent ItemSetsSelectBuilder(int uniqueIndex, List<ItemSetData> itemSetsData, bool disable = false)
    {
        var options = itemSetsData.Select(x => new DiscordSelectComponentOption(x.Name.WithMaxLength(100), ItemSetMessageBuilder.GetPacket(x.Id), x.Id.ToString()));

        return new(InteractionManager.SelectComponentPacketBuilder(uniqueIndex), "Sélectionne une panoplie pour l'afficher", options, disable);
    }
}
