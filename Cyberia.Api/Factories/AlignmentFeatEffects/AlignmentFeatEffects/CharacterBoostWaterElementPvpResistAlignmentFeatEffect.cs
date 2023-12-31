namespace Cyberia.Api.Factories.AlignmentFeatEffects;

public sealed record CharacterBoostWaterElementPvpResistAlignmentFeatEffect : AlignmentFeatEffect, IAlignmentFeatEffect<CharacterBoostWaterElementPvpResistAlignmentFeatEffect>
{
    public int Value { get; init; }

    public CharacterBoostWaterElementPvpResistAlignmentFeatEffect(int id, int value)
        : base(id)
    {
        Value = value;
    }

    public static CharacterBoostWaterElementPvpResistAlignmentFeatEffect? Create(int effectId, params int[] parameters)
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
