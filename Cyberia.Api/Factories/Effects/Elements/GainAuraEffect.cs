using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record GainAuraEffect
    : Effect, IEffect
{
    public int AuraId { get; init; }

    private GainAuraEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int auraId)
        : base(id, duration, probability, criteria, effectArea)
    {
        AuraId = auraId;
    }

    internal static GainAuraEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param3);
    }

    public Description GetDescription()
    {
        return GetDescription(string.Empty, string.Empty, AuraId);
    }
}
