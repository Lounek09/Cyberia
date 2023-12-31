namespace Cyberia.Api.Factories.AlignmentFeatEffects;

public sealed record CharacterBoostWisdomAlignmentFeatEffect : AlignmentFeatEffect, IAlignmentFeatEffect<CharacterBoostWisdomAlignmentFeatEffect>
{
    public int Value { get; init; }

    public CharacterBoostWisdomAlignmentFeatEffect(int id, int value)
        : base(id)
    {
        Value = value;
    }

    public static CharacterBoostWisdomAlignmentFeatEffect? Create(int effectId, params int[] parameters)
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
