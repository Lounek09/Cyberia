using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Factories.Effects.Templates;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record BoostSpellBaseDamageEffect : SpellModifierEffect
{
    private BoostSpellBaseDamageEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int spellId, int value)
        : base(id, duration, probability, criteria, effectArea, spellId, value)
    {

    }

    internal static BoostSpellBaseDamageEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param1, (int)parameters.Param3);
    }
}
