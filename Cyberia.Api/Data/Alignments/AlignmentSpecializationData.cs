using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Alignments;

public sealed class AlignmentSpecializationData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public string Name { get; init; }

    [JsonPropertyName("d")]
    public string Description { get; init; }

    [JsonPropertyName("o")]
    public int AlignmentOrderId { get; init; }

    [JsonPropertyName("av")]
    public int AlignmentLevelRequired { get; init; }

    [JsonPropertyName("f")]
    [JsonInclude]
    internal IReadOnlyList<JsonElement> CompressedAlignmentFeatsParameters { get; init; }

    [JsonIgnore]
    public IReadOnlyList<AlignmentFeatParametersData> AlignmentFeatsParametersData { get; internal set; }

    [JsonConstructor]
    internal AlignmentSpecializationData()
    {
        Name = string.Empty;
        Description = string.Empty;
        CompressedAlignmentFeatsParameters = [];
        AlignmentFeatsParametersData = [];
    }

    public AlignmentOrderData? GetAlignementOrderData()
    {
        return DofusApi.Datacenter.AlignmentsRepository.GetAlignmentOrderDataById(AlignmentOrderId);
    }
}
