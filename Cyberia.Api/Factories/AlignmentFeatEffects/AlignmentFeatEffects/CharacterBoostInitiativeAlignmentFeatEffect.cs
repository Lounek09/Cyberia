namespace Cyberia.Api.Factories.AlignmentFeatEffects;

public sealed record CharacterBoostInitiativeAlignmentFeatEffect : AlignmentFeatEffect, IAlignmentFeatEffect<CharacterBoostInitiativeAlignmentFeatEffect>
{
    public int Value { get; init; }

    public CharacterBoostInitiativeAlignmentFeatEffect(int id, int value)
        : base(id)
    {
        Value = value;
    }

    public static CharacterBoostInitiativeAlignmentFeatEffect? Create(int effectId, params int[] parameters)
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
