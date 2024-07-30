using Cyberia.Api.Managers;

using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Alignments;

public sealed class AlignmentData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public LocalizedString Name { get; init; }

    [JsonPropertyName("c")]
    public bool CanConquest { get; init; }

    [JsonConstructor]
    internal AlignmentData()
    {
        Name = LocalizedString.Empty;
    }

    public async Task<string> GetIconImagePathAsync(CdnImageSize size)
    {
        return await CdnManager.GetImagePathAsync("alignments", Id, size);
    }

    public async Task<string> GetMiniImagePathAsync(CdnImageSize size)
    {
        return await CdnManager.GetImagePathAsync("alignments/mini", Id, size);
    }

    public bool CanJoin(int alignmentId)
    {
        var alignmentJoinData = DofusApi.Datacenter.AlignmentsRepository.GetAlignmentJoinDataById(Id);

        return alignmentJoinData is not null && alignmentJoinData.CanJoin(alignmentId);
    }

    public bool CanAttack(int alignmentId)
    {
        var alignmentAttackData = DofusApi.Datacenter.AlignmentsRepository.GetAlignmentAttackDataById(Id);

        return alignmentAttackData is not null && alignmentAttackData.CanAttack(alignmentId);
    }

    public bool CanViewPvpGain(int alignmentId)
    {
        var alignmentViewPvpGainData = DofusApi.Datacenter.AlignmentsRepository.GetAlignmentViewPvpGainDataById(Id);

        return alignmentViewPvpGainData is not null && alignmentViewPvpGainData.CanViewPvpGain(alignmentId);
    }
}
