namespace Cyberia.Api.Factories.AlignmentFeatEffects;

public sealed record CharacterBoostIntelligenceAlignmentFeatEffect : AlignmentFeatEffect, IAlignmentFeatEffect<CharacterBoostIntelligenceAlignmentFeatEffect>
{
    public int Value { get; init; }

    public CharacterBoostIntelligenceAlignmentFeatEffect(int id, int value)
        : base(id)
    {
        Value = value;
    }

    public static CharacterBoostIntelligenceAlignmentFeatEffect? Create(int effectId, params int[] parameters)
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
