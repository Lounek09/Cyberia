namespace Cyberia.Api.Factories.AlignmentFeatEffects;

public sealed record CharacterBoostNeutralElementPercentAlignmentFeatEffect : AlignmentFeatEffect, IAlignmentFeatEffect<CharacterBoostNeutralElementPercentAlignmentFeatEffect>
{
    public int Value { get; init; }

    public CharacterBoostNeutralElementPercentAlignmentFeatEffect(int id, int value)
        : base(id)
    {
        Value = value;
    }

    public static CharacterBoostNeutralElementPercentAlignmentFeatEffect? Create(int effectId, params int[] parameters)
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
