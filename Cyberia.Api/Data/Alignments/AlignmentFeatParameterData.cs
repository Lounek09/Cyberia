using Cyberia.Api.Factories.AlignmentFeatEffects;

using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Alignments;

public sealed class AlignmentFeatParametersData
{
    public int AlignmentFeatId { get; init; }

    public int Level { get; init; }

    public IAlignmentFeatEffect? AlignmentFeatEffect { get; init; }

    [JsonConstructor]
    internal AlignmentFeatParametersData()
    {

    }

    public AlignmentFeatData? GetAlignmentFeatData()
    {
        return DofusApi.Datacenter.AlignmentsRepository.GetAlignmentFeatDataById(AlignmentFeatId);
    }
}
