namespace Cyberia.Api.Factories.AlignmentFeatEffects;

public sealed record CharacterBoostRangeAlignmentFeatEffect : AlignmentFeatEffect
{
    public int Value { get; init; }

    public CharacterBoostRangeAlignmentFeatEffect(int id, int value)
        : base(id)
    {
        Value = value;
    }

    internal static CharacterBoostRangeAlignmentFeatEffect? Create(int effectId, params int[] parameters)
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
