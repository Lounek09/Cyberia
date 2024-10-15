using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CreatedSinceEffect : Effect
{
    public int Days { get; init; }

    private CreatedSinceEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int days)
        : base(id, duration, probability, criteria, effectArea)
    {
        Days = days;
    }

    internal static CreatedSinceEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param3);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, string.Empty, string.Empty, Days);
    }
}
