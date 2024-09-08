using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Alignments.Localized;

internal sealed class AlignmentLocalizedData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public string Name { get; init; }

    [JsonConstructor]
    internal AlignmentLocalizedData()
    {
        Name = string.Empty;
    }
}
