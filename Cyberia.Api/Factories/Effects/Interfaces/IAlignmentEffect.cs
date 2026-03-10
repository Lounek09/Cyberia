using Cyberia.Api.Data.Alignments;

namespace Cyberia.Api.Factories.Effects.Interfaces;

public interface IAlignmentEffect
{
    int AlignmentId { get; }

    AlignmentData? GetAlignmentData();
}
