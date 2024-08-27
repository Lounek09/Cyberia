namespace Cyberia.Api.Factories.AlignmentFeatEffects;

public sealed record CharacterEnergyLossBoostAlignmentFeatEffect : AlignmentFeatEffect
{
    public int Value { get; init; }

    public CharacterEnergyLossBoostAlignmentFeatEffect(int id, int value)
        : base(id)
    {
        Value = value;
    }

    internal static CharacterEnergyLossBoostAlignmentFeatEffect? Create(int effectId, params int[] parameters)
    {
        if (parameters.Length > 0)
        {
            return new(effectId, parameters[0]);
        }

        return null;
    }

    public override DescriptionString GetDescription()
    {
        return GetDescription(Value);
    }
}
