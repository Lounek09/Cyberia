using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterMultiplyReceivedDamageOrGiveLifeWithRatioEffect : Effect, IEffect<CharacterMultiplyReceivedDamageOrGiveLifeWithRatioEffect>
{
    public int DamageRatio { get; init; }
    public int HealRatio { get; init; }
    public int DamageProbability { get; init; }

    private CharacterMultiplyReceivedDamageOrGiveLifeWithRatioEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int damageRatio, int healRatio, int damageProbability)
        : base(id, duration, probability, criteria, effectArea)
    {
        DamageRatio = damageRatio;
        HealRatio = healRatio;
        DamageProbability = damageProbability;
    }

    public static CharacterMultiplyReceivedDamageOrGiveLifeWithRatioEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param1, parameters.Param2, parameters.Param3);
    }

    public Description GetDescription()
    {
        return GetDescription(DamageRatio, HealRatio, DamageProbability);
    }
}
