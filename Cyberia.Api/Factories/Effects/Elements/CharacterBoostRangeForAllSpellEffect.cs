using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterBoostRangeForAllSpellEffect
    : Effect, IEffect
{
    public int Range { get; init; }

    private CharacterBoostRangeForAllSpellEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int range)
        : base(id, duration, probability, criteria, effectArea)
    {
        Range = range;
    }

    internal static CharacterBoostRangeForAllSpellEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param3);
    }

    public Description GetDescription()
    {
        return GetDescription(string.Empty, string.Empty, Range);
    }
}
