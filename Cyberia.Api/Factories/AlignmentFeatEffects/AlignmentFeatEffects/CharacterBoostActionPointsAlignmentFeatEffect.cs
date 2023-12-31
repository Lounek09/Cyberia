namespace Cyberia.Api.Factories.AlignmentFeatEffects;

public sealed record CharacterBoostActionPointsAlignmentFeatEffect : AlignmentFeatEffect, IAlignmentFeatEffect<CharacterBoostActionPointsAlignmentFeatEffect>
{
    public int Value { get; init; }

    public CharacterBoostActionPointsAlignmentFeatEffect(int id, int value)
        : base(id)
    {
        Value = value;
    }

    public static CharacterBoostActionPointsAlignmentFeatEffect? Create(int effectId, params int[] parameters)
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
