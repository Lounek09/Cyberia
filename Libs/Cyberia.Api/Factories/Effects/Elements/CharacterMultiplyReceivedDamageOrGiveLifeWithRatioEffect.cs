using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterMultiplyReceivedDamageOrGiveLifeWithRatioEffect : Effect
{
    public int DamageRatio { get; init; }
    public int HealRatio { get; init; }
    public int DamageProbability { get; init; }

    private CharacterMultiplyReceivedDamageOrGiveLifeWithRatioEffect(int id, int damageRatio, int healRatio, int damageProbability)
        : base(id)
    {
        DamageRatio = damageRatio;
        HealRatio = healRatio;
        DamageProbability = damageProbability;
    }

    internal static CharacterMultiplyReceivedDamageOrGiveLifeWithRatioEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param1, (int)parameters.Param2, (int)parameters.Param3);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, DamageRatio, HealRatio, DamageProbability);
    }
}
