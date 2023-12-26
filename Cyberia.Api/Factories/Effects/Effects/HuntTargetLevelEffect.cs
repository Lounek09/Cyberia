using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record HuntTargetLevelEffect : Effect, IEffect<HuntTargetLevelEffect>
{
    public int Level { get; init; }

    private HuntTargetLevelEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int level)
        : base(id, duration, probability, criteria, effectArea)
    {
        Level = level;
    }

    public static HuntTargetLevelEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param3);
    }

    public Description GetDescription()
    {
        return GetDescription(null, null, Level);
    }
}
