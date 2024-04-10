namespace Cyberia.Api.Factories.AlignmentFeatEffects;

public sealed record CharacterBoostFireElementPvpResistAlignmentFeatEffect : AlignmentFeatEffect, IAlignmentFeatEffect
{
    public int Value { get; init; }

    public CharacterBoostFireElementPvpResistAlignmentFeatEffect(int id, int value)
        : base(id)
    {
        Value = value;
    }

    internal static CharacterBoostFireElementPvpResistAlignmentFeatEffect? Create(int effectId, params int[] parameters)
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
