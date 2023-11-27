using Cyberia.Api.Data.Custom;
using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data;

public sealed class MonsterSuperRaceData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public string Name { get; init; }

    [JsonPropertyName("s")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [JsonInclude]
    internal int MonsterSuperRaceId { get; init; }

    [JsonConstructor]
    internal MonsterSuperRaceData()
    {
        Name = string.Empty;
    }
}

public sealed class MonsterRaceData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public string Name { get; init; }

    [JsonPropertyName("s")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int MonsterSuperRaceId { get; init; }

    [JsonConstructor]
    internal MonsterRaceData()
    {
        Name = string.Empty;
    }

    public MonsterSuperRaceData? GetMonsterSuperRaceData()
    {
        return DofusApi.Datacenter.MonstersData.GetMonsterSuperRaceDataById(MonsterSuperRaceId);
    }
}



public sealed class MonsterGradeData : IDofusData
{
    [JsonPropertyName("l")]
    public int Level { get; init; }

    [JsonPropertyName("r")]
    [JsonConverter(typeof(ReadOnlyCollectionConverter<int>))]
    public ReadOnlyCollection<int> Resistances { get; init; }

    [JsonPropertyName("lp")]
    public int? LifePoint { get; init; }

    [JsonPropertyName("ap")]
    public int? ActionPoint { get; init; }

    [JsonPropertyName("mp")]
    public int? MovementPoint { get; init; }

    [JsonConstructor]
    internal MonsterGradeData()
    {
        Resistances = ReadOnlyCollection<int>.Empty;
    }

    public int GetNeutralResistance()
    {
        return Resistances.Count > 0 ? Resistances[0] : 0;
    }

    public int GetEarthResistance()
    {
        return Resistances.Count > 1 ? Resistances[1] : 0;
    }

    public int GetFireResistance()
    {
        return Resistances.Count > 2 ? Resistances[2] : 0;
    }

    public int GetWaterResistance()
    {
        return Resistances.Count > 3 ? Resistances[3] : 0;
    }

    public int GetAirResistance()
    {
        return Resistances.Count > 4 ? Resistances[4] : 0;
    }

    public int GetActionPointDodge()
    {
        return Resistances.Count > 5 ? Resistances[5] : 0;
    }

    public int GetMovementPointDodge()
    {
        return Resistances.Count > 6 ? Resistances[6] : 0;
    }
}

