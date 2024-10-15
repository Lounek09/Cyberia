using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record UntranslatedEffect : Effect
{
    public EffectParameters Parameters { get; init; }

    internal UntranslatedEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, EffectParameters parameters)
        : base(id, duration, probability, criteria, effectArea)
    {
        Parameters = parameters;
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, Parameters.Param1, Parameters.Param2, Parameters.Param3, Parameters.Param4);
    }
}
