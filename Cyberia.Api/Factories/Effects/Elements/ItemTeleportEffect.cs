using Cyberia.Api.Data.Maps;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record ItemTeleportEffect : Effect, IEffect
{
    public int MapId { get; init; }
    public int Cell { get; init; }

    private ItemTeleportEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int mapId, int cell)
        : base(id, duration, probability, criteria, effectArea)
    {
        MapId = mapId;
        Cell = cell;
    }

    internal static ItemTeleportEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        var binary1 = Convert.ToString(parameters.Param1, 2);
        var binary2 = Convert.ToString(parameters.Param2, 2);
        var binary3 = Convert.ToString(parameters.Param3, 2);

        var mapId = Convert.ToInt32(binary1 + binary2.PadLeft(15, '0') + binary3[^2..], 2);
        var cell = Convert.ToInt32(binary3[..^2], 2);

        return new(effectId, duration, probability, criteria, effectArea, mapId, cell);
    }

    public MapData? GetMapData()
    {
        return DofusApi.Datacenter.MapsData.GetMapDataById(MapId);
    }

    public Description GetDescription()
    {
        var map = GetMapData();
        var mapAreaSubAreaName = map is null ? PatternDecoder.Description(Resources.Unknown_Data, MapId) : map.GetMapAreaName();
        var coordinate = map is null ? "[x, x]" : map.GetCoordinate();

        return GetDescription(mapAreaSubAreaName, coordinate);
    }
}
