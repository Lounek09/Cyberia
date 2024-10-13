using Cyberia.Api.Data.Monsters.Custom;
using Cyberia.Api.Data.Monsters.Localized;
using Cyberia.Api.JsonConverters;
using Cyberia.Langzilla.Enums;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Monsters;

public sealed class MonstersRepository : DofusRepository, IDofusRepository
{
    public static string FileName => "monsters.json";

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

    public MonsterSuperRaceData? GetMonsterSuperRaceDataById(int id)
    {
        MonsterSuperRaces.TryGetValue(id, out var monsterSuperRaceData);
        return monsterSuperRaceData;
    }

    public string GetMonsterSuperRaceNameById(int id)
    {
        var monsterSuperRaceData = GetMonsterSuperRaceDataById(id);

        return monsterSuperRaceData is null
            ? Translation.Format(ApiTranslations.Unknown_Data, id)
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
            ? Translation.Format(ApiTranslations.Unknown_Data, id)
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
                return x.NormalizedName.ToString().Contains(y, StringComparison.OrdinalIgnoreCase);
            });
        });
    }

    public string GetMonsterNameById(int id)
    {
        var monsterData = GetMonsterDataById(id);

        return monsterData is null
            ? Translation.Format(ApiTranslations.Unknown_Data, id)
            : monsterData.Name;
    }

    protected override void LoadCustomData()
    {
        var customRepository = DofusCustomRepository.Load<MonstersCustomRepository>();

        foreach (var monsterCustomData in customRepository.MonstersCustom)
        {
            var monsterData = GetMonsterDataById(monsterCustomData.Id);
            if (monsterData is not null)
            {
                monsterData.BreedSummon = monsterCustomData.BreedSummon;
                monsterData.TrelloUrl = monsterCustomData.TrelloUrl;
            }
        }
    }

    protected override void LoadLocalizedData(LangType type, Language language)
    {
        var twoLetterISOLanguageName = language.ToStringFast();
        var localizedRepository = DofusLocalizedRepository.Load<MonstersLocalizedRepository>(type, language);

        foreach (var monsterSuperRaceLocalizedData in localizedRepository.MonsterSuperRaces)
        {
            var monsterSuperRaceData = GetMonsterSuperRaceDataById(monsterSuperRaceLocalizedData.Id);
            monsterSuperRaceData?.Name.Add(twoLetterISOLanguageName, monsterSuperRaceLocalizedData.Name);
        }

        foreach (var monsterRaceLocalizedData in localizedRepository.MonsterRaces)
        {
            var monsterRaceData = GetMonsterRaceDataById(monsterRaceLocalizedData.Id);
            monsterRaceData?.Name.Add(twoLetterISOLanguageName, monsterRaceLocalizedData.Name);
        }

        foreach (var monsterLocalizedData in localizedRepository.Monsters)
        {
            var monsterData = GetMonsterDataById(monsterLocalizedData.Id);
            if (monsterData is not null)
            {
                monsterData.Name.Add(twoLetterISOLanguageName, monsterLocalizedData.Name);
                monsterData.NormalizedName.Add(twoLetterISOLanguageName, monsterLocalizedData.NormalizedName);
            }
        }
    }

    protected override void FinalizeLoading()
    {
        foreach (var pair in Monsters)
        {
            var i = 1;
            foreach (var monsterGradeData in pair.Value.GetMonsterGradesData())
            {
                monsterGradeData.MonsterData = pair.Value;
                monsterGradeData.Rank = i++;
            }
        }
    }
}
