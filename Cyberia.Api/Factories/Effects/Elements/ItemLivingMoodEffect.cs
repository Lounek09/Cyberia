using Cyberia.Api.Enums;
using Cyberia.Api.Extensions;
using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record ItemLivingMoodEffect : Effect
{
    public Corpulence Corpulence { get; init; }

    private ItemLivingMoodEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, Corpulence corpulence)
        : base(id, duration, probability, criteria, effectArea)
    {
        Corpulence = corpulence;
    }

    internal static ItemLivingMoodEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (Corpulence)parameters.Param3);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, string.Empty, string.Empty, Corpulence.GetDescription());
    }
}
