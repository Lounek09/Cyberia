using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterGainXpWithoutBoostEffect : Effect
{
    public long Value { get; init; }

    private CharacterGainXpWithoutBoostEffect(int id, long value)
        : base(id)
    {
        Value = value;
    }

    internal static CharacterGainXpWithoutBoostEffect Create(int effectId, EffectParameters parameters)
    {
        var value = parameters.Param1 * (int)Math.Pow(32, 6) + parameters.Param2 * (int)Math.Pow(32, 3) + parameters.Param3;

        return new(effectId, value);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, Value.ToFormattedString(culture));
    }
}
