using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Alignments;

public sealed class AlignmentSpecializationData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public LocalizedString Name { get; init; }

    [JsonPropertyName("d")]
    public LocalizedString Description { get; init; }

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
        Name = LocalizedString.Empty;
        Description = LocalizedString.Empty;
        CompressedAlignmentFeatsParameters = ReadOnlyCollection<JsonElement>.Empty;
        AlignmentFeatsParametersData = ReadOnlyCollection<AlignmentFeatParametersData>.Empty;
    }

    public AlignmentOrderData? GetAlignementOrderData()
    {
        return DofusApi.Datacenter.AlignmentsRepository.GetAlignmentOrderDataById(AlignmentOrderId);
    }
}
