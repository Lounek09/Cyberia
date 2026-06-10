using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Factories.Effects.Templates;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record FightAddGlyphCastingSpellEndTurnEffect : GlyphEffect
{
    private FightAddGlyphCastingSpellEndTurnEffect(int id, int spellId, int level)
        : base(id, spellId, level) { }

    internal static FightAddGlyphCastingSpellEndTurnEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param1, (int)parameters.Param2);
    }
}
