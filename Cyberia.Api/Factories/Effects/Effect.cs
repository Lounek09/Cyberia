using Cyberia.Api.Data.Effects;
using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Factories.Effects.Elements;
using Cyberia.Langzilla.Enums;

using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace Cyberia.Api.Factories.Effects;

/// <inheritdoc cref="IEffect"/>
public abstract record Effect : IEffect
{
    public int Id { get; init; }
    public int Duration { get; init; }
    public int Probability { get; init; }
    public CriteriaReadOnlyCollection Criteria { get; init; }
    public bool Dispellable { get; init; }
    public EffectArea EffectArea { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Effect"/> record.
    /// </summary>
    /// <param name="id">The unique identifier of the effect.</param>
    /// <param name="duration">The duration of the effect.</param>
    /// <param name="probability">The probability (as a percentage) that the effect will occur.</param>
    /// <param name="criteria">The criteria where the effect is applicable</param>
    /// <param name="effectArea">The area of the effect.</param>
    protected Effect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea)
    {
        Id = id;
        Duration = duration;
        Probability = probability;
        Criteria = criteria;
        Dispellable = dispellable;
        EffectArea = effectArea;
    }

    public EffectData? GetEffectData()
    {
        return DofusApi.Datacenter.EffectsRepository.GetEffectDataById(Id);
    }

    public DescriptionString GetDescription(Language language)
    {
        return GetDescription(language.ToCulture());
    }

    [OverloadResolutionPriority(2)]
    public abstract DescriptionString GetDescription(CultureInfo? culture = null);

    /// <inheritdoc cref="IEffect.GetDescription(CultureInfo)"/>
    protected DescriptionString GetDescription<T>(CultureInfo? culture, T parameter)
    {
        return GetDescription(culture, parameter?.ToString() ?? string.Empty);
    }

    /// <inheritdoc cref="IEffect.GetDescription(CultureInfo)"/>
    protected DescriptionString GetDescription<T0, T1>(CultureInfo? culture, T0 parameter0, T1 parameter1)
    {
        return GetDescription(culture,
            parameter0?.ToString() ?? string.Empty,
            parameter1?.ToString() ?? string.Empty);
    }

    /// <inheritdoc cref="IEffect.GetDescription(CultureInfo)"/>
    protected DescriptionString GetDescription<T0, T1, T2>(CultureInfo? culture, T0 parameter0, T1 parameter1, T2 parameter2)
    {
        return GetDescription(culture,
            parameter0?.ToString() ?? string.Empty,
            parameter1?.ToString() ?? string.Empty,
            parameter2?.ToString() ?? string.Empty);
    }

    /// <inheritdoc cref="IEffect.GetDescription(CultureInfo)"/>
    protected DescriptionString GetDescription<T0, T1, T2, T3>(CultureInfo? culture, T0 parameter0, T1 parameter1, T2 parameter2, T3 parameter3)
    {
        return GetDescription(culture,
            parameter0?.ToString() ?? string.Empty,
            parameter1?.ToString() ?? string.Empty,
            parameter2?.ToString() ?? string.Empty,
            parameter3?.ToString() ?? string.Empty);
    }

    /// <inheritdoc cref="IEffect.GetDescription(CultureInfo)"/>
    [OverloadResolutionPriority(1)]
    protected DescriptionString GetDescription(CultureInfo? culture, params IReadOnlyList<string> parameters)
    {
        var effectData = GetEffectData();
        if (effectData is null)
        {
            if (this is not UntranslatedEffect)
            {
                Log.Warning("Unknown EffectData {@Effect}", this);
            }

            return new DescriptionString(Translation.Get<ApiTranslations>("Effect.Unknown", culture),
                Id.ToString(),
                string.Join(',', parameters));
        }

        StringBuilder builder = new();

        if (Probability > 0)
        {
            builder.Append(Translation.Format(Translation.Get<ApiTranslations>("Effect.Probability", culture), Probability));
            builder.Append(" : ");
        }

        builder.Append(effectData.Description.ToString(culture));

        if (Duration <= -1 || Duration >= 63)
        {
            builder.Append(" (");
            builder.Append(Translation.Get<ApiTranslations>("ShortInfinity", culture));
            builder.Append(')');
        }
        else if (Duration != 0)
        {
            builder.Append(" (");
            builder.Append(Translation.Format(Translation.Get<ApiTranslations>("Effect.Turn", culture), Duration));
            builder.Append(')');
        }

        return new DescriptionString(builder.ToString(), parameters);
    }

    public int CompareTo(IEffect? other)
    {
        if (other is null)
        {
            return 1;
        }

        var effectData = GetEffectData();
        var otherEffectData = other.GetEffectData();

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
