using Cyberia.Api.DatacenterNS;

namespace Cyberia.Api.Factories.Criteria.PlayerCriteria
{
    public sealed record MapCriterion : Criterion, ICriterion<MapCriterion>
    {
        public int MapId { get; init; }

        private MapCriterion(string id, char @operator, int mapId) :
            base(id, @operator)
        {
            MapId = mapId;
        }

        public static MapCriterion? Create(string id, char @operator, params string[] parameters)
        {
            if (parameters.Length > 0 && int.TryParse(parameters[0], out int mapId))
            {
                return new(id, @operator, mapId);
            }

            return null;
        }

        public MapData? GetMapData()
        {
            return DofusApi.Instance.Datacenter.MapsData.GetMapDataById(MapId);
        }

        protected override string GetDescriptionName()
        {
            return $"Criterion.Map.{GetOperatorDescriptionName()}";
        }

        public Description GetDescription()
        {
            MapData? map = GetMapData();
            string mapAreaSubAreaName = map is null ? PatternDecoder.Description(Resources.Unknown_Data, MapId) : map.GetMapAreaName();
            string coordinate = map is null ? "[x, x]" : map.GetCoordinate();

            return GetDescription(coordinate, mapAreaSubAreaName);
        }
    }
}
