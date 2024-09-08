using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Alignments.Localized;

internal sealed class AlignmentFeatLocalizedData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public string Name { get; init; }

    [JsonConstructor]
    internal AlignmentFeatLocalizedData()
    {
        Name = string.Empty;
    }
}
