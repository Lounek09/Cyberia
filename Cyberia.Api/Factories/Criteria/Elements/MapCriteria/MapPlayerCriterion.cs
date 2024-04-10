using Cyberia.Api.Data.Maps;

namespace Cyberia.Api.Factories.Criteria;

public sealed record MapPlayerCriterion : Criterion, ICriterion
{
    public int MapId { get; init; }
    public int MaxPlayers { get; init; }

    private MapPlayerCriterion(string id, char @operator, int mapId, int maxPlayers)
        : base(id, @operator)
    {
        MapId = mapId;
        MaxPlayers = maxPlayers;
    }

    internal static MapPlayerCriterion? Create(string id, char @operator, params string[] parameters)
    {
        if (parameters.Length > 1 && int.TryParse(parameters[0], out var mapId) && int.TryParse(parameters[1], out var maxPlayers))
        {
            return new(id, @operator, mapId, maxPlayers);
        }

        return null;
    }

    public MapData? GetMapData()
    {
        return DofusApi.Datacenter.MapsData.GetMapDataById(MapId);
    }

    protected override string GetDescriptionName()
    {
        return $"Criterion.MapPlayer.{GetOperatorDescriptionName()}";
    }

    public Description GetDescription()
    {
        var map = GetMapData();
        var mapAreaSubAreaName = map is null ? PatternDecoder.Description(Resources.Unknown_Data, MapId) : map.GetMapAreaName();
        var coordinate = map is null ? "[x, x]" : map.GetCoordinate();

        return GetDescription(coordinate, mapAreaSubAreaName, MaxPlayers);
    }
}
