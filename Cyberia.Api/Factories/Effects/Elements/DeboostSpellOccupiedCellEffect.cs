using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record DeboostSpellOccupiedCellEffect
    : SpellModifierEffect, IEffect
{
    private DeboostSpellOccupiedCellEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int spellId, int value)
        : base(id, duration, probability, criteria, effectArea, spellId, value)
    {

    }

    internal static DeboostSpellOccupiedCellEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param1, (int)parameters.Param3);
    }
}
