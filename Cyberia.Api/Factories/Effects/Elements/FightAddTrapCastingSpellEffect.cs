using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record FightAddTrapCastingSpellEffect : TrapEffect, IEffect
{
    private FightAddTrapCastingSpellEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int spellId, int level)
        : base(id, duration, probability, criteria, effectArea, spellId, level)
    {

    }

    internal static FightAddTrapCastingSpellEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param1, (int)parameters.Param2);
    }
}
