namespace Cyberia.Api.Factories.AlignmentFeatEffects;

public sealed record CharacterBoostAgilityAlignmentFeatEffect : AlignmentFeatEffect, IAlignmentFeatEffect<CharacterBoostAgilityAlignmentFeatEffect>
{
    public int Value { get; init; }

    public CharacterBoostAgilityAlignmentFeatEffect(int id, int value)
        : base(id)
    {
        Value = value;
    }

    public static CharacterBoostAgilityAlignmentFeatEffect? Create(int effectId, params int[] parameters)
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
