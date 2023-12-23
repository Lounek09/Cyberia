using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterReduceWeaponCostEffect : Effect, IEffect<CharacterReduceWeaponCostEffect>
{
    public int Value { get; init; }

    private CharacterReduceWeaponCostEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int value)
        : base(effectId, duration, probability, criteria, effectArea)
    {
        Value = value;
    }

    public static CharacterReduceWeaponCostEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param3);
    }

    public Description GetDescription()
    {
        return GetDescription(null, null, Value);
    }
}
