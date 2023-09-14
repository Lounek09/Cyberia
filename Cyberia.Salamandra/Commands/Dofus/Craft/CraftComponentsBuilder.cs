using Cyberia.Api.DatacenterNS;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public static class CraftComponentsBuilder
    {
        public static DiscordButtonComponent CraftButtonBuilder(Craft craft, int qte = 1, bool disable = false)
        {
            return new(ButtonStyle.Success, CraftMessageBuilder.GetPacket(craft.Id, qte), "Craft", disable);
        }

        public static DiscordSelectComponent CraftsSelectBuilder(int uniqueIndex, List<Craft> crafts, int qte = 1, bool disable = false)
        {
            List<DiscordSelectComponentOption> options = new();

            foreach (Craft craft in crafts)
            {
                Item? item = craft.GetItem();
                if (item is not null)
                    options.Add(new(item.Name.WithMaxLength(100), CraftMessageBuilder.GetPacket(craft.Id, qte), craft.Id.ToString()));
            }

            return new(InteractionManager.SelectComponentPacketBuilder(uniqueIndex), "Sélectionne un item pour calculer son craft", options, disable);
        }
    }
}
