using Cyberia.Api.Data.Skills;

using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.InteractiveObjects;

public sealed class InteractiveObjectData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public LocalizedString Name { get; init; }

    [JsonPropertyName("t")]
    public int Type { get; init; }

    [JsonPropertyName("sk")]
    public IReadOnlyList<int> SkillsId { get; init; }

    [JsonIgnore]
    public int GfxId { get; internal set; }

    [JsonConstructor]
    internal InteractiveObjectData()
    {
        Name = LocalizedString.Empty;
        SkillsId = ReadOnlyCollection<int>.Empty;
    }

    public IEnumerable<SkillData> GetSkillsData()
    {
        foreach (var skillId in SkillsId)
        {
            var skillData = DofusApi.Datacenter.SkillsRepository.GetSkillDataById(skillId);
            if (skillData is not null)
            {
                yield return skillData;
            }
        }
    }
}
