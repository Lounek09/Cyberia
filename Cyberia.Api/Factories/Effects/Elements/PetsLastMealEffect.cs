using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Managers;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record PetsLastMealEffect : Effect
{
    public DateTime DateTime { get; init; }

    private PetsLastMealEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, DateTime dateTime)
        : base(id, duration, probability, criteria, effectArea)
    {
        DateTime = dateTime;
    }

    internal static PetsLastMealEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, DateTimeManager.CreateDateTimeFromEffectParameters(parameters));
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, DateTime.ToString(culture?.DateTimeFormat ?? CultureInfo.CurrentCulture.DateTimeFormat));
    }
}
