using Cyberia.Api.Data;
using Cyberia.Api.Data.Houses;
using Cyberia.Api.Data.Maps;
using Cyberia.Salamandra.Commands.Dofus.House;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Formatters;
using Cyberia.Salamandra.Services;

using DSharpPlus.Entities;

using Microsoft.Extensions.DependencyInjection;

using System.Globalization;

namespace Cyberia.Salamandra.Commands.Dofus.Map;

public sealed class MapMessageBuilder : ICustomMessageBuilder
{
    public const string PacketHeader = "MA";
    public const int PacketVersion = 1;

    private readonly IEmbedBuilderService _embedBuilderService;
    private readonly MapData _mapData;
    private readonly MapSubAreaData? _mapSubAreaData;
    private readonly MapAreaData? _mapAreaData;
    private readonly HouseData? _houseData;
    private readonly CultureInfo? _culture;

    public MapMessageBuilder(IEmbedBuilderService embedBuilderService, MapData mapData, CultureInfo? culture)
    {
        _embedBuilderService = embedBuilderService;
        _mapData = mapData;
        _mapSubAreaData = _mapData.GetMapSubAreaData();
        _mapAreaData = _mapSubAreaData?.GetMapAreaData();
        _houseData = _mapData.GetHouseData();
        _culture = culture;
    }

    public static MapMessageBuilder? Create(IServiceProvider provider, int version, CultureInfo? culture, params ReadOnlySpan<string> parameters)
    {
        if (version == PacketVersion &&
            parameters.Length > 0 &&
            int.TryParse(parameters[0], out var mapId))
        {
            var dofusDatacenter = provider.GetRequiredService<DofusDatacenter>();

            var mapData = dofusDatacenter.MapsRepository.GetMapDataById(mapId);
            if (mapData is not null)
            {
                var embedBuilderService = provider.GetRequiredService<IEmbedBuilderService>();

                return new(embedBuilderService, mapData, culture);
            }
        }

        return null;
    }

    public static string GetPacket(int mapId)
    {
        return PacketFormatter.Action(PacketHeader, PacketVersion, mapId);
    }

    public async Task<T> BuildAsync<T>() where T : IDiscordMessageBuilder, new()
    {
        var message = new T()
            .AddEmbed(await EmbedBuilder());

        var components = ButtonsBuilder();
        if (components.Any())
        {
            message.AddActionRowComponent(components);
        }

        return (T)message;
    }

    private async Task<DiscordEmbedBuilder> EmbedBuilder()
    {
        var embed = _embedBuilderService.CreateBaseEmbedBuilder(EmbedCategory.Map, Translation.Get<BotTranslations>("Embed.Map.Author", _culture), _culture)
            .WithTitle($"{_mapData.GetCoordinate()} ({_mapData.Id})")
            .WithDescription(_mapData.GetFullName(_culture))
            .WithImageUrl(await _mapData.GetImagePathAsync());

        return embed;
    }

    private IEnumerable<DiscordButtonComponent> ButtonsBuilder()
    {
        var mapsData = _mapData.GetMapsDataAtSameCoordinate();
        if (mapsData.Skip(1).Any())
        {
            yield return MapComponentsBuilder.PaginatedMapCoordinateButtonBuilder(_mapData);
        }

        if (_mapSubAreaData is not null)
        {
            yield return MapComponentsBuilder.PaginatedMapMapSubAreaButtonBuilder(_mapSubAreaData, _culture);
        }

        if (_mapAreaData is not null)
        {
            yield return MapComponentsBuilder.PaginatedMapMapAreaButtonBuilder(_mapAreaData, _culture);
        }

        if (_houseData is not null)
        {
            yield return HouseComponentsBuilder.HouseButtonBuilder(_houseData, _culture);
        }
    }
}
