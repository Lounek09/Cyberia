using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Skills.Localized;

internal sealed class SkillLocalizedData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("d")]
    public string Description { get; init; }

    [JsonConstructor]
    internal SkillLocalizedData()
    {
        Description = string.Empty;
    }
}
