namespace Cyberia.Api.Factories.AlignmentFeatEffects;

public sealed record CharacterBoostNeutralElementPvpPercentAlignmentFeatEffect : AlignmentFeatEffect, IAlignmentFeatEffect<CharacterBoostNeutralElementPvpPercentAlignmentFeatEffect>
{
    public int Value { get; init; }

    public CharacterBoostNeutralElementPvpPercentAlignmentFeatEffect(int id, int value)
        : base(id)
    {
        Value = value;
    }

    public static CharacterBoostNeutralElementPvpPercentAlignmentFeatEffect? Create(int effectId, params int[] parameters)
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
