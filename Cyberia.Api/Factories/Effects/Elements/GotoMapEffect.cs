using Cyberia.Api.Data.Maps;
using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record GotoMapEffect : Effect
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
