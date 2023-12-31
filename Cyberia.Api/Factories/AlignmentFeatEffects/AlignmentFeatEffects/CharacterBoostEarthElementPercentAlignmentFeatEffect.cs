namespace Cyberia.Api.Factories.AlignmentFeatEffects;

public sealed record CharacterBoostEarthElementPercentAlignmentFeatEffect : AlignmentFeatEffect, IAlignmentFeatEffect<CharacterBoostEarthElementPercentAlignmentFeatEffect>
{
    public int Value { get; init; }

    public CharacterBoostEarthElementPercentAlignmentFeatEffect(int id, int value)
        : base(id)
    {
        Value = value;
    }

    public static CharacterBoostEarthElementPercentAlignmentFeatEffect? Create(int effectId, params int[] parameters)
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