public sealed class MonsterData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public string Name { get; init; }

    [JsonPropertyName("g")]
    public int GfxId { get; init; }

    [JsonPropertyName("b")]
    public int MonsterRaceId { get; init; }

    [JsonPropertyName("a")]
    public int AlignmentId { get; init; }

    [JsonPropertyName("k")]
    public bool Kickable { get; init; }

    [JsonPropertyName("d")]
    public bool Boss { get; init; }

    [JsonPropertyName("s")]
    public bool SearcheableInBigStore { get; init; }

    [JsonPropertyName("g1")]
    public MonsterGradeData? MonsterGradeData1 { get; init; }

    [JsonPropertyName("g2")]
    public MonsterGradeData? MonsterGradeData2 { get; init; }

    [JsonPropertyName("g3")]
    public MonsterGradeData? MonsterGradeData3 { get; init; }

    [JsonPropertyName("g4")]
    public MonsterGradeData? MonsterGradeData4 { get; init; }

    [JsonPropertyName("g5")]
    public MonsterGradeData? MonsterGradeData5 { get; init; }

    [JsonPropertyName("g6")]
    public MonsterGradeData? MonsterGradeData6 { get; init; }

    [JsonPropertyName("g7")]
    public MonsterGradeData? MonsterGradeData7 { get; init; }

    [JsonPropertyName("g8")]
    public MonsterGradeData? MonsterGradeData8 { get; init; }

    [JsonPropertyName("g9")]
    public MonsterGradeData? MonsterGradeData9 { get; init; }

    [JsonPropertyName("g10")]
    public MonsterGradeData? MonsterGradeData10 { get; init; }

    [JsonIgnore]
    public bool BreedSummon { get; internal set; }

    [JsonIgnore]
    public string TrelloUrl { get; internal set; }

    [JsonConstructor]
    internal MonsterData()
    {
        Name = string.Empty;
        TrelloUrl = string.Empty;
    }

    public async Task<string> GetImagePath()
    {
        var url = $"{DofusApi.Config.CdnUrl}/images/artworks/{GfxId}.png";

        if (await DofusApi.HttpClient.ExistsAsync(url))
        {
            return url;
        }

        return $"{DofusApi.Config.CdnUrl}/images/artworks/unknown.png";
    }

    public MonsterRaceData? GetMonsterRaceData()
    {
        return DofusApi.Datacenter.MonstersData.GetMonsterRaceDataById(MonsterRaceId);
    }

    public AlignmentData? GetAlignmentData()
    {
        return DofusApi.Datacenter.AlignmentsData.GetAlignmentDataById(AlignmentId);
    }

    public MonsterGradeData? GetMonsterGradeData(int grade = 1)
    {
        return grade switch
        {
            1 => MonsterGradeData1,
            2 => MonsterGradeData2,
            3 => MonsterGradeData3,
            4 => MonsterGradeData4,
            5 => MonsterGradeData5,
            6 => MonsterGradeData6,
            7 => MonsterGradeData7,
            8 => MonsterGradeData8,
            9 => MonsterGradeData9,
            10 => MonsterGradeData10,
            _ => null,
        };
    }

    public int GetMaxGradeNumber()
    {
        for (var i = 10; i > 0; i--)
        {
            if (GetMonsterGradeData(i) is not null)
            {
                return i;
            }
        }

        return -1;
    }

    public int GetMinLevel()
    {
        return MonsterGradeData1 is null ? -1 : MonsterGradeData1.Level;
    }

    public int GetMaxLevel()
    {
        var monsterGradeData = GetMonsterGradeData(GetMaxGradeNumber());
        return monsterGradeData is null ? -1 : monsterGradeData.Level;
    }
}

public sealed class MonstersData : IDofusData
{
    private const string FILE_NAME = "monsters.json";

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
    internal MonstersData()
    {
        MonsterSuperRaces = FrozenDictionary<int, MonsterSuperRaceData>.Empty;
        MonsterRaces = FrozenDictionary<int, MonsterRaceData>.Empty;
        Monsters = FrozenDictionary<int, MonsterData>.Empty;
    }

    internal static MonstersData Load()
    {
        var data = Datacenter.LoadDataFromFile<MonstersData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
        var customData = Datacenter.LoadDataFromFile<MonstersCustomData>(Path.Combine(DofusApi.CUSTOM_PATH, FILE_NAME));

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

        return monsterSuperRaceData is null ? PatternDecoder.Description(Resources.Unknown_Data, id) : monsterSuperRaceData.Name;
    }

    public MonsterRaceData? GetMonsterRaceDataById(int id)
    {
        MonsterRaces.TryGetValue(id, out var monsterRaceData);
        return monsterRaceData;
    }

    public string GetMonsterRaceNameById(int id)
    {
        var monsterRaceData = GetMonsterRaceDataById(id);

        return monsterRaceData is null ? PatternDecoder.Description(Resources.Unknown_Data, id) : monsterRaceData.Name;
    }

    public MonsterData? GetMonsterDataById(int id)
    {
        Monsters.TryGetValue(id, out var monsterData);
        return monsterData;
    }

    public IEnumerable<MonsterData> GetMonstersDataByName(string name)
    {
        var names = name.NormalizeCustom().Split(' ');
        return Monsters.Values.Where(x => names.All(x.Name.NormalizeCustom().Contains));
    }

    public string GetMonsterNameById(int id)
    {
        var monsterData = GetMonsterDataById(id);

        return monsterData is null ? PatternDecoder.Description(Resources.Unknown_Data, id) : monsterData.Name;
    }
}
