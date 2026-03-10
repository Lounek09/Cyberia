using Cyberia.Api.Data.Maps;

namespace Cyberia.Api.Factories.Effects.Interfaces;

public interface IMapEffect
{
    int MapId { get; }

    MapData? GetMapData();
}
