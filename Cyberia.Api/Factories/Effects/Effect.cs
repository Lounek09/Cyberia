using Cyberia.Api.Data.Effects;
using Cyberia.Api.Factories.Effects.Elements;
using Cyberia.Api.JsonConverters;
using Cyberia.Langzilla.Primitives;

using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Factories.Effects;

/// <summary>
/// Represents an effect of an in game object (a spell, an item, etc).
/// </summary>
[JsonConverter(typeof(EffectConverter))]
public abstract record Effect : IComparable<Effect>
{
    /// <summary>
    /// Gets the unique identifier of the effect.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Effect"/> record.
    /// </summary>
    /// <param name="id">The ID of the effect.</param>
    protected Effect(int id)
    {
        Id = id;
    }

    /// <summary>
    /// Gets the data of the effect.
    /// </summary>
    /// <returns>The <see cref="EffectData"/> object containing the data of the effect.</returns>
    public EffectData? GetData()
    {
        return DofusApi.Datacenter.EffectsRepository.GetEffectDataById(Id);
    }

    /// <summary>
    /// Generates a human-readable description of the effect for the specified language.
    /// </summary>
    /// <param name="language">The language to generate the description for.</param>
    /// <returns>The <see cref="DescriptionString"/> object containing the description of the effect for the specified language.</returns>
    public DescriptionString GetDescription(Language language)
    {
        return GetDescription(language.ToCulture());
    }

    /// <summary>
    /// Generates a human-readable description of the effect for the specified culture.
    /// </summary>
    /// <param name="culture">The culture to generate the description for.</param>
    /// <returns>The <see cref="DescriptionString"/> object containing the description of the effect for the specified culture.</returns>
    [OverloadResolutionPriority(2)]
    public abstract DescriptionString GetDescription(CultureInfo? culture = null);

    /// <inheritdoc cref="GetDescription(CultureInfo)"/>
    protected DescriptionString GetDescription<T>(CultureInfo? culture, T parameter)
    {
        return GetDescription(culture, parameter?.ToString() ?? string.Empty);
    }

    /// <inheritdoc cref="GetDescription(CultureInfo)"/>
    protected DescriptionString GetDescription<T0, T1>(CultureInfo? culture, T0 parameter0, T1 parameter1)
    {
        return GetDescription(culture,
            parameter0?.ToString() ?? string.Empty,
            parameter1?.ToString() ?? string.Empty);
    }

    /// <inheritdoc cref="GetDescription(CultureInfo)"/>
    protected DescriptionString GetDescription<T0, T1, T2>(CultureInfo? culture, T0 parameter0, T1 parameter1, T2 parameter2)
    {
        return GetDescription(culture,
            parameter0?.ToString() ?? string.Empty,
            parameter1?.ToString() ?? string.Empty,
            parameter2?.ToString() ?? string.Empty);
    }

    /// <inheritdoc cref="GetDescription(CultureInfo)"/>
    protected DescriptionString GetDescription<T0, T1, T2, T3>(CultureInfo? culture, T0 parameter0, T1 parameter1, T2 parameter2, T3 parameter3)
    {
        return GetDescription(culture,
            parameter0?.ToString() ?? string.Empty,
            parameter1?.ToString() ?? string.Empty,
            parameter2?.ToString() ?? string.Empty,
            parameter3?.ToString() ?? string.Empty);
    }

    /// <inheritdoc cref="GetDescription(CultureInfo)"/>
    [OverloadResolutionPriority(1)]
    protected DescriptionString GetDescription(CultureInfo? culture, params string[] parameters)
    {
        var effectData = GetData();
        if (effectData is null)
        {
            if (this is not UntranslatedEffect)
            {
                Log.Warning("Unknown EffectData {@Effect}", this);
            }

            return new DescriptionString(Translation.Get<ApiTranslations>("Effect.Unknown", culture),
                Id.ToString(), string.Join(',', parameters));
        }

        return new DescriptionString(effectData.Description.ToString(culture), parameters);
    }

    public int CompareTo(Effect? other)
    {
        if (other is null)
        {
            return 1;
        }

        var effectData = GetData();
        var otherEffectData = other.GetData();

        if (effectData is null && otherEffectData is null)
        {
            return 0;
        }

        if (effectData is null)
        {
            return -1;
        }

        if (otherEffectData is null)
        {
            return 1;
        }

        return effectData.CompareTo(otherEffectData);
    }
}
