using Cyberia.Api.Data.Monsters;
using Cyberia.Salamandra.Managers;

using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus.Monster;

public static class MonsterComponentsBuilder
{
    public static DiscordButtonComponent MonsterButtonBuilder(MonsterData monsterData, bool disable = false)
    {
        return new(DiscordButtonStyle.Success, MonsterMessageBuilder.GetPacket(monsterData.Id), monsterData.Name, disable);
    }

    public static DiscordSelectComponent MonstersSelectBuilder(int index, IEnumerable<MonsterData> monstersData, bool disable = false)
    {
        var options = monstersData
            .Take(Constant.MaxSelectOption)
            .Select(x =>
            {
                return new DiscordSelectComponentOption(
                    x.Name.WithMaxLength(100),
                    MonsterMessageBuilder.GetPacket(x.Id),
                    x.Id.ToString());
            });

        return new(InteractionManager.SelectComponentPacketBuilder(index), "Sélectionne un monstre pour l'afficher", options, disable);
    }
}
