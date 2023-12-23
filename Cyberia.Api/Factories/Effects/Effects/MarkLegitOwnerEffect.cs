using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record MarkLegitOwnerEffect : Effect, IEffect<MarkLegitOwnerEffect>
{
    public string Name { get; init; }

    private MarkLegitOwnerEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, string name)
        : base(effectId, duration, probability, criteria, effectArea)
    {
        Name = name;
    }

    public static MarkLegitOwnerEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param4);
    }

    public Description GetDescription()
    {
        return GetDescription(null, null, null, Name);
    }
}
