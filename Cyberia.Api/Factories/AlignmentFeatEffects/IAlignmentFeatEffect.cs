using Cyberia.Api.Data.Alignments;
using Cyberia.Langzilla.Enums;

using System.Globalization;

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
    /// Generates a human-readable description of the alignment feat effect for the specified language.
    /// </summary>
    /// <param name="language">The language to generate the description for.</param>
    /// <returns>The <see cref="DescriptionString"/> object containing the description of the alignment feat effect for the specified language.</returns>
    DescriptionString GetDescription(Language language);

    /// <summary>
    /// Generates a human-readable description of the alignment feat effect for the specified culture.
    /// </summary>
    /// <param name="culture">The culture to generate the description for.</param>
    /// <returns>The <see cref="DescriptionString"/> object containing the description of the alignment feat effect for the specified culture.</returns>
    DescriptionString GetDescription(CultureInfo? culture = null);
}
