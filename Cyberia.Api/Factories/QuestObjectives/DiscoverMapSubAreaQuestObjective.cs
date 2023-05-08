using Cyberia.Api.DatacenterNS;

namespace Cyberia.Api.Factories.QuestObjectives
{
    public sealed class DiscoverMapSubAreaQuestObjective : BasicQuestObjective
    {
        public int MapSubAreaId { get; init; }

        public DiscoverMapSubAreaQuestObjective(QuestObjective questObjective, int mapSubAreaId) :
            base(questObjective)
        {
            MapSubAreaId = mapSubAreaId;
        }

        public static new DiscoverMapSubAreaQuestObjective? Create(QuestObjective questObjective)
        {
            if (questObjective.Parameters.Count > 0 && 
                int.TryParse(questObjective.Parameters[0], out int mapSubAreaId))
                return new(questObjective, mapSubAreaId);

            return null;
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
