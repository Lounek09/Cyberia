using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record RideDetailsEffect : Effect
{
    public int ItemUuid { get; init; }
    public DateTime ExpirationDate { get; init; }

    private RideDetailsEffect(int id, int itemUuid, DateTime expirationDate)
        : base(id)
    {
        ItemUuid = itemUuid;
        ExpirationDate = expirationDate;
    }

    internal static RideDetailsEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param1, DateTimeOffset.FromUnixTimeMilliseconds(parameters.Param2).UtcDateTime);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, ItemUuid, ExpirationDate.ToString(culture?.DateTimeFormat ?? CultureInfo.CurrentCulture.DateTimeFormat));
    }
}
