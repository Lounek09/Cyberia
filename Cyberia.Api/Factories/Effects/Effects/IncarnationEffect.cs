using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record IncarnationEffect : Effect, IEffect<IncarnationEffect>
{
    public int Level { get; init; }

    private IncarnationEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int level)
        : base(effectId, duration, probability, criteria, effectArea)
    {
        Level = level;
    }

    public static IncarnationEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param3);
    }

    public Description GetDescription()
    {
        return GetDescription(null, null, Level);
    }
}
