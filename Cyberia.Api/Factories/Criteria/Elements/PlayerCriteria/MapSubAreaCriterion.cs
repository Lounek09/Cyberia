using Cyberia.Api.Data.Maps;

namespace Cyberia.Api.Factories.Criteria;

public sealed record MapSubAreaCriterion : Criterion
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
        return DofusApi.Datacenter.MapsRepository.GetMapSubAreaDataById(MapSubAreaId);
    }

    protected override string GetDescriptionKey()
    {
        return $"Criterion.MapSubArea.{GetOperatorDescriptionKey()}";
    }

    public override Description GetDescription()
    {
        var mapSubAreaName = DofusApi.Datacenter.MapsRepository.GetMapSubAreaNameById(MapSubAreaId);

        return GetDescription(mapSubAreaName);
    }
}
