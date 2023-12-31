using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Alignments;

public sealed class AlignmentData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public string Name { get; init; }

    [JsonPropertyName("c")]
    public bool CanConquest { get; init; }

    [JsonConstructor]
    internal AlignmentData()
    {
        Name = string.Empty;
    }

    public bool CanJoin(int alignmentId)
    {
        var alignmentJoinData = DofusApi.Datacenter.AlignmentsData.GetAlignmentJoinDataById(Id);

        return alignmentJoinData is not null && alignmentJoinData.CanJoin(alignmentId);
    }

    public bool CanAttack(int alignmentId)
    {
        var alignmentAttackData = DofusApi.Datacenter.AlignmentsData.GetAlignmentAttackDataById(Id);

        return alignmentAttackData is not null && alignmentAttackData.CanAttack(alignmentId);
    }

    public bool CanViewPvpGain(int alignmentId)
    {
        var alignmentViewPvpGainData = DofusApi.Datacenter.AlignmentsData.GetAlignmentViewPvpGainDataById(Id);

        return alignmentViewPvpGainData is not null && alignmentViewPvpGainData.CanViewPvpGain(alignmentId);
    }
}
