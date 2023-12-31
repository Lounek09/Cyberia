namespace Cyberia.Api.Factories.AlignmentFeatEffects;

public sealed record CharacterBoostActionPointsLostAlignmentFeatEffect : AlignmentFeatEffect, IAlignmentFeatEffect<CharacterBoostActionPointsLostAlignmentFeatEffect>
{
    public int Value { get; init; }

    public CharacterBoostActionPointsLostAlignmentFeatEffect(int id, int value)
        : base(id)
    {
        Value = value;
    }

    public static CharacterBoostActionPointsLostAlignmentFeatEffect? Create(int effectId, params int[] parameters)
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
