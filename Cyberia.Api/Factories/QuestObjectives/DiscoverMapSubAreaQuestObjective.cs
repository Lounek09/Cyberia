using Cyberia.Api.DatacenterNS;

namespace Cyberia.Api.Factories.QuestObjectives
{
    public record class DiscoverMapSubAreaQuestObjective : BasicQuestObjective
    {
        public int MapSubAreaId { get; init; }

        public DiscoverMapSubAreaQuestObjective(QuestObjective questObjective) :
            base(questObjective)
        {
            List<string> parameters = questObjective.Parameters;

            MapSubAreaId = parameters.Count > 0 && int.TryParse(parameters[0], out int mapSubAreaId) ? mapSubAreaId : 0;
        }

        public static new DiscoverMapSubAreaQuestObjective Create(QuestObjective questObjective)
        {
            return new(questObjective);
        }

        public MapSubArea? GetMapSubArea()
        {
            return DofusApi.Instance.Datacenter.MapsData.GetMapSubAreaById(MapSubAreaId);
        }

        public override string GetDescription()
        {
            string mapSubAreaName = DofusApi.Instance.Datacenter.MapsData.GetMapSubAreaNameById(MapSubAreaId);

            return GetDescriptionFromParameters(mapSubAreaName);
        }
    }
}
