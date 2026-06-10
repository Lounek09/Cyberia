using Cyberia.Api.Data.Maps;

using System.Globalization;

namespace Cyberia.Api.Factories.Criteria.Elements.Maps;

public sealed record MapPlayerCriterion : Criterion
{
    public int MapId { get; init; }
    public int MaxPlayers { get; init; }

    private MapPlayerCriterion(string id, char @operator, int mapId, int maxPlayers)
        : base(id, @operator)
    {
        MapId = mapId;
        MaxPlayers = maxPlayers;
    }

    internal static MapPlayerCriterion? Create(string id, char @operator, params ReadOnlySpan<string> parameters)
    {
        if (parameters.Length > 1 && int.TryParse(parameters[0], out var mapId) && int.TryParse(parameters[1], out var maxPlayers))
        {
            return new(id, @operator, mapId, maxPlayers);
        }

        return null;
    }

    public MapData? GetMapData()
    {
        return DofusApi.Datacenter.MapsRepository.GetMapDataById(MapId);
    }

    protected override string GetDescriptionKey()
    {
        return $"Criterion.MapPlayer.{GetOperatorDescriptionKey()}";
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        var map = GetMapData();
        if (map is null)
        {
            return GetDescription(culture, Translation.UnknownData(MapId, culture), 666, 666, MaxPlayers);
        }

        return GetDescription(culture, map.GetFullName(culture), map.X, map.Y, MaxPlayers);
    }
}
