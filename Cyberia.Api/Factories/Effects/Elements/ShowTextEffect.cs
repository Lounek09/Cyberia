using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record ShowTextEffect : Effect
{
    public string Value { get; init; }

    private ShowTextEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, string value)
        : base(id, duration, probability, criteria, effectArea)
    {
        Value = value;
    }

    internal static ShowTextEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param4);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, string.Empty, string.Empty, string.Empty, Value);
    }
}
