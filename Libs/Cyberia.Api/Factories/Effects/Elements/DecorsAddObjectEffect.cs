using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record DecorsAddObjectEffect : Effect
{
    public int GfxId { get; init; }

    private DecorsAddObjectEffect(int id, int gfxId)
        : base(id)
    {
        GfxId = gfxId;
    }

    internal static DecorsAddObjectEffect Create(int effectId, EffectParameters parameters)
    {
        // Param3 is a supposition
        return new(effectId, (int)parameters.Param3);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, string.Empty, string.Empty, GfxId);
    }
}
