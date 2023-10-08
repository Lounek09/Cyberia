using Cyberia.Api.DatacenterNS;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed record TeleportToMap2Effect : Effect, IEffect<TeleportToMap2Effect>
    {
        public int MapId { get; init; }
        public int Cell { get; init; }

        private TeleportToMap2Effect(int effectId, int duration, int probability, List<ICriteriaElement> criteria, EffectArea effectArea, int mapId, int cell) :
            base(effectId, duration, probability, criteria, effectArea)
        {
            MapId = mapId;
            Cell = cell;
        }

        public static TeleportToMap2Effect Create(int effectId, EffectParameters parameters, int duration, int probability, List<ICriteriaElement> criteria, EffectArea effectArea)
        {
            string binary1 = Convert.ToString(parameters.Param1, 2);
            string binary2 = Convert.ToString(parameters.Param2, 2);
            string binary3 = Convert.ToString(parameters.Param3, 2);

            int mapId = Convert.ToInt32(binary1 + binary2.PadLeft(15, '0') + binary3[^2..], 2);
            int cell = Convert.ToInt32(binary3[..^2], 2);

            return new(effectId, duration, probability, criteria, effectArea, mapId, cell);
        }

        public MapData? GetMapData()
        {
            return DofusApi.Instance.Datacenter.MapsData.GetMapDataById(MapId);
        }

        public Description GetDescription()
        {
            MapData? map = GetMapData();
            string mapAreaSubAreaName = map is null ? PatternDecoder.Description(Resources.Unknown_Data, MapId) : map.GetMapAreaName();
            string coordinate = map is null ? "[x, x]" : map.GetCoordinate();

            return GetDescription(mapAreaSubAreaName, coordinate);
        }
    }
}
