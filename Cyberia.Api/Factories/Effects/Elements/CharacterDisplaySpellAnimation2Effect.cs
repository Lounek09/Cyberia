using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterDisplaySpellAnimation2Effect : Effect
{
    public int GfxId { get; init; }

    private CharacterDisplaySpellAnimation2Effect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int gfxId)
        : base(id, duration, probability, criteria, effectArea)
    {
        GfxId = gfxId;
    }

    internal static CharacterDisplaySpellAnimation2Effect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param3);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, string.Empty, string.Empty, GfxId);
    }
}
