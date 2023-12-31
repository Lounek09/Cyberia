namespace Cyberia.Api.Factories.AlignmentFeatEffects;

public sealed record CharacterBoostVitalityAlignmentFeatEffect : AlignmentFeatEffect, IAlignmentFeatEffect<CharacterBoostVitalityAlignmentFeatEffect>
{
    public int Value { get; init; }

    public CharacterBoostVitalityAlignmentFeatEffect(int id, int value)
        : base(id)
    {
        Value = value;
    }

    public static CharacterBoostVitalityAlignmentFeatEffect? Create(int effectId, params int[] parameters)
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
