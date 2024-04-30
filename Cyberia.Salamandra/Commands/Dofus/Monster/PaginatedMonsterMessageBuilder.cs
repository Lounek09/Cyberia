using Cyberia.Api;
using Cyberia.Api.Data.Monsters;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus.Monster;

public sealed class PaginatedMonsterMessageBuilder : PaginatedMessageBuilder<MonsterData>
{
    public const string PacketHeader = "PM";
    public const int PacketVersion = 1;

    private readonly string _search;

    public PaginatedMonsterMessageBuilder(List<MonsterData> monstersData, string search, int selectedPageIndex = 0)
        : base(EmbedCategory.Bestiary, "Bestiaire", "Plusieurs monstres trouvés :", monstersData, selectedPageIndex)
    {
        _search = search;
    }

    public static PaginatedMonsterMessageBuilder? Create(int version, string[] parameters)
    {
        if (version == PacketVersion &&
            parameters.Length > 2 &&
            int.TryParse(parameters[1], out var selectedPageIndex))
        {
            var monstersData = DofusApi.Datacenter.MonstersData.GetMonstersDataByName(parameters[2]).ToList();
            if (monstersData.Count > 0)
            {
                return new PaginatedMonsterMessageBuilder(monstersData, parameters[2], selectedPageIndex);
            }
        }

        return null;
    }

    public static string GetPacket(string search, int selectedPageIndex = 0, PaginatedAction action = PaginatedAction.None)
    {
        return InteractionManager.ComponentPacketBuilder(PacketHeader, PacketVersion, (int)action, selectedPageIndex, search);
    }

    protected override IEnumerable<string> GetContent()
    {
        foreach (var monsterData in _data)
        {
            var minLevel = monsterData.GetMinLevel();
            var maxLevel = monsterData.GetMaxLevel();

            yield return $"- Niv.{minLevel}{(minLevel == maxLevel ? string.Empty : $"-{maxLevel}")} {Formatter.Bold($"{monsterData.Name} {(monsterData.BreedSummon ? "(invocation)" : string.Empty)}")} ({monsterData.Id})";
        }
    }

    protected override DiscordSelectComponent SelectBuilder()
    {
        return MonsterComponentsBuilder.MonstersSelectBuilder(0, _data);
    }

    protected override string PreviousPacketBuilder()
    {
        return GetPacket(_search, PreviousPageIndex());
    }

    protected override string NextPacketBuilder()
    {
        return GetPacket(_search, NextPageIndex());
    }
}
