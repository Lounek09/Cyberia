namespace Cyberia.Api.Factories.AlignmentFeatEffects;

public sealed record CharacterBoostNeutralElementPvpResistAlignmentFeatEffect : AlignmentFeatEffect, IAlignmentFeatEffect<CharacterBoostNeutralElementPvpResistAlignmentFeatEffect>
{
    public int Value { get; init; }

    public CharacterBoostNeutralElementPvpResistAlignmentFeatEffect(int id, int value)
        : base(id)
    {
        Value = value;
    }

    public static CharacterBoostNeutralElementPvpResistAlignmentFeatEffect? Create(int effectId, params int[] parameters)
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
