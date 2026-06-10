using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Factories.Effects.Templates;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record DeboostSpellActionPointsCostEffect : SpellModifierEffect
{
    private DeboostSpellActionPointsCostEffect(int id, int spellId, int value)
        : base(id, spellId, value) { }

    internal static DeboostSpellActionPointsCostEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param1, (int)parameters.Param3);
    }
}
