﻿using System.Globalization;

namespace Cyberia.Api.Factories.AlignmentFeatEffects.Elements;

public sealed record CharacterBoostFireElementPvpResistAlignmentFeatEffect : AlignmentFeatEffect
{
    public int Value { get; init; }

    public CharacterBoostFireElementPvpResistAlignmentFeatEffect(int id, int value)
        : base(id)
    {
        Value = value;
    }

    internal static CharacterBoostFireElementPvpResistAlignmentFeatEffect? Create(int effectId, params ReadOnlySpan<int> parameters)
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
