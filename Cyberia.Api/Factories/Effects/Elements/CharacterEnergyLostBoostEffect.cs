using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterEnergyLossBoostEffect
    : Effect, IEffect
{
    public int EnergyLoss { get; init; }

    private CharacterEnergyLossBoostEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int energyLoss)
        : base(id, duration, probability, criteria, effectArea)
    {
        EnergyLoss = energyLoss;
    }

    internal static CharacterEnergyLossBoostEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param1);
    }

    public Description GetDescription()
    {
        return GetDescription(EnergyLoss);
    }
}
