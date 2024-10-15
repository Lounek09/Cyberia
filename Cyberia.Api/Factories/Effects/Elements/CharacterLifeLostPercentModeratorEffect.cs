using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterLifeLostPercentReductorEffect : Effect
{
    public int PercentReductor { get; init; }

    private CharacterLifeLostPercentReductorEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int percentModerator)
        : base(id, duration, probability, criteria, effectArea)
    {
        PercentReductor = percentModerator;
    }

    internal static CharacterLifeLostPercentReductorEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param1);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, PercentReductor);
    }
}
