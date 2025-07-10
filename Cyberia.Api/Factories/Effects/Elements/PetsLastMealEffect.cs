using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Utils;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record PetsLastMealEffect : Effect
{
    public DateTime LastFedAt { get; init; }

    private PetsLastMealEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea, DateTime lastFedAt)
        : base(id, duration, probability, criteria, dispellable, effectArea)
    {
        LastFedAt = lastFedAt;
    }

    internal static PetsLastMealEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, dispellable, effectArea, GameDateFormatter.CreateDateTimeFromEffectParameters(parameters));
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, LastFedAt.ToShortRolePlayString(culture));
    }
}
