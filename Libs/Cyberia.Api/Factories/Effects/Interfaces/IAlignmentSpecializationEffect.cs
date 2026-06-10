using Cyberia.Api.Data.Alignments;

namespace Cyberia.Api.Factories.Effects.Interfaces;

public interface IAlignmentSpecializationEffect
{
    int AlignmentSpecializationId { get; }

    AlignmentSpecializationData? GetAlignmentSpecializationData();
}
