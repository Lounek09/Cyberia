using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterManaUseKillLifeFireEffect : Effect
{
    public int ActionPoints { get; init; }
    public int Damage { get; init; }

    private CharacterManaUseKillLifeFireEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int actionPoints, int damage)
        : base(id, duration, probability, criteria, effectArea)
    {
        ActionPoints = actionPoints;
        Damage = damage;
    }

    internal static CharacterManaUseKillLifeFireEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param1, (int)parameters.Param2);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, ActionPoints, Damage);
    }
}
