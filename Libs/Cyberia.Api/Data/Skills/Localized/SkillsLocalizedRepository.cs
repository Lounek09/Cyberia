using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Skills.Localized;

internal sealed class SkillsLocalizedRepository : DofusLocalizedRepository, IDofusRepository
{
    public static string FileName => SkillsRepository.FileName;

    [JsonPropertyName("SK")]
    public IReadOnlyList<SkillLocalizedData> Skills { get; init; }

    [JsonConstructor]
    internal SkillsLocalizedRepository()
    {
        Skills = ReadOnlyCollection<SkillLocalizedData>.Empty;
    }
}
