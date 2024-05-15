using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Skills;

public sealed class SkillsRepository : IDofusRepository
{
    private const string c_fileName = "skills.json";

    [JsonPropertyName("SK")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, SkillData>))]
    public FrozenDictionary<int, SkillData> Skills { get; init; }

    [JsonConstructor]
    internal SkillsRepository()
    {
        Skills = FrozenDictionary<int, SkillData>.Empty;
    }

    internal static SkillsRepository Load(string directoryPath)
    {
        var filePath = Path.Join(directoryPath, c_fileName);

        return Datacenter.LoadRepository<SkillsRepository>(filePath);
    }

    public SkillData? GetSkillDataById(int id)
    {
        Skills.TryGetValue(id, out var skillData);
        return skillData;
    }
}
