using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Values;

namespace Cyberia.Api.Factories.Effects;

public sealed record ItemLivingMoodEffect : Effect, IEffect
{
    public Corpulence Corpulence { get; init; }

    private ItemLivingMoodEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, Corpulence corpulence)
        : base(id, duration, probability, criteria, effectArea)
    {
        Corpulence = corpulence;
    }

    internal static ItemLivingMoodEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (Corpulence)parameters.Param3);
    }

    public Description GetDescription()
    {
        return GetDescription(string.Empty, string.Empty, Corpulence.GetDescription());
    }
}
