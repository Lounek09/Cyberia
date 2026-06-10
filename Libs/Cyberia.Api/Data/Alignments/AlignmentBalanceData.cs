using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Alignments;

public sealed class AlignmentBalanceData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("s")]
    public int Start { get; init; }

    [JsonPropertyName("e")]
    public int End { get; init; }

    [JsonPropertyName("n")]
    public LocalizedString Name { get; init; }

    [JsonPropertyName("d")]
    public LocalizedString Description { get; init; }

    [JsonConstructor]
    internal AlignmentBalanceData()
    {
        Name = LocalizedString.Empty;
        Description = LocalizedString.Empty;
    }
}
