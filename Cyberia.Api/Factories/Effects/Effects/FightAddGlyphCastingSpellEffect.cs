using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record FightAddGlyphCastingSpellEffect : GlyphEffect, IEffect<FightAddGlyphCastingSpellEffect>
{
    private FightAddGlyphCastingSpellEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int spellId, int level)
        : base(effectId, duration, probability, criteria, effectArea, spellId, level)
    {

    }

    public static FightAddGlyphCastingSpellEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param1, parameters.Param2);
    }
}
