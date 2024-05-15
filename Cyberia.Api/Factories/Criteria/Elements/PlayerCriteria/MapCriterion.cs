using Cyberia.Api.Data.Maps;

namespace Cyberia.Api.Factories.Criteria;

public sealed record MapCriterion : Criterion, ICriterion
{
    public int MapId { get; init; }

    private MapCriterion(string id, char @operator, int mapId)
        : base(id, @operator)
    {
        MapId = mapId;
    }

    internal static MapCriterion? Create(string id, char @operator, params string[] parameters)
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

    protected override string GetDescriptionName()
    {
        return $"Criterion.Map.{GetOperatorDescriptionName()}";
    }

    public Description GetDescription()
    {
        var map = GetMapData();
        var mapAreaSubAreaName = map is null ? PatternDecoder.Description(Resources.Unknown_Data, MapId) : map.GetMapAreaName();
        var coordinate = map is null ? "[x, x]" : map.GetCoordinate();

        return GetDescription(coordinate, mapAreaSubAreaName);
    }
}
