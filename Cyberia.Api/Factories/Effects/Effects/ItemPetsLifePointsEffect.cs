using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record ItemPetsLifePointsEffect : Effect, IEffect<ItemPetsLifePointsEffect>
{
    public int LifePoints { get; init; }

    private ItemPetsLifePointsEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int lifePoints)
        : base(id, duration, probability, criteria, effectArea)
    {
        LifePoints = lifePoints;
    }

    public static ItemPetsLifePointsEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param3);
    }

    public Description GetDescription()
    {
        return GetDescription(null, null, LifePoints);
    }
}
