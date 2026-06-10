using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Alignments.Localized;

internal sealed class AlignmentFeatEffectLocalizedData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("v")]
    public string Description { get; init; }

    [JsonConstructor]
    internal AlignmentFeatEffectLocalizedData()
    {
        Description = string.Empty;
    }
}
