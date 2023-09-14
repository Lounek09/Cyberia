using Cyberia.Api.DatacenterNS;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public static class ItemComponentsBuilder
    {
        public static DiscordButtonComponent ItemButtonBuilder(Item item, int craftQte = 1, bool disable = false)
        {
            return new(ButtonStyle.Success, ItemMessageBuilder.GetPacket(item.Id, craftQte), item.Name, disable);
        }

        public static DiscordSelectComponent ItemsSelectBuilder(int index, List<Item> items, bool disable = false)
        {
            IEnumerable<DiscordSelectComponentOption> options = items.Select(x => new DiscordSelectComponentOption(x.Name.WithMaxLength(100), ItemMessageBuilder.GetPacket(x.Id), Bot.Instance.Api.Datacenter.ItemsData.GetItemTypeNameById(x.ItemTypeId)));

            return new(InteractionManager.SelectComponentPacketBuilder(index), "Sélectionne un item pour l'afficher", options, disable);
        }
    }
}
