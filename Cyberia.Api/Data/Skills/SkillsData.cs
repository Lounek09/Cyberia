using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Skills;

public sealed class SkillsData
    : IDofusData
{
    private const string c_fileName = "skills.json";

    private static readonly string s_filePath = Path.Join(DofusApi.OutputPath, c_fileName);

    [JsonPropertyName("SK")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, SkillData>))]
    public FrozenDictionary<int, SkillData> Skills { get; init; }

    [JsonConstructor]
    internal SkillsData()
    {
        Skills = FrozenDictionary<int, SkillData>.Empty;
    }

    internal static async Task<SkillsData> LoadAsync()
    {
        return await Datacenter.LoadDataAsync<SkillsData>(s_filePath);
    }

    public SkillData? GetSkillDataById(int id)
    {
        Skills.TryGetValue(id, out var skillData);
        return skillData;
    }
}
