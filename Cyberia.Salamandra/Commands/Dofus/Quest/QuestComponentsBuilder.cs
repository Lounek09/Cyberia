using Cyberia.Api.DatacenterNS;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public static class QuestComponentsBuilder
    {
        public static DiscordButtonComponent QuestButtonBuilder(QuestData questData, bool disable = false)
        {
            return new(ButtonStyle.Primary, QuestMessageBuilder.GetPacket(questData.Id), questData.Name, disable);
        }

        public static DiscordSelectComponent QuestsSelectBuilder(int index, List<QuestData> questsData, bool disable = false)
        {
            IEnumerable<DiscordSelectComponentOption> options = questsData.Select(x => new DiscordSelectComponentOption(x.Name.WithMaxLength(100), QuestMessageBuilder.GetPacket(x.Id), x.Id.ToString()));

            return new(InteractionManager.SelectComponentPacketBuilder(index), "Sélectionne une quête pour l'afficher", options, disable);
        }
    }
}
