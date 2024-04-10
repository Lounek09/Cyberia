using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterLifeLostPercentReductorEffect
    : Effect, IEffect
{
    public int PercentReductor { get; init; }

    private CharacterLifeLostPercentReductorEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int percentModerator)
        : base(id, duration, probability, criteria, effectArea)
    {
        PercentReductor = percentModerator;
    }

    internal static CharacterLifeLostPercentReductorEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param1);
    }

    public Description GetDescription()
    {
        return GetDescription(PercentReductor);
    }
}
