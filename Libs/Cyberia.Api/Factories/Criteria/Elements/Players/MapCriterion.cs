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
        if (map is null)
        {
            return GetDescription(culture, Translation.UnknownData(MapId, culture), 666, 666);
        }

        return GetDescription(culture, map.GetFullName(culture), map.X, map.Y);
    }
}
