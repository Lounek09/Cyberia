﻿namespace Cyberia.Api.Factories.AlignmentFeatEffects;

public sealed record CharacterBoostStrengthAlignmentFeatEffect
    : AlignmentFeatEffect, IAlignmentFeatEffect
{
    public int Value { get; init; }

    public CharacterBoostStrengthAlignmentFeatEffect(int id, int value)
        : base(id)
    {
        Value = value;
    }

    internal static CharacterBoostStrengthAlignmentFeatEffect? Create(int effectId, params int[] parameters)
    {
        if (parameters.Length > 0)
        {
            return new(effectId, parameters[0]);
        }

        return null;
    }

    public Description GetDescription()
    {
        return GetDescription(Value);
    }
}