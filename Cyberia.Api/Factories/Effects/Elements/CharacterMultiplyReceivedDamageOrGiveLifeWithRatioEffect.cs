using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterMultiplyReceivedDamageOrGiveLifeWithRatioEffect : Effect, IEffect
{
    public int DamageRatio { get; init; }
    public int HealRatio { get; init; }
    public int DamageProbability { get; init; }

    private CharacterMultiplyReceivedDamageOrGiveLifeWithRatioEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int damageRatio, int healRatio, int damageProbability)
        : base(id, duration, probability, criteria, effectArea)
    {
        DamageRatio = damageRatio;
        HealRatio = healRatio;
        DamageProbability = damageProbability;
    }

    internal static CharacterMultiplyReceivedDamageOrGiveLifeWithRatioEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param1, (int)parameters.Param2, (int)parameters.Param3);
    }

    public Description GetDescription()
    {
        return GetDescription(DamageRatio, HealRatio, DamageProbability);
    }
}
