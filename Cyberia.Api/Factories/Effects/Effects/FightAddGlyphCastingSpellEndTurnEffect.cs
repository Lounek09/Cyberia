using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record FightAddGlyphCastingSpellEndTurnEffect : GlyphEffect, IEffect<FightAddGlyphCastingSpellEndTurnEffect>
{
    private FightAddGlyphCastingSpellEndTurnEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int spellId, int level)
        : base(effectId, duration, probability, criteria, effectArea, spellId, level)
    {

    }

    public static FightAddGlyphCastingSpellEndTurnEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param1, parameters.Param2);
    }
}
