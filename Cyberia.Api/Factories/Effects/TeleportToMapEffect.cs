using Cyberia.Api.DatacenterNS;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed class TeleportToMapEffect : BasicEffect
    {
        public int MapId { get; init; }
        public int Cell { get; init; }

        public TeleportToMapEffect(int effectId, EffectParameters parameters, int duration, int probability, Area area) : 
            base(effectId, parameters, duration, probability, area)
        {
            MapId = parameters.Param2;
            Cell = parameters.Param3;
        }

        public static new TeleportToMapEffect Create(int effectId, EffectParameters parameters, int duration, int probability, Area area)
        {
            return new(effectId, parameters, duration, probability, area);
        }

        public Map? GetMap()
        {
            return DofusApi.Instance.Datacenter.MapsData.GetMapById(MapId);
        }

        public override string GetDescription()
        {
            Map? map = GetMap();
            string mapAreaSubAreaName = map is null ? "Inconnu" : map.GetMapAreaName();
            string coordinate = map is null ? "[x, x]" : map.GetCoordinate();

            return GetDescriptionFromParameters(mapAreaSubAreaName, coordinate);
        }
    }
}
