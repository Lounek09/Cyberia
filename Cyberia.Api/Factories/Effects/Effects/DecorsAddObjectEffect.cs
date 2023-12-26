using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record DecorsAddObjectEffect : Effect, IEffect<DecorsAddObjectEffect>
{
    public int GfxId { get; init; }

    private DecorsAddObjectEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int gfxId)
        : base(id, duration, probability, criteria, effectArea)
    {
        GfxId = gfxId;
    }

    public static DecorsAddObjectEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        // Param3 is a supposition
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param3);
    }

    public Description GetDescription()
    {
        return GetDescription(null, null, GfxId);
    }
}
