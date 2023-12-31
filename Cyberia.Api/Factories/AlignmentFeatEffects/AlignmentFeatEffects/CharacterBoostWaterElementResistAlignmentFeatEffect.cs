namespace Cyberia.Api.Factories.AlignmentFeatEffects;

public sealed record CharacterBoostWaterElementResistAlignmentFeatEffect : AlignmentFeatEffect, IAlignmentFeatEffect<CharacterBoostWaterElementResistAlignmentFeatEffect>
{
    public int Value { get; init; }

    public CharacterBoostWaterElementResistAlignmentFeatEffect(int id, int value)
        : base(id)
    {
        Value = value;
    }

    public static CharacterBoostWaterElementResistAlignmentFeatEffect? Create(int effectId, params int[] parameters)
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
