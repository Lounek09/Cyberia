using Cyberia.Api.Data.Maps;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Factories.Effects.Interfaces;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record ItemTeleportEffect : Effect, IMapEffect
{
    public int MapId { get; init; }
    public int Cell { get; init; }

    private ItemTeleportEffect(int id, int mapId, int cell)
        : base(id)
    {
        MapId = mapId;
        Cell = cell;
    }

    internal static ItemTeleportEffect Create(int effectId, EffectParameters parameters)
    {
        var binary1 = Convert.ToString(parameters.Param1, 2);
        var binary2 = Convert.ToString(parameters.Param2, 2);
        var binary3 = Convert.ToString(parameters.Param3, 2);

        var mapId = Convert.ToInt32(binary1 + binary2.PadLeft(15, '0') + binary3[^2..], 2);
        var cell = Convert.ToInt32(binary3[..^2], 2);

        return new(effectId, mapId, cell);
    }

    public MapData? GetMapData()
    {
        return DofusApi.Datacenter.MapsRepository.GetMapDataById(MapId);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        var map = GetMapData();
        if (map is null)
        {
            return GetDescription(culture, Translation.UnknownData(MapId, culture), 666, 666);
        }

        return GetDescription(culture, map.GetFullName(culture), map.X, map.Y);
    }
}
