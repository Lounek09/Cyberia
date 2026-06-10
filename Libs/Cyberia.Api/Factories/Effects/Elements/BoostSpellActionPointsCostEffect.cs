using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Factories.Effects.Templates;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record BoostSpellActionPointsCostEffect : SpellModifierEffect
{
    private BoostSpellActionPointsCostEffect(int id, int spellId, int value)
        : base(id, spellId, value) { }

    internal static BoostSpellActionPointsCostEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param1, (int)parameters.Param3);
    }
}
