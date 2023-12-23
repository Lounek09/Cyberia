using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterManaUseKillLifeFireEffect : Effect, IEffect<CharacterManaUseKillLifeFireEffect>
{
    public int ActionPoints { get; init; }
    public int Damage { get; init; }

    private CharacterManaUseKillLifeFireEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int actionPoints, int damage)
        : base(effectId, duration, probability, criteria, effectArea)
    {
        ActionPoints = actionPoints;
        Damage = damage;
    }

    public static CharacterManaUseKillLifeFireEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param1, parameters.Param2);
    }

    public Description GetDescription()
    {
        return GetDescription(ActionPoints, Damage);
    }
}
