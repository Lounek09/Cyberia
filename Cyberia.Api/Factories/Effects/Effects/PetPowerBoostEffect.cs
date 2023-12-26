using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record PetPowerBoostEffect : Effect, IEffect<PetPowerBoostEffect>
{
    public int Power { get; init; }

    private PetPowerBoostEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int power)
        : base(id, duration, probability, criteria, effectArea)
    {
        Power = power;
    }

    public static PetPowerBoostEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param3);
    }

    public Description GetDescription()
    {
        return GetDescription(null, null, Power);
    }
}
