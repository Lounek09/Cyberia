namespace Cyberia.Api.Factories.AlignmentFeatEffects;

public sealed record CharacterBoostMagicFindAlignmentFeatEffect : AlignmentFeatEffect, IAlignmentFeatEffect<CharacterBoostMagicFindAlignmentFeatEffect>
{
    public int Value { get; init; }

    public CharacterBoostMagicFindAlignmentFeatEffect(int id, int value)
        : base(id)
    {
        Value = value;
    }

    public static CharacterBoostMagicFindAlignmentFeatEffect? Create(int effectId, params int[] parameters)
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
