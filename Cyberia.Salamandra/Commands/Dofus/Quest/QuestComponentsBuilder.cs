using Cyberia.Api.Data.Quests;
using Cyberia.Salamandra.Formatters;

using DSharpPlus.Entities;

using System.Globalization;

namespace Cyberia.Salamandra.Commands.Dofus.Quest;

public static class QuestComponentsBuilder
{
    public static DiscordButtonComponent QuestButtonBuilder(QuestData questData, CultureInfo? culture, bool disable = false)
    {
        return new DiscordButtonComponent(
            DiscordButtonStyle.Primary,
            QuestMessageBuilder.GetPacket(questData.Id, 0, null),
            questData.Name.ToString(culture),
            disable);
    }

    public static DiscordSelectComponent QuestsSelectBuilder(int index, IEnumerable<QuestData> questsData, CultureInfo? culture, bool disable = false)
    {
        var options = questsData
            .Take(Constant.MaxSelectOption)
            .Select(x =>
            {
                return new DiscordSelectComponentOption(
                    x.Name.ToString(culture).WithMaxLength(100),
                    QuestMessageBuilder.GetPacket(x.Id, 0, null),
                    x.Id.ToString());
            });

        return new DiscordSelectComponent(
            PacketFormatter.Select(index),
            Translation.Get<BotTranslations>("Select.Quest.Placeholder", culture),
            options,
            disable);
    }
}
