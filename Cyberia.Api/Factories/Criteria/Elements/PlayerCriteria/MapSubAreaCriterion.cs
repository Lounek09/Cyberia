using Cyberia.Api.Data.Maps;

namespace Cyberia.Api.Factories.Criteria;

public sealed record MapSubAreaCriterion
    : Criterion, ICriterion
{
    public int MapSubAreaId { get; init; }

    private MapSubAreaCriterion(string id, char @operator, int mapSubAreaId)
        : base(id, @operator)
    {
        MapSubAreaId = mapSubAreaId;
    }

    internal static MapSubAreaCriterion? Create(string id, char @operator, params string[] parameters)
    {
        if (parameters.Length > 0 && int.TryParse(parameters[0], out var mapSubAreaId))
        {
            return new(id, @operator, mapSubAreaId);
        }

        return null;
    }

    public MapSubAreaData? GetMapSubAreaData()
    {
        return DofusApi.Datacenter.MapsData.GetMapSubAreaDataById(MapSubAreaId);
    }

    protected override string GetDescriptionName()
    {
        return $"Criterion.MapSubArea.{GetOperatorDescriptionName()}";
    }

    public Description GetDescription()
    {
        var mapSubAreaName = DofusApi.Datacenter.MapsData.GetMapSubAreaNameById(MapSubAreaId);

        return GetDescription(mapSubAreaName);
    }
}
