using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record PetPowerBoostEffect
    : Effect, IEffect
{
    public int Power { get; init; }

    private PetPowerBoostEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int power)
        : base(id, duration, probability, criteria, effectArea)
    {
        Power = power;
    }

    internal static PetPowerBoostEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param3);
    }

    public Description GetDescription()
    {
        return GetDescription(string.Empty, string.Empty, Power);
    }
}
