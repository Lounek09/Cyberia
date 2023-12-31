namespace Cyberia.Api.Factories.AlignmentFeatEffects;

public sealed record CharacterBoostFireElementPvpResistAlignmentFeatEffect : AlignmentFeatEffect, IAlignmentFeatEffect<CharacterBoostFireElementPvpResistAlignmentFeatEffect>
{
    public int Value { get; init; }

    public CharacterBoostFireElementPvpResistAlignmentFeatEffect(int id, int value)
        : base(id)
    {
        Value = value;
    }

    public static CharacterBoostFireElementPvpResistAlignmentFeatEffect? Create(int effectId, params int[] parameters)
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
