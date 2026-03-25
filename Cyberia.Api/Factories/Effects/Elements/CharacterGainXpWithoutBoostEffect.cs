using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterGainXpWithoutBoostEffect : Effect
{
    public long Value { get; init; }

    private CharacterGainXpWithoutBoostEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea, long value)
        : base(id, duration, probability, criteria, dispellable, effectArea)
    {
        Value = value;
    }

    internal static CharacterGainXpWithoutBoostEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea)
    {
        var value = parameters.Param1 * (int)Math.Pow(32, 6) + parameters.Param2 * (int)Math.Pow(32, 3) + parameters.Param3;

        return new(effectId, duration, probability, criteria, dispellable, effectArea, value);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, Value.ToFormattedString(culture));
    }
}
