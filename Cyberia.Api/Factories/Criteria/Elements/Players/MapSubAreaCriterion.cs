﻿using Cyberia.Api.Data.Maps;

using System.Globalization;

namespace Cyberia.Api.Factories.Criteria.Elements.Players;

public sealed record MapSubAreaCriterion : Criterion
{
    public int MapSubAreaId { get; init; }

    private MapSubAreaCriterion(string id, char @operator, int mapSubAreaId)
        : base(id, @operator)
    {
        MapSubAreaId = mapSubAreaId;
    }

    internal static MapSubAreaCriterion? Create(string id, char @operator, params ReadOnlySpan<string> parameters)
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

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        var mapSubAreaName = DofusApi.Datacenter.MapsRepository.GetMapSubAreaNameById(MapSubAreaId, culture);

        return GetDescription(culture, mapSubAreaName);
    }
}
