namespace Cyberia.Api.Factories.AlignmentFeatEffects;

public sealed record CharacterBoostNeutralElementResistAlignmentFeatEffect : AlignmentFeatEffect, IAlignmentFeatEffect<CharacterBoostNeutralElementResistAlignmentFeatEffect>
{
    public int Value { get; init; }

    public CharacterBoostNeutralElementResistAlignmentFeatEffect(int id, int value)
        : base(id)
    {
        Value = value;
    }

    public static CharacterBoostNeutralElementResistAlignmentFeatEffect? Create(int effectId, params int[] parameters)
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
