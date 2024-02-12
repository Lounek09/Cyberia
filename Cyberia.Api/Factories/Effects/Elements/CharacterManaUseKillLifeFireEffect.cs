using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterManaUseKillLifeFireEffect
    : Effect, IEffect
{
    public int ActionPoints { get; init; }
    public int Damage { get; init; }

    private CharacterManaUseKillLifeFireEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int actionPoints, int damage)
        : base(id, duration, probability, criteria, effectArea)
    {
        ActionPoints = actionPoints;
        Damage = damage;
    }

    internal static CharacterManaUseKillLifeFireEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param1, (int)parameters.Param2);
    }

    public Description GetDescription()
    {
        return GetDescription(ActionPoints, Damage);
    }
}
