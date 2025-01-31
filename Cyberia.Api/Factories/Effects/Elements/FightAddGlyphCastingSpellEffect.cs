﻿using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Factories.Effects.Templates;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record FightAddGlyphCastingSpellEffect : GlyphEffect
{
    private FightAddGlyphCastingSpellEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea, int spellId, int level)
        : base(id, duration, probability, criteria, dispellable, effectArea, spellId, level)
    {

    }

    internal static FightAddGlyphCastingSpellEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, dispellable, effectArea, (int)parameters.Param1, (int)parameters.Param2);
    }
}
