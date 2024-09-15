using Cyberia.Api;
using Cyberia.Api.Data.Houses;
using Cyberia.Api.Data.Maps;
using Cyberia.Salamandra.Commands.Dofus.House;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Managers;
using Cyberia.Salamandra.Services;

using DSharpPlus.Entities;

using Microsoft.Extensions.DependencyInjection;

namespace Cyberia.Salamandra.Commands.Dofus.Map;

public sealed class MapMessageBuilder : ICustomMessageBuilder
{
    public const string PacketHeader = "MA";
    public const int PacketVersion = 1;

    private readonly EmbedBuilderService _embedBuilderService;
    private readonly MapData _mapData;
    private readonly MapSubAreaData? _mapSubAreaData;
    private readonly MapAreaData? _mapAreaData;
    private readonly HouseData? _houseData;

    public MapMessageBuilder(EmbedBuilderService embedBuilderService, MapData mapData)
    {
        _embedBuilderService = embedBuilderService;
        _mapData = mapData;
        _mapSubAreaData = _mapData.GetMapSubAreaData();
        _mapAreaData = _mapSubAreaData?.GetMapAreaData();
        _houseData = _mapData.GetHouseData();
    }

    public static MapMessageBuilder? Create(IServiceProvider provider, int version, string[] parameters)
    {
        if (version == PacketVersion &&
            parameters.Length > 0 &&
            int.TryParse(parameters[0], out var mapId))
        {
            var mapData = DofusApi.Datacenter.MapsRepository.GetMapDataById(mapId);
            if (mapData is not null)
            {
                var embedBuilderService = provider.GetRequiredService<EmbedBuilderService>();

                return new(embedBuilderService, mapData);
            }
        }

        return null;
    }

    public static string GetPacket(int mapId)
    {
        return PacketManager.ComponentBuilder(PacketHeader, PacketVersion, mapId);
    }

    public async Task<T> BuildAsync<T>() where T : IDiscordMessageBuilder, new()
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

    private async Task<DiscordEmbedBuilder> EmbedBuilder()
    {
        var embed = _embedBuilderService.CreateEmbedBuilder(EmbedCategory.Map, BotTranslations.Embed_Map_Author)
            .WithTitle($"{_mapData.GetCoordinate()} ({_mapData.Id})")
            .WithDescription(_mapData.GetMapAreaName())
            .WithImageUrl(await _mapData.GetImagePathAsync());

        return embed;
    }

    private IEnumerable<DiscordButtonComponent> ButtonsBuilder()
    {
        var mapsData = DofusApi.Datacenter.MapsRepository.GetMapsDataByCoordinate(_mapData.X, _mapData.Y);
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
