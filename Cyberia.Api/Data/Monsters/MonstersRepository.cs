using Cyberia.Api.Data.Monsters.Custom;
using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Monsters;

public sealed class MonstersRepository : IDofusRepository
{
    private const string c_fileName = "monsters.json";

    [JsonPropertyName("MSR")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, MonsterSuperRaceData>))]
    public FrozenDictionary<int, MonsterSuperRaceData> MonsterSuperRaces { get; init; }

    [JsonPropertyName("MR")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, MonsterRaceData>))]
    public FrozenDictionary<int, MonsterRaceData> MonsterRaces { get; init; }

    [JsonPropertyName("M")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, MonsterData>))]
    public FrozenDictionary<int, MonsterData> Monsters { get; init; }

    [JsonConstructor]
    internal MonstersRepository()
    {
        MonsterSuperRaces = FrozenDictionary<int, MonsterSuperRaceData>.Empty;
        MonsterRaces = FrozenDictionary<int, MonsterRaceData>.Empty;
        Monsters = FrozenDictionary<int, MonsterData>.Empty;
    }

    internal static MonstersRepository Load(string directoryPath)
    {
        var filePath = Path.Join(directoryPath, c_fileName);
        var customFilePath = Path.Join(DofusApi.CustomPath, c_fileName);

        var data = Datacenter.LoadRepository<MonstersRepository>(filePath);
        var customData = Datacenter.LoadRepository<MonstersCustomRepository>(customFilePath);

        foreach (var pair in data.Monsters)
        {
            var i = 1;
            foreach (var monsterGradeData in pair.Value.GetMonsterGradesData())
            {
                monsterGradeData.MonsterData = pair.Value;
                monsterGradeData.Rank = i++;
            }
        }

        foreach (var monsterCustomData in customData.MonstersCustom)
        {
            var monsterData = data.GetMonsterDataById(monsterCustomData.Id);
            if (monsterData is not null)
            {
                monsterData.BreedSummon = monsterCustomData.BreedSummon;
                monsterData.TrelloUrl = monsterCustomData.TrelloUrl;
            }
        }

        return data;
    }

    public MonsterSuperRaceData? GetMonsterSuperRaceDataById(int id)
    {
        MonsterSuperRaces.TryGetValue(id, out var monsterSuperRaceData);
        return monsterSuperRaceData;
    }

    public string GetMonsterSuperRaceNameById(int id)
    {
        var monsterSuperRaceData = GetMonsterSuperRaceDataById(id);

        return monsterSuperRaceData is null
            ? PatternDecoder.Description(Resources.Unknown_Data, id)
            : monsterSuperRaceData.Name;
    }

    public MonsterRaceData? GetMonsterRaceDataById(int id)
    {
        MonsterRaces.TryGetValue(id, out var monsterRaceData);
        return monsterRaceData;
    }

    public string GetMonsterRaceNameById(int id)
    {
        var monsterRaceData = GetMonsterRaceDataById(id);

        return monsterRaceData is null
            ? PatternDecoder.Description(Resources.Unknown_Data, id)
            : monsterRaceData.Name;
    }

    public MonsterData? GetMonsterDataById(int id)
    {
        Monsters.TryGetValue(id, out var monsterData);
        return monsterData;
    }

    public IEnumerable<MonsterData> GetMonstersDataByName(string name)
    {
        var names = name.NormalizeToAscii().Split(' ', StringSplitOptions.RemoveEmptyEntries);

        return Monsters.Values.Where(x =>
        {
            return names.All(y =>
            {
                return x.Name.NormalizeToAscii().Contains(y, StringComparison.OrdinalIgnoreCase);
            });
        });
    }

    public string GetMonsterNameById(int id)
    {
        var monsterData = GetMonsterDataById(id);

        return monsterData is null
            ? PatternDecoder.Description(Resources.Unknown_Data, id)
            : monsterData.Name;
    }
}
