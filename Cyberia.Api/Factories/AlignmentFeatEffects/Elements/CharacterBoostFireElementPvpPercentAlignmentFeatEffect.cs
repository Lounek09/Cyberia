namespace Cyberia.Api.Factories.AlignmentFeatEffects;

public sealed record CharacterBoostFireElementPvpPercentAlignmentFeatEffect : AlignmentFeatEffect, IAlignmentFeatEffect
{
    public int Value { get; init; }

    public CharacterBoostFireElementPvpPercentAlignmentFeatEffect(int id, int value)
        : base(id)
    {
        Value = value;
    }

    internal static CharacterBoostFireElementPvpPercentAlignmentFeatEffect? Create(int effectId, params int[] parameters)
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
