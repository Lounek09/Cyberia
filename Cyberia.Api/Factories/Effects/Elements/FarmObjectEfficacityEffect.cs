using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record FarmObjectEfficacityEffect : Effect
{
    public int Effectiveness { get; init; }

    private FarmObjectEfficacityEffect(int id, int effectiveness)
        : base(id)
    {
        Effectiveness = effectiveness;
    }

    internal static FarmObjectEfficacityEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param3);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, string.Empty, string.Empty, Effectiveness);
    }
}
