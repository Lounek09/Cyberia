using Cyberia.Api.Data.Maps;
using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record ItemTeleportEffect : Effect
{
    public int MapId { get; init; }
    public int Cell { get; init; }

    private ItemTeleportEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea, int mapId, int cell)
        : base(id, duration, probability, criteria, dispellable, effectArea)
    {
        MapId = mapId;
        Cell = cell;
    }

    internal static ItemTeleportEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea)
    {
        var binary1 = Convert.ToString(parameters.Param1, 2);
        var binary2 = Convert.ToString(parameters.Param2, 2);
        var binary3 = Convert.ToString(parameters.Param3, 2);

        var mapId = Convert.ToInt32(binary1 + binary2.PadLeft(15, '0') + binary3[^2..], 2);
        var cell = Convert.ToInt32(binary3[..^2], 2);

        return new(effectId, duration, probability, criteria, dispellable, effectArea, mapId, cell);
    }

    public MapData? GetMapData()
    {
        return DofusApi.Datacenter.MapsRepository.GetMapDataById(MapId);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        var map = GetMapData();
        var mapAreaSubAreaName = map is null ? Translation.UnknownData(MapId, culture) : map.GetMapAreaName(culture);
        var coordinate = map is null ? "[x, x]" : map.GetCoordinate();

        return GetDescription(culture, mapAreaSubAreaName, coordinate);
    }
}
