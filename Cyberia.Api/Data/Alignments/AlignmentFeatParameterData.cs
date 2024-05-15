using Cyberia.Api.Factories.AlignmentFeatEffects;

namespace Cyberia.Api.Data.Alignments;

public sealed class AlignmentFeatParametersData
{
    public int AlignmentFeatId { get; init; }

    public int Level { get; init; }

    public IAlignmentFeatEffect? AlignmentFeatEffect { get; init; }

    public AlignmentFeatData? GetAlignmentFeatData()
    {
        return DofusApi.Datacenter.AlignmentsRepository.GetAlignmentFeatDataById(AlignmentFeatId);
    }

    public AlignmentFeatParametersData()
    {

    }
}
