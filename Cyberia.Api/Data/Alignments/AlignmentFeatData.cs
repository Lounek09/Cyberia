using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Alignments;

public sealed class AlignmentFeatData
    : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public string Name { get; init; }

    [JsonPropertyName("g")]
    public int GfxId { get; init; }

    [JsonPropertyName("e")]
    public int AlignmentFeatEffectId { get; init; }

    [JsonConstructor]
    internal AlignmentFeatData()
    {
        Name = string.Empty;
    }

    public AlignmentFeatEffectData? GetAlignmentFeatEffectData()
    {
        return DofusApi.Datacenter.AlignmentsData.GetAlignmentFeatEffectDataById(AlignmentFeatEffectId);
    }
}
