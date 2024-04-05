using Cyberia.Api;
using Cyberia.Api.Data.Houses;
using Cyberia.Api.Data.Maps;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Managers;

using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus;

public sealed class MapMessageBuilder : ICustomMessageBuilder
{
    public const string PacketHeader = "MA";
    public const int PacketVersion = 1;

    private readonly MapData _mapData;
    private readonly MapSubAreaData? _mapSubAreaData;
    private readonly MapAreaData? _mapAreaData;
    private readonly HouseData? _houseData;

    public MapMessageBuilder(MapData mapData)
    {
        _mapData = mapData;
        _mapSubAreaData = _mapData.GetMapSubAreaData();
        _mapAreaData = _mapSubAreaData?.GetMapAreaData();
        _houseData = _mapData.GetHouseData();
    }

    public static MapMessageBuilder? Create(int version, string[] parameters)
    {
        if (version == PacketVersion &&
            parameters.Length > 0 &&
            int.TryParse(parameters[0], out var mapId))
        {
            var mapData = DofusApi.Datacenter.MapsData.GetMapDataById(mapId);
            if (mapData is not null)
            {
                return new(mapData);
            }
        }

        return null;
    }

    public static string GetPacket(int mapId)
    {
        return InteractionManager.ComponentPacketBuilder(PacketHeader, PacketVersion, mapId);
    }

    public async Task<T> GetMessageAsync<T>() where T : IDiscordMessageBuilder, new()
    {
        var message = new T()
            .AddEmbed(await EmbedBuilder());

        var components = ButtonsBuilder();
        if (components.Any())
        {
            message.AddComponents(components);
        }

        return (T)message;
    }

    private Task<DiscordEmbedBuilder> EmbedBuilder()
    {
        var embed = EmbedManager.CreateEmbedBuilder(EmbedCategory.Map, "Carte du monde")
            .WithTitle($"{_mapData.GetCoordinate()} ({_mapData.Id})")
            .WithDescription(_mapData.GetMapAreaName())
            .WithImageUrl(_mapData.GetImagePath());

        return Task.FromResult(embed);
    }

    private IEnumerable<DiscordButtonComponent> ButtonsBuilder()
    {
        var mapsData = DofusApi.Datacenter.MapsData.GetMapsDataByCoordinate(_mapData.XCoord, _mapData.YCoord);
        if (mapsData.Skip(1).Any())
        {
            yield return MapComponentsBuilder.PaginatedMapCoordinateButtonBuilder(_mapData);
        }

        if (_mapSubAreaData is not null)
        {
            yield return MapComponentsBuilder.PaginatedMapMapSubAreaButtonBuilder(_mapSubAreaData);
        }

        if (_mapAreaData is not null)
        {
            yield return MapComponentsBuilder.PaginatedMapMapAreaButtonBuilder(_mapAreaData);
        }

        if (_houseData is not null)
        {
            yield return HouseComponentsBuilder.HouseButtonBuilder(_houseData);
        }
    }
}
