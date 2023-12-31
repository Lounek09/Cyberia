namespace Cyberia.Api.Factories.AlignmentFeatEffects;

public sealed record CharacterBoostChanceAlignmentFeatEffect : AlignmentFeatEffect, IAlignmentFeatEffect<CharacterBoostChanceAlignmentFeatEffect>
{
    public int Value { get; init; }

    public CharacterBoostChanceAlignmentFeatEffect(int id, int value)
        : base(id)
    {
        Value = value;
    }

    public static CharacterBoostChanceAlignmentFeatEffect? Create(int effectId, params int[] parameters)
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
