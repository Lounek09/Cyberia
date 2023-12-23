using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterBoostRangeForAllSpellEffect : Effect, IEffect<CharacterBoostRangeForAllSpellEffect>
{
    public int Range { get; init; }

    private CharacterBoostRangeForAllSpellEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int range)
        : base(effectId, duration, probability, criteria, effectArea)
    {
        Range = range;
    }

    public static CharacterBoostRangeForAllSpellEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param3);
    }

    public Description GetDescription()
    {
        return GetDescription(null, null, Range);
    }
}
