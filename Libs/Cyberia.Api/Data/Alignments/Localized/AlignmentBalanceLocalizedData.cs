using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Alignments.Localized;

internal sealed class AlignmentBalanceLocalizedData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public string Name { get; init; }

    [JsonPropertyName("d")]
    public string Description { get; init; }

    [JsonConstructor]
    internal AlignmentBalanceLocalizedData()
    {
        Name = string.Empty;
        Description = string.Empty;
    }
}
