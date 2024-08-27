using Cyberia.Api.Data.Alignments;

namespace Cyberia.Api.Factories.AlignmentFeatEffects;

/// <summary>
/// Represents an effect of an alignment feat.
/// </summary>
public interface IAlignmentFeatEffect
{
    /// <summary>
    /// Gets the unique identifier of the alignment feat effect.
    /// </summary>
    int Id { get; init; }

    /// <summary>
    /// Gets the alignment feat effect data.
    /// </summary>
    /// <returns>The <see cref="AlignmentFeatEffectData"/> object containing the data of the alignment feat effect.</returns>
    AlignmentFeatEffectData? GetAlignmentFeatEffectData();

    /// <summary>
    /// Generates a human-readable description of the alignment feat effect.
    /// </summary>
    /// <returns>The <see cref="DescriptionString"/> object containing the description of the alignment feat effect.</returns>
    DescriptionString GetDescription();
}
