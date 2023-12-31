namespace Cyberia.Api.Factories.AlignmentFeatEffects;

public sealed record CharacterBoostWaterElementPvpPercentAlignmentFeatEffect : AlignmentFeatEffect, IAlignmentFeatEffect<CharacterBoostWaterElementPvpPercentAlignmentFeatEffect>
{
    public int Value { get; init; }

    public CharacterBoostWaterElementPvpPercentAlignmentFeatEffect(int id, int value)
        : base(id)
    {
        Value = value;
    }

    public static CharacterBoostWaterElementPvpPercentAlignmentFeatEffect? Create(int effectId, params int[] parameters)
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
