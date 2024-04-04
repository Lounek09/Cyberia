using Cyberia.Api.Data.Quests;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus;

public static class QuestComponentsBuilder
{
    public static DiscordButtonComponent QuestButtonBuilder(QuestData questData, bool disable = false)
    {
        return new(ButtonStyle.Primary, QuestMessageBuilder.GetPacket(questData.Id), questData.Name, disable);
    }

    public static DiscordSelectComponent QuestsSelectBuilder(int index, IEnumerable<QuestData> questsData, bool disable = false)
    {
        var options = questsData
            .Take(Constant.MaxSelectOption)
            .Select(x =>
            {
                return new DiscordSelectComponentOption(
                    x.Name.WithMaxLength(100),
                    QuestMessageBuilder.GetPacket(x.Id),
                    x.Id.ToString());
            });

        return new(InteractionManager.SelectComponentPacketBuilder(index), "Sélectionne une quête pour l'afficher", options, disable);
    }
}
