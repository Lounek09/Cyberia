using Cyberia.Api.Data.Maps;

using System.Globalization;

namespace Cyberia.Api.Factories.Criteria.Elements.Players;

public sealed record MapAreaCriterion : Criterion
{
    public int MapAreaId { get; init; }

    private MapAreaCriterion(string id, char @operator, int mapAreaId)
        : base(id, @operator)
    {
        MapAreaId = mapAreaId;
    }

    internal static MapAreaCriterion? Create(string id, char @operator, params string[] parameters)
    {
        if (parameters.Length > 0 && int.TryParse(parameters[0], out var mapAreaId))
        {
            return new(id, @operator, mapAreaId);
        }

        return null;
    }

    public MapAreaData? GetMapSubAreaData()
    {
        return DofusApi.Datacenter.MapsRepository.GetMapAreaDataById(MapAreaId);
    }

    protected override string GetDescriptionKey()
    {
        return $"Criterion.MapArea.{GetOperatorDescriptionKey()}";
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        var mapSubAreaName = DofusApi.Datacenter.MapsRepository.GetMapAreaNameById(MapAreaId, culture);

        return GetDescription(culture, mapSubAreaName);
    }
}
