namespace Cyberia.Api.Factories.AlignmentFeatEffects;

public sealed record CharacterBoostMaximumEnergyPointsAlignmentFeatEffect : AlignmentFeatEffect, IAlignmentFeatEffect<CharacterBoostMaximumEnergyPointsAlignmentFeatEffect>
{
    public int Value { get; init; }

    public CharacterBoostMaximumEnergyPointsAlignmentFeatEffect(int id, int value)
        : base(id)
    {
        Value = value;
    }

    public static CharacterBoostMaximumEnergyPointsAlignmentFeatEffect? Create(int effectId, params int[] parameters)
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
