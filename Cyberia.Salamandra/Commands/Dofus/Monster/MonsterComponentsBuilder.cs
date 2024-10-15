using Cyberia.Api.Data.Monsters;
using Cyberia.Salamandra.Formatters;

using DSharpPlus.Entities;

using System.Globalization;

namespace Cyberia.Salamandra.Commands.Dofus.Monster;

public static class MonsterComponentsBuilder
{
    public static DiscordButtonComponent MonsterButtonBuilder(MonsterData monsterData, CultureInfo? culture, bool disable = false)
    {
        return new DiscordButtonComponent(
            DiscordButtonStyle.Success,
            MonsterMessageBuilder.GetPacket(monsterData.Id),
            monsterData.Name.ToString(culture),
            disable);
    }

    public static DiscordSelectComponent MonstersSelectBuilder(int index, IEnumerable<MonsterData> monstersData, CultureInfo? culture, bool disable = false)
    {
        var options = monstersData
            .Take(Constant.MaxSelectOption)
            .Select(x =>
            {
                return new DiscordSelectComponentOption(
                    x.Name.ToString(culture).WithMaxLength(100),
                    MonsterMessageBuilder.GetPacket(x.Id),
                    x.Id.ToString());
            });

        return new DiscordSelectComponent(
            PacketFormatter.Select(index),
            Translation.Get<BotTranslations>("Select.Monster.Placeholder", culture),
            options,
            disable);
    }
}
