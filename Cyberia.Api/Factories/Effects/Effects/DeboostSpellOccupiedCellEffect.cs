using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record DeboostSpellOccupiedCellEffect : SpellModifierEffect, IEffect<DeboostSpellOccupiedCellEffect>
{
    private DeboostSpellOccupiedCellEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int spellId, int value)
        : base(effectId, duration, probability, criteria, effectArea, spellId, value)
    {
    
    }

    public static DeboostSpellOccupiedCellEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param1, parameters.Param3);
    }
}
