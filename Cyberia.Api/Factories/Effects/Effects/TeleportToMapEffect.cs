﻿using Cyberia.Api.DatacenterNS;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed record TeleportToMapEffect : Effect, IEffect<TeleportToMapEffect>
    {
        public int MapId { get; init; }
        public int Cell { get; init; }

        private TeleportToMapEffect(int effectId, int duration, int probability, List<ICriteriaElement> criteria, EffectArea effectArea, int mapId, int cell) :
            base(effectId, duration, probability, criteria, effectArea)
        {
            MapId = mapId;
            Cell = cell;
        }

        public static TeleportToMapEffect Create(int effectId, EffectParameters parameters, int duration, int probability, List<ICriteriaElement> criteria, EffectArea effectArea)
        {
            return new(effectId, duration, probability, criteria, effectArea, parameters.Param2, parameters.Param3);
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