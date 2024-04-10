using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Alignments;

public sealed class AlignmentOrderData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public string Name { get; init; }

    [JsonPropertyName("a")]
    public int AlignmentId { get; init; }

    [JsonConstructor]
    internal AlignmentOrderData()
    {
        Name = string.Empty;
    }

    public AlignmentData? GetAlignementData()
    {
        return DofusApi.Datacenter.AlignmentsData.GetAlignmentDataById(AlignmentId);
    }
}
