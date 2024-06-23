namespace Cyberia.Api.Factories.AlignmentFeatEffects;

public sealed record CharacterBoostEarthElementPvpPercentAlignmentFeatEffect : AlignmentFeatEffect
{
    public int Value { get; init; }

    public CharacterBoostEarthElementPvpPercentAlignmentFeatEffect(int id, int value)
        : base(id)
    {
        Value = value;
    }

    internal static CharacterBoostEarthElementPvpPercentAlignmentFeatEffect? Create(int effectId, params int[] parameters)
    {
        if (parameters.Length > 0)
        {
            return new(effectId, parameters[0]);
        }

        return null;
    }

    public override Description GetDescription()
    {
        return GetDescription(Value);
    }
}
