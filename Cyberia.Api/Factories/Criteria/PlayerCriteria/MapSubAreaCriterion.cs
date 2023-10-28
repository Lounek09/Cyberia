using Cyberia.Api.Data;

namespace Cyberia.Api.Factories.Criteria.PlayerCriteria
{
    public sealed record MapSubAreaCriterion : Criterion, ICriterion<MapSubAreaCriterion>
    {
        public int MapSubAreaId { get; init; }

        private MapSubAreaCriterion(string id, char @operator, int mapSubAreaId) :
            base(id, @operator)
        {
            MapSubAreaId = mapSubAreaId;
        }

        public static MapSubAreaCriterion? Create(string id, char @operator, params string[] parameters)
        {
            if (parameters.Length > 0 && int.TryParse(parameters[0], out int mapSubAreaId))
            {
                return new(id, @operator, mapSubAreaId);
            }

            return null;
        }

        public MapSubAreaData? GetMapSubAreaData()
        {
            return DofusApi.Instance.Datacenter.MapsData.GetMapSubAreaDataById(MapSubAreaId);
        }

        protected override string GetDescriptionName()
        {
            return $"Criterion.MapSubArea.{GetOperatorDescriptionName()}";
        }

        public Description GetDescription()
        {
            string mapSubAreaName = DofusApi.Instance.Datacenter.MapsData.GetMapSubAreaNameById(MapSubAreaId);

            return GetDescription(mapSubAreaName);
        }
    }
}
