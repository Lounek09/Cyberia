using Cyberia.Api.Data.Maps;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record GotoMapEffect
    : Effect, IEffect
{
    public int MapId { get; init; }
    public int Cell { get; init; }

    private GotoMapEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int mapId, int cell)
        : base(id, duration, probability, criteria, effectArea)
    {
        MapId = mapId;
        Cell = cell;
    }

    internal static GotoMapEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param2, (int)parameters.Param3);
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
