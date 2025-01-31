﻿using Cyberia.Api.Data.Effects;
using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Langzilla.Enums;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects;

/// <summary>
/// Represents an effect of an in game object (a spell, an item, etc).
/// </summary>
public interface IEffect : IComparable<IEffect>
{
    /// <summary>
    /// Gets the unique identifier of the effect.
    /// </summary>
    int Id { get; }

    /// <summary>
    /// Gets the duration of the effect.
    /// </summary>
    int Duration { get; }

    /// <summary>
    /// Gets the probability of the effect in percentage.
    /// </summary>
    int Probability { get; }

    /// <summary>
    /// Gets the criteria where the effect is applicable.
    /// </summary>
    CriteriaReadOnlyCollection Criteria { get; }

    /// <summary>
    /// Gets a value indicating whether the effect is dispellable.
    /// </summary>
    bool Dispellable { get; }

    /// <summary>
    /// Gets the area of the effect.
    /// </summary>
    EffectArea EffectArea { get; }

    /// <summary>
    /// Gets the data of the effect.
    /// </summary>
    /// <returns>The <see cref="EffectData"/> object containing the data of the effect.</returns>
    EffectData? GetEffectData();

    /// <summary>
    /// Generates a human-readable description of the effect for the specified language.
    /// </summary>
    /// <param name="language">The language to generate the description for.</param>
    /// <returns>The <see cref="DescriptionString"/> object containing the description of the effect for the specified language.</returns>
    DescriptionString GetDescription(Language language);

    /// <summary>
    /// Generates a human-readable description of the effect for the specified culture.
    /// </summary>
    /// <param name="culture">The culture to generate the description for.</param>
    /// <returns>The <see cref="DescriptionString"/> object containing the description of the effect for the specified culture.</returns>
    DescriptionString GetDescription(CultureInfo? culture = null);
}
