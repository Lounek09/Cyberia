using Cyberia.Api.Data.Maps;

namespace Cyberia.Api.Factories.Criteria;

public sealed record MapAreaCriterion : Criterion, ICriterion
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
        return DofusApi.Datacenter.MapsData.GetMapAreaDataById(MapAreaId);
    }

    protected override string GetDescriptionName()
    {
        return $"Criterion.MapArea.{GetOperatorDescriptionName()}";
    }

    public Description GetDescription()
    {
        var mapSubAreaName = DofusApi.Datacenter.MapsData.GetMapAreaNameById(MapAreaId);

        return GetDescription(mapSubAreaName);
    }
}
