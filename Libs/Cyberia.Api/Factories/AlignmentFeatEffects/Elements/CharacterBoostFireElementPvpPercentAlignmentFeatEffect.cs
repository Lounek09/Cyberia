using System.Globalization;

namespace Cyberia.Api.Factories.AlignmentFeatEffects.Elements;

public sealed record CharacterBoostFireElementPvpPercentAlignmentFeatEffect : AlignmentFeatEffect
{
    public int Value { get; init; }

    public CharacterBoostFireElementPvpPercentAlignmentFeatEffect(int id, int value)
        : base(id)
    {
        Value = value;
    }

    internal static CharacterBoostFireElementPvpPercentAlignmentFeatEffect? Create(int effectId, params ReadOnlySpan<int> parameters)
    {
        if (parameters.Length > 0)
        {
            return new(effectId, parameters[0]);
        }

        return null;
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, Value);
    }
}
