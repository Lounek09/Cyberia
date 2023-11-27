using Cyberia.Api.Data;

namespace Cyberia.Api.Factories.QuestObjectives;

public record class DiscoverMapSubAreaQuestObjective : QuestObjective, IQuestObjective<DiscoverMapSubAreaQuestObjective>
{
    public int MapSubAreaId { get; init; }

    private DiscoverMapSubAreaQuestObjective(QuestObjectiveData questObjectiveData, int mapSubAreaId)
        : base(questObjectiveData)
    {
        MapSubAreaId = mapSubAreaId;
    }

    public static DiscoverMapSubAreaQuestObjective? Create(QuestObjectiveData questObjectiveData)
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
        return DofusApi.Datacenter.MapsData.GetMapSubAreaDataById(MapSubAreaId);
    }

    public Description GetDescription()
    {
        var mapSubAreaName = DofusApi.Datacenter.MapsData.GetMapSubAreaNameById(MapSubAreaId);

        return GetDescription(mapSubAreaName);
    }
}
