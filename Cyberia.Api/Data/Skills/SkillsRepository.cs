﻿using Cyberia.Api.Data.Skills.Localized;
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

    protected override void LoadLocalizedData(LangType type, Language language)
    {
        var twoLetterISOLanguageName = language.ToStringFast();
        var localizedRepository = DofusLocalizedRepository.Load<SkillsLocalizedRepository>(type, language);

        foreach (var skillLocalizedData in localizedRepository.Skills)
        {
            var skillData = GetSkillDataById(skillLocalizedData.Id);
            skillData?.Description.TryAdd(twoLetterISOLanguageName, skillLocalizedData.Description);
        }
    }
}
