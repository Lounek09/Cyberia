using Cyberia.Api;
using Cyberia.Api.Data.Maps;
using Cyberia.Salamandra.Formatters;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus.Map;

public static class MapComponentsBuilder
{
    public static DiscordButtonComponent MapButtonBuilder(MapData mapData, bool disable = false)
    {
        return new(DiscordButtonStyle.Success, MapMessageBuilder.GetPacket(mapData.Id), mapData.GetCoordinate(), disable);
    }

    public static DiscordButtonComponent PaginatedMapCoordinateButtonBuilder(MapData mapData, bool disable = false)
    {
        return new(DiscordButtonStyle.Success,
            PaginatedMapMessageBuilder.GetPacket(MapSearchCategory.Coordinate, $"{mapData.X}{PacketFormatter.Separator}{mapData.Y}"),
            mapData.GetCoordinate(),
            disable);
    }

    public static DiscordButtonComponent PaginatedMapMapSubAreaButtonBuilder(MapSubAreaData mapSubAreaData, bool disable = false)
    {
        return new(DiscordButtonStyle.Success,
            PaginatedMapMessageBuilder.GetPacket(MapSearchCategory.MapSubArea,
            mapSubAreaData.Id.ToString()),
            mapSubAreaData.Name,
            disable);
    }

    public static DiscordButtonComponent PaginatedMapMapAreaButtonBuilder(MapAreaData mapAreaData, bool disable = false)
    {
        return new(DiscordButtonStyle.Success, PaginatedMapMessageBuilder.GetPacket(MapSearchCategory.MapArea, mapAreaData.Id.ToString()), mapAreaData.Name, disable);
    }

    public static DiscordSelectComponent MapsSelectBuilder(int uniqueIndex, IEnumerable<MapData> mapsData, bool disable = false)
    {
        var options = mapsData
            .Take(Constant.MaxSelectOption)
            .Select(x =>
            {
                return new DiscordSelectComponentOption(
                    $"{x.GetCoordinate()} ({x.Id})",
                    MapMessageBuilder.GetPacket(x.Id),
                    x.GetMapAreaName().WithMaxLength(50));
            });

        return new(PacketFormatter.Select(uniqueIndex), BotTranslations.Select_Map_Placeholder, options, disable);
    }

    public static DiscordSelectComponent MapSubAreasSelectBuilder(int uniqueIndex, IEnumerable<MapSubAreaData> mapSubAreasData, bool disable = false)
    {
        var options = mapSubAreasData
            .Take(Constant.MaxSelectOption)
            .Select(x =>
            {
                return new DiscordSelectComponentOption(
                    StringExtensions.WithMaxLength(x.Name, 100),
                    PaginatedMapMessageBuilder.GetPacket(MapSearchCategory.MapSubArea, x.Id.ToString()),
                    DofusApi.Datacenter.MapsRepository.GetMapAreaNameById(x.MapAreaId));
            });

        return new(PacketFormatter.Select(uniqueIndex), BotTranslations.Select_MapSubArea_Placeholder, options, disable);
    }

    public static DiscordSelectComponent MapAreasSelectBuilder(int uniqueIndex, IEnumerable<MapAreaData> mapAreasData, bool disable = false)
    {
        var options = mapAreasData
            .Take(Constant.MaxSelectOption)
            .Select(x =>
            {
                return new DiscordSelectComponentOption(
                    StringExtensions.WithMaxLength(x.Name, 100),
                    PaginatedMapMessageBuilder.GetPacket(MapSearchCategory.MapArea, x.Id.ToString()),
                    DofusApi.Datacenter.MapsRepository.GetMapSuperAreaNameById(x.MapSuperAreaId));
            });

        return new(PacketFormatter.Select(uniqueIndex), BotTranslations.Select_MapArea_Placeholder, options, disable);
    }
}
