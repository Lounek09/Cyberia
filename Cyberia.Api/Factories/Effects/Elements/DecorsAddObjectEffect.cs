using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record DecorsAddObjectEffect : Effect
{
    public int GfxId { get; init; }

    private DecorsAddObjectEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int gfxId)
        : base(id, duration, probability, criteria, effectArea)
    {
        GfxId = gfxId;
    }

    internal static DecorsAddObjectEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        // Param3 is a supposition
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param3);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, string.Empty, string.Empty, GfxId);
    }
}
