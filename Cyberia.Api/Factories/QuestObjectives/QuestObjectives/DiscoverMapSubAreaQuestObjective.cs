using Cyberia.Api.DatacenterNS;

namespace Cyberia.Api.Factories.QuestObjectives
{
    public record class DiscoverMapSubAreaQuestObjective : QuestObjective, IQuestObjective<DiscoverMapSubAreaQuestObjective>
    {
        public int MapSubAreaId { get; init; }

        private DiscoverMapSubAreaQuestObjective(QuestObjectiveData questObjectiveData, int mapSubAreaId) :
            base(questObjectiveData)
        {
            MapSubAreaId = mapSubAreaId;
        }

        public static DiscoverMapSubAreaQuestObjective? Create(QuestObjectiveData questObjectiveData)
        {
            List<string> parameters = questObjectiveData.Parameters;
            if (parameters.Count > 0 && int.TryParse(parameters[0], out int mapSubAreaId))
            {
                return new(questObjectiveData, mapSubAreaId);
            }

            return null;
        }

        public MapSubAreaData? GetMapSubAreaData()
        {
            return DofusApi.Instance.Datacenter.MapsData.GetMapSubAreaDataById(MapSubAreaId);
        }

        public Description GetDescription()
        {
            string mapSubAreaName = DofusApi.Instance.Datacenter.MapsData.GetMapSubAreaNameById(MapSubAreaId);

            return GetDescription(mapSubAreaName);
        }
    }
}
