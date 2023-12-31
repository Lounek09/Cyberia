using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Alignments;

internal sealed class AlignmentViewPvpGainData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("v")]
    public IReadOnlyList<bool> Values { get; init; }

    [JsonConstructor]
    internal AlignmentViewPvpGainData()
    {
        Values = [];
    }

    public bool CanViewPvpGain(int targetAlignmentId)
    {
        return Values.Count >= targetAlignmentId && Values[targetAlignmentId];
    }
}
