using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record GainAuraEffect : Effect, IEffect<GainAuraEffect>
{
    public int AuraId { get; init; }

    private GainAuraEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int auraId)
        : base(effectId, duration, probability, criteria, effectArea)
    {
        AuraId = auraId;
    }

    public static GainAuraEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param3);
    }

    public Description GetDescription()
    {
        return GetDescription(null, null, AuraId);
    }
}
