using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Factories.Effects.Templates;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record FightAddGlyphCastingSpellEffect : GlyphEffect
{
    private FightAddGlyphCastingSpellEffect(int id, int spellId, int level)
        : base(id, spellId, level) { }

    internal static FightAddGlyphCastingSpellEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param1, (int)parameters.Param2);
    }
}
