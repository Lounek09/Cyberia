using Cyberia.Api.DatacenterNS;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed class TeleportToMap2Effect : BasicEffect
    {
        public int MapId { get; init; }
        public int Cell { get; init; }

        public TeleportToMap2Effect(int effectId, EffectParameters parameters, int duration, int probability, Area area) : base(effectId, parameters, duration, probability, area)
        {
            string binary1 = Convert.ToString(parameters.Param1, 2);
            string binary2 = Convert.ToString(parameters.Param2, 2);
            string binary3 = Convert.ToString(parameters.Param3, 2);

            MapId = Convert.ToInt32(binary1 + binary2.PadLeft(15, '0') + binary3[^2..], 2);
            Cell = Convert.ToInt32(binary3[..^2], 2);
        }

        public static new TeleportToMap2Effect Create(int effectId, EffectParameters parameters, int duration, int probability, Area area)
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
