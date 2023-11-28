using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Skills;

public sealed class SkillsData : IDofusData
{
    private const string FILE_NAME = "skills.json";

    [JsonPropertyName("SK")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, SkillData>))]
    public FrozenDictionary<int, SkillData> Skills { get; init; }

    [JsonConstructor]
    internal SkillsData()
    {
        Skills = FrozenDictionary<int, SkillData>.Empty;
    }

    internal static SkillsData Load()
    {
        return Datacenter.LoadDataFromFile<SkillsData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
    }

    public SkillData? GetSkillDataById(int id)
    {
        Skills.TryGetValue(id, out var skillData);
        return skillData;
    }
}
