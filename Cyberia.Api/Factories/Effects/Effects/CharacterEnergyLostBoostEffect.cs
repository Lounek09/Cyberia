using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterEnergyLossBoostEffect : Effect, IEffect<CharacterEnergyLossBoostEffect>
{
    public int EnergyLoss { get; init; }

    private CharacterEnergyLossBoostEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int energyLoss)
        : base(effectId, duration, probability, criteria, effectArea)
    {
        EnergyLoss = energyLoss;
    }

    public static CharacterEnergyLossBoostEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param1);
    }

    public Description GetDescription()
    {
        return GetDescription(EnergyLoss);
    }
}
