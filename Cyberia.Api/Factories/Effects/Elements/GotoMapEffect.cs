using Cyberia.Api.Data.Maps;
using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record GotoMapEffect : Effect
{
    public int MapId { get; init; }
    public int Cell { get; init; }

    private GotoMapEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea, int mapId, int cell)
        : base(id, duration, probability, criteria, dispellable, effectArea)
    {
        MapId = mapId;
        Cell = cell;
    }

    internal static GotoMapEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, dispellable, effectArea, (int)parameters.Param2, (int)parameters.Param3);
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
