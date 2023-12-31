namespace Cyberia.Api.Factories.AlignmentFeatEffects;

public sealed record CharacterBoostRangeAlignmentFeatEffect : AlignmentFeatEffect, IAlignmentFeatEffect<CharacterBoostRangeAlignmentFeatEffect>
{
    public int Value { get; init; }

    public CharacterBoostRangeAlignmentFeatEffect(int id, int value)
        : base(id)
    {
        Value = value;
    }

    public static CharacterBoostRangeAlignmentFeatEffect? Create(int effectId, params int[] parameters)
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
