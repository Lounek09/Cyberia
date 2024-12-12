using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record RideDetailsEffect : Effect
{
    public int ItemUuid { get; init; }
    public DateTime ExpirationDate { get; init; }

    private RideDetailsEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea, int itemUuid, DateTime expirationDate)
        : base(id, duration, probability, criteria, dispellable, effectArea)
    {
        ItemUuid = itemUuid;
        ExpirationDate = expirationDate;
    }

    internal static RideDetailsEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, dispellable, effectArea, (int)parameters.Param1, DateTimeOffset.FromUnixTimeMilliseconds(parameters.Param2).UtcDateTime);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, ItemUuid, ExpirationDate.ToString(culture?.DateTimeFormat ?? CultureInfo.CurrentCulture.DateTimeFormat));
    }
}
