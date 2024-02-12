using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record FightAddGlyphCastingSpellEndTurnEffect
    : GlyphEffect, IEffect
{
    private FightAddGlyphCastingSpellEndTurnEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int spellId, int level)
        : base(id, duration, probability, criteria, effectArea, spellId, level)
    {

    }

    internal static FightAddGlyphCastingSpellEndTurnEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param1, (int)parameters.Param2);
    }
}
