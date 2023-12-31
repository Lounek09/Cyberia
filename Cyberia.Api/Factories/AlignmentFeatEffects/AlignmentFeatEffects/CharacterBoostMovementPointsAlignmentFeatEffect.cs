namespace Cyberia.Api.Factories.AlignmentFeatEffects;

public sealed record CharacterBoostMovementPointsAlignmentFeatEffect : AlignmentFeatEffect, IAlignmentFeatEffect<CharacterBoostMovementPointsAlignmentFeatEffect>
{
    public int Value { get; init; }

    public CharacterBoostMovementPointsAlignmentFeatEffect(int id, int value)
        : base(id)
    {
        Value = value;
    }

    public static CharacterBoostMovementPointsAlignmentFeatEffect? Create(int effectId, params int[] parameters)
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
