using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Alignments;

public sealed class AlignmentFeatEffectData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("v")]
    public string Description { get; init; }

    [JsonConstructor]
    internal AlignmentFeatEffectData()
    {
        Description = string.Empty;
    }
}
