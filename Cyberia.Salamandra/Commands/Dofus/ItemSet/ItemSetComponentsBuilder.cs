using Cyberia.Api.DatacenterNS;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public static class ItemSetComponentsBuilder
    {
        public static DiscordButtonComponent ItemSetButtonBuilder(ItemSet itemSet, bool disable = false)
        {
            return new(ButtonStyle.Success, ItemSetMessageBuilder.GetPacket(itemSet.Id), itemSet.Name, disable);
        }

        public static DiscordSelectComponent ItemSetsSelectBuilder(int uniqueIndex, List<ItemSet> itemSets, bool disable = false)
        {
            IEnumerable<DiscordSelectComponentOption> options = itemSets.Select(x => new DiscordSelectComponentOption(x.Name.WithMaxLength(100), ItemSetMessageBuilder.GetPacket(x.Id), x.Id.ToString()));
            
            return new(InteractionManager.SelectComponentPacketBuilder(uniqueIndex), "Sélectionne une panoplie pour l'afficher", options, disable);
        }
    }
}
