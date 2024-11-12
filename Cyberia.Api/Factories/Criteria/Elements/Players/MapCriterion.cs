using Cyberia.Api.Data.Maps;

using System.Globalization;

namespace Cyberia.Api.Factories.Criteria.Elements.Players;

public sealed record MapCriterion : Criterion
{
    public int MapId { get; init; }

    private MapCriterion(string id, char @operator, int mapId)
        : base(id, @operator)
    {
        MapId = mapId;
    }

    internal static MapCriterion? Create(string id, char @operator, params ReadOnlySpan<string> parameters)
    {
        if (parameters.Length > 0 && int.TryParse(parameters[0], out var mapId))
        {
            return new(id, @operator, mapId);
        }

        return null;
    }

    public MapData? GetMapData()
    {
        return DofusApi.Datacenter.MapsRepository.GetMapDataById(MapId);
    }

    protected override string GetDescriptionKey()
    {
        return $"Criterion.Map.{GetOperatorDescriptionKey()}";
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        var map = GetMapData();
        var mapAreaSubAreaName = map is null ? Translation.UnknownData(MapId, culture) : map.GetMapAreaName(culture);
        var coordinate = map is null ? "[x, x]" : map.GetCoordinate();

        return GetDescription(culture, coordinate, mapAreaSubAreaName);
    }
}
