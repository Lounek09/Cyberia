using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Aligments;

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
    //TODO: JsonConverter for AlignmentFeatsParameters in AlignmentSpecializationData
    private List<List<object>> AlignmentFeatsParameters { get; init; }

    [JsonConstructor]
    internal AlignmentSpecializationData()
    {
        Name = string.Empty;
        Description = string.Empty;
        AlignmentFeatsParameters = [];
    }

    public AlignmentOrderData? GetAlignementOrderData()
    {
        return DofusApi.Datacenter.AlignmentsData.GetAlignmentOrderDataById(AlignmentOrderId);
    }
}
