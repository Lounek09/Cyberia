using Cyberia.Api;
using Cyberia.Api.Data.Maps;
using Cyberia.Salamandra.Formatters;

using DSharpPlus.Entities;

using System.Globalization;

namespace Cyberia.Salamandra.Commands.Dofus.Map;

public static class MapComponentsBuilder
{
    public static DiscordButtonComponent MapButtonBuilder(MapData mapData, bool disable = false)
    {
        return new DiscordButtonComponent(
            DiscordButtonStyle.Success,
            MapMessageBuilder.GetPacket(mapData.Id),
            mapData.GetCoordinate(),
            disable);
    }

    public static DiscordButtonComponent PaginatedMapCoordinateButtonBuilder(MapData mapData, bool disable = false)
    {
        return new DiscordButtonComponent(
            DiscordButtonStyle.Success,
            PaginatedMapMessageBuilder.GetPacket(MapSearchCategory.Coordinate, $"{mapData.X}{PacketFormatter.Separator}{mapData.Y}"),
            mapData.GetCoordinate(),
            disable);
    }

    public static DiscordButtonComponent PaginatedMapMapSubAreaButtonBuilder(MapSubAreaData mapSubAreaData, CultureInfo? culture, bool disable = false)
    {
        return new DiscordButtonComponent(
            DiscordButtonStyle.Success,
            PaginatedMapMessageBuilder.GetPacket(MapSearchCategory.MapSubArea,
            mapSubAreaData.Id.ToString()),
            mapSubAreaData.Name.ToString(culture),
            disable);
    }

    public static DiscordButtonComponent PaginatedMapMapAreaButtonBuilder(MapAreaData mapAreaData, CultureInfo? culture, bool disable = false)
    {
        return new DiscordButtonComponent(
            DiscordButtonStyle.Success,
            PaginatedMapMessageBuilder.GetPacket(MapSearchCategory.MapArea, mapAreaData.Id.ToString()),
            mapAreaData.Name.ToString(culture),
            disable);
    }

    public static DiscordSelectComponent MapsSelectBuilder(int uniqueIndex, IEnumerable<MapData> mapsData, CultureInfo? culture, bool disable = false)
    {
        var options = mapsData
            .Take(Constant.MaxSelectOption)
            .Select(x =>
            {
                return new DiscordSelectComponentOption(
                    $"{x.GetCoordinate()} ({x.Id})",
                    MapMessageBuilder.GetPacket(x.Id),
                    x.GetMapAreaName(culture).WithMaxLength(50));
            });

        return new DiscordSelectComponent(
            PacketFormatter.Select(uniqueIndex),
            Translation.Get<BotTranslations>("Select.Map.Placeholder", culture),
            options,
            disable);
    }

    public static DiscordSelectComponent MapSubAreasSelectBuilder(int uniqueIndex, IEnumerable<MapSubAreaData> mapSubAreasData, CultureInfo? culture, bool disable = false)
    {
        var options = mapSubAreasData
            .Take(Constant.MaxSelectOption)
            .Select(x =>
            {
                return new DiscordSelectComponentOption(
                    x.Name.ToString(culture).WithMaxLength(100),
                    PaginatedMapMessageBuilder.GetPacket(MapSearchCategory.MapSubArea, x.Id.ToString()),
                    DofusApi.Datacenter.MapsRepository.GetMapAreaNameById(x.MapAreaId, culture));
            });

        return new DiscordSelectComponent(
            PacketFormatter.Select(uniqueIndex),
            Translation.Get<BotTranslations>("Select.MapSubArea.Placeholder", culture),
            options,
            disable);
    }

    public static DiscordSelectComponent MapAreasSelectBuilder(int uniqueIndex, IEnumerable<MapAreaData> mapAreasData, CultureInfo? culture, bool disable = false)
    {
        var options = mapAreasData
            .Take(Constant.MaxSelectOption)
            .Select(x =>
            {
                return new DiscordSelectComponentOption(
                    x.Name.ToString(culture).WithMaxLength(100),
                    PaginatedMapMessageBuilder.GetPacket(MapSearchCategory.MapArea, x.Id.ToString()),
                    DofusApi.Datacenter.MapsRepository.GetMapSuperAreaNameById(x.MapSuperAreaId));
            });

        return new DiscordSelectComponent(
            PacketFormatter.Select(uniqueIndex),
            Translation.Get<BotTranslations>("Select.MapArea.Placeholder", culture),
            options,
            disable);
    }
}
