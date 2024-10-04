using Cyberia.Api.Data.Quests;
using Cyberia.Salamandra.Formatters;

using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus.Quest;

public static class QuestComponentsBuilder
{
    public static DiscordButtonComponent QuestButtonBuilder(QuestData questData, bool disable = false)
    {
        return new(DiscordButtonStyle.Primary, QuestMessageBuilder.GetPacket(questData.Id), questData.Name, disable);
    }

    public static DiscordSelectComponent QuestsSelectBuilder(int index, IEnumerable<QuestData> questsData, bool disable = false)
    {
        var options = questsData
            .Take(Constant.MaxSelectOption)
            .Select(x =>
            {
                return new DiscordSelectComponentOption(
                    StringExtensions.WithMaxLength(x.Name, 100),
                    QuestMessageBuilder.GetPacket(x.Id),
                    x.Id.ToString());
            });

        return new(PacketFormatter.Select(index), BotTranslations.Select_Quest_Placeholder, options, disable);
    }
}
