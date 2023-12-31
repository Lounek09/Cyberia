namespace Cyberia.Api.Factories.AlignmentFeatEffects;

public sealed record CharacterBoostDamagesAlignmentFeatEffect : AlignmentFeatEffect, IAlignmentFeatEffect<CharacterBoostDamagesAlignmentFeatEffect>
{
    public int Value { get; init; }

    public CharacterBoostDamagesAlignmentFeatEffect(int id, int value)
        : base(id)
    {
        Value = value;
    }

    public static CharacterBoostDamagesAlignmentFeatEffect? Create(int effectId, params int[] parameters)
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
