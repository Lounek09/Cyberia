﻿using Cyberia.Api.Data.Maps;
using Cyberia.Api.Data.Quests;

using System.Globalization;

namespace Cyberia.Api.Factories.QuestObjectives.Elements;

public sealed record DiscoverMapSubAreaQuestObjective : QuestObjective
{
    public int MapSubAreaId { get; init; }

    private DiscoverMapSubAreaQuestObjective(QuestObjectiveData questObjectiveData, int mapSubAreaId)
        : base(questObjectiveData)
    {
        MapSubAreaId = mapSubAreaId;
    }

    internal static DiscoverMapSubAreaQuestObjective? Create(QuestObjectiveData questObjectiveData)
    {
        var parameters = questObjectiveData.Parameters;
        if (parameters.Count > 0 && int.TryParse(parameters[0], out var mapSubAreaId))
        {
            return new(questObjectiveData, mapSubAreaId);
        }

        return null;
    }

    public MapSubAreaData? GetMapSubAreaData()
    {
        return DofusApi.Datacenter.MapsRepository.GetMapSubAreaDataById(MapSubAreaId);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        var mapSubAreaName = DofusApi.Datacenter.MapsRepository.GetMapSubAreaNameById(MapSubAreaId, culture);

        return GetDescription(culture, mapSubAreaName);
    }
}
