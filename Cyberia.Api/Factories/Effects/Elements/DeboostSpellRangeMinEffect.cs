﻿using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record DeboostSpellRangeMinEffect
    : SpellModifierEffect, IEffect
{
    private DeboostSpellRangeMinEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int spellId, int value)
        : base(id, duration, probability, criteria, effectArea, spellId, value)
    {
    
    }

    internal static DeboostSpellRangeMinEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param1, parameters.Param3);
    }
}