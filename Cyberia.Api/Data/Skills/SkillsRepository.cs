using Cyberia.Api.Data.Skills.Localized;
using Cyberia.Api.JsonConverters;
using Cyberia.Langzilla.Enums;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Skills;

public sealed class SkillsRepository : DofusRepository, IDofusRepository
{
    public static string FileName => "skills.json";

    [JsonPropertyName("SK")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, SkillData>))]
    public FrozenDictionary<int, SkillData> Skills { get; init; }

    [JsonConstructor]
    internal SkillsRepository()
    {
        Skills = FrozenDictionary<int, SkillData>.Empty;
    }

    public SkillData? GetSkillDataById(int id)
    {
        Skills.TryGetValue(id, out var skillData);
        return skillData;
    }

    protected override void LoadLocalizedData(LangsIdentifier identifier)
    {
        var twoLetterISOLanguageName = identifier.Language.ToStringFast();
        var localizedRepository = DofusLocalizedRepository.Load<SkillsLocalizedRepository>(identifier);

        foreach (var skillLocalizedData in localizedRepository.Skills)
        {
            var skillData = GetSkillDataById(skillLocalizedData.Id);
            skillData?.Description.TryAdd(twoLetterISOLanguageName, skillLocalizedData.Description);
        }
    }
}
