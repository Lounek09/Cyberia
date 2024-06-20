using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterMultiplyDamagesEffect : Effect, IEffect
{
    public int Multiplier { get; init; }

    private CharacterMultiplyDamagesEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int multiplier)
        : base(id, duration, probability, criteria, effectArea)
    {
        Multiplier = multiplier;
    }

    internal static CharacterMultiplyDamagesEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param1);
    }

    public Description GetDescription()
    {
        return GetDescription(Multiplier);
    }
}
