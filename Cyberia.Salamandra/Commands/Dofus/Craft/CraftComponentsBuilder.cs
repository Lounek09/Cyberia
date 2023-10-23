using Cyberia.Api.DatacenterNS;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public static class CraftComponentsBuilder
    {
        public static DiscordButtonComponent CraftButtonBuilder(CraftData craftData, int qte = 1, bool disable = false)
        {
            return new(ButtonStyle.Success, CraftMessageBuilder.GetPacket(craftData.Id, qte), "Craft", disable);
        }

        public static DiscordSelectComponent CraftsSelectBuilder(int uniqueIndex, List<CraftData> craftsData, int qte = 1, bool disable = false)
        {
            List<DiscordSelectComponentOption> options = new();

            foreach (CraftData craftData in craftsData)
            {
                ItemData? itemData = craftData.GetItemData();
                if (itemData is not null)
                {
                    options.Add(new(itemData.Name.WithMaxLength(100), CraftMessageBuilder.GetPacket(craftData.Id, qte), craftData.Id.ToString()));
                }
            }

            return new(InteractionManager.SelectComponentPacketBuilder(uniqueIndex), "Sélectionne un item pour calculer son craft", options, disable);
        }
    }
}
