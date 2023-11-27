using Cyberia.Api;
using Cyberia.Api.Data;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus;

public sealed class PaginatedMonsterMessageBuilder : PaginatedMessageBuilder<MonsterData>
{
    public const string PACKET_HEADER = "PM";
    public const int PACKET_VERSION = 1;

    private readonly string _search;

    public PaginatedMonsterMessageBuilder(List<MonsterData> monstersData, string search, int selectedPageIndex = 0)
        : base(DofusEmbedCategory.Bestiary, "Bestiaire", "Plusieurs monstres trouvés :", monstersData, selectedPageIndex)
    {
        _search = search;
    }

    public static PaginatedMonsterMessageBuilder? Create(int version, string[] parameters)
    {
        if (version == PACKET_VERSION &&
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

    public static string GetPacket(string search, int selectedPageIndex = 0, PaginatedAction action = PaginatedAction.Nothing)
    {
        return InteractionManager.ComponentPacketBuilder(PACKET_HEADER, PACKET_VERSION, (int)action, selectedPageIndex, search);
    }

    protected override IEnumerable<string> GetContent()
    {
        foreach (var monsterData in _data)
        {
            var minLevel = monsterData.GetMinLevel();
            var maxLevel = monsterData.GetMaxLevel();

            yield return $"- Niv.{minLevel}{(minLevel == maxLevel ? "" : $"-{maxLevel}")} {Formatter.Bold($"{monsterData.Name} {(monsterData.BreedSummon ? "(invocation)" : "")}")} ({monsterData.Id})";
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
