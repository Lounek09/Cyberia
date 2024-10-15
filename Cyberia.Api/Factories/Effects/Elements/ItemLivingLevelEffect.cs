using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record ItemLivingLevelEffect : Effect
{
    public int Experience { get; init; }

    private ItemLivingLevelEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int experience)
        : base(id, duration, probability, criteria, effectArea)
    {
        Experience = experience;
    }

    internal static ItemLivingLevelEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param3);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, string.Empty, string.Empty, Experience);
    }
}
