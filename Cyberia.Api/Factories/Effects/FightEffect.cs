using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Langzilla.Primitives;

using System.Globalization;
using System.Text;

namespace Cyberia.Api.Factories.Effects;

/// <summary>
/// Represents an effect in a fight.
/// </summary>
//[JsonConverter(typeof(FightEffectConverter))]
public sealed record FightEffect
{
    /// <summary>
    /// Gets the base effect instance.
    /// </summary>
    public Effect Effect { get; init; }

    /// <summary>
    /// Gets the duration of the effect.
    /// </summary>
    public int Duration { get; init; }

    /// <summary>
    /// Gets the probability of the effect in percentage.
    /// </summary>
    public int Probability { get; init; }

    /// <summary>
    /// Gets the criteria where the effect is applicable.
    /// </summary>
    public CriterionReadOnlyCollection Criteria { get; init; }

    /// <summary>
    /// Gets a value indicating whether the effect is dispellable.
    /// </summary>
    public bool Dispellable { get; init; }

    /// <summary>
    /// Gets the area of the effect.
    /// </summary>
    public EffectArea EffectArea { get; init; }

    /// <inheritdoc cref="FightEffect(Effect, int, int, CriterionReadOnlyCollection, bool, EffectArea)"/>
    internal FightEffect(Effect effect)
        : this(effect, 0, 0, CriterionReadOnlyCollection.Empty, true, EffectAreaFactory.Default) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="FightEffect"/> record.
    /// </summary>
    /// <param name="effect">The base effect.</param>
    /// <param name="duration">The duration of the effect.</param>
    /// <param name="probability">The probability (as a percentage) that the effect will occur.</param>
    /// <param name="criteria">The criteria where the effect is applicable.</param>
    /// <param name="effectArea">The area of the effect.</param>
    public FightEffect(Effect effect, int duration, int probability, CriterionReadOnlyCollection criteria, bool dispellable, EffectArea effectArea)
    {

        Effect = effect;
        Duration = duration;
        Probability = probability;
        Criteria = criteria;
        Dispellable = dispellable;
        EffectArea = effectArea;
    }

    /// <inheritdoc cref="Effect.GetDescription(Language)"/>
    public DescriptionString GetDescription(Language language)
    {
        return GetDescription(language.ToCulture());
    }

    /// <inheritdoc cref="Effect.GetDescription(CultureInfo)"/>
    public DescriptionString GetDescription(CultureInfo? culture)
    {
        var effectDescription = Effect.GetDescription(culture);

        var hasProbability = Probability > 0;
        var hasDuration = Duration != 0;

        if (!hasProbability && !hasDuration)
        {
            return effectDescription;
        }

        StringBuilder builder = new();

        if (hasProbability)
        {
            builder.Append(Translation.Format(Translation.Get<ApiTranslations>("Effect.Probability", culture), Probability));
            builder.Append(" : ");
        }

        builder.Append(effectDescription.Template);

        if (hasDuration)
        {
            builder.Append(" (");
            builder.Append(Duration <= -1 || Duration >= 63
                ? Translation.Get<ApiTranslations>("ShortInfinity", culture)
                : Translation.Format(Translation.Get<ApiTranslations>("Effect.Turn", culture), Duration));
            builder.Append(')');
        }

        return new DescriptionString(builder.ToString(), effectDescription.Parameters);
    }
}
