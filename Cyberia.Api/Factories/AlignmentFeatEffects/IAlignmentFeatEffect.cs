using Cyberia.Api.Data.Alignments;

namespace Cyberia.Api.Factories.AlignmentFeatEffects;

public interface IAlignmentFeatEffect
{
    int Id { get; init; }

    AlignmentFeatEffectData? GetAlignmentFeatEffectData();

    Description GetDescription();
}
