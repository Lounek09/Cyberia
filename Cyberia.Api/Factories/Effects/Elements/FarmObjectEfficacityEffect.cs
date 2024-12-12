using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record FarmObjectEfficacityEffect : Effect
{
    public int Effectiveness { get; init; }

    private FarmObjectEfficacityEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea, int effectiveness)
        : base(id, duration, probability, criteria, dispellable, effectArea)
    {
        Effectiveness = effectiveness;
    }

    internal static FarmObjectEfficacityEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, dispellable, effectArea, (int)parameters.Param3);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, string.Empty, string.Empty, Effectiveness);
    }
}
