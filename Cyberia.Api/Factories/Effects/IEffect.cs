using Cyberia.Api.Data.Effects;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

namespace Cyberia.Api.Factories.Effects;

/// <summary>
/// Represents an effect of an in game object (a spell, an item, etc).
/// </summary>
public interface IEffect : IComparable<IEffect>
{
    /// <summary>
    /// Gets the unique identifier of the effect.
    /// </summary>
    int Id { get; init; }

    /// <summary>
    /// Gets the duration of the effect.
    /// </summary>
    int Duration { get; init; }

    /// <summary>
    /// Gets the probability of the effect in percentage.
    /// </summary>
    int Probability { get; init; }

    /// <summary>
    /// Gets the criteria where the effect is applicable.
    /// </summary>
    CriteriaReadOnlyCollection Criteria { get; init; }

    /// <summary>
    /// Gets the area of the effect.
    /// </summary>
    EffectArea EffectArea { get; init; }

    /// <summary>
    /// Gets the data of the effect.
    /// </summary>
    /// <returns>The <see cref="EffectData"/> object containing the data of the effect.</returns>
    EffectData? GetEffectData();

    /// <summary>
    /// Generates a human-readable description of the effect.
    /// </summary>
    /// <returns>The <see cref="DescriptionString"/> object containing the description of the effect.</returns>
    DescriptionString GetDescription();
}
