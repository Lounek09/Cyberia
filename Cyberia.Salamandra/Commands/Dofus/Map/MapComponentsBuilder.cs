using Cyberia.Api.Data;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public static class MapComponentsBuilder
    {
        public static DiscordButtonComponent MapButtonBuilder(MapData mapData, bool disable = false)
        {
            return new(ButtonStyle.Success, MapMessageBuilder.GetPacket(mapData.Id), mapData.GetCoordinate(), disable);
        }

        public static DiscordButtonComponent PaginatedMapCoordinateButtonBuilder(MapData mapData, bool disable = false)
        {
            return new(ButtonStyle.Success, PaginatedMapMessageBuilder.GetPacket(MapSearchCategory.Coordinate, $"{mapData.XCoord}{InteractionManager.PACKET_PARAMETER_SEPARATOR}{mapData.YCoord}"), mapData.GetCoordinate(), disable);
        }

        public static DiscordButtonComponent PaginatedMapMapSubAreaButtonBuilder(MapSubAreaData mapSubAreaData, bool disable = false)
        {
            return new(ButtonStyle.Success, PaginatedMapMessageBuilder.GetPacket(MapSearchCategory.MapSubArea, mapSubAreaData.Id.ToString()), mapSubAreaData.Name, disable);
        }

        public static DiscordButtonComponent PaginatedMapMapAreaButtonBuilder(MapAreaData mapAreaData, bool disable = false)
        {
            return new(ButtonStyle.Success, PaginatedMapMessageBuilder.GetPacket(MapSearchCategory.MapArea, mapAreaData.Id.ToString()), mapAreaData.Name, disable);
        }

        public static DiscordSelectComponent MapsSelectBuilder(int uniqueIndex, List<MapData> mapsData, bool disable = false)
        {
            IEnumerable<DiscordSelectComponentOption> options = mapsData.Select(x => new DiscordSelectComponentOption($"{x.GetCoordinate()} ({x.Id}) {(x.IsHouse() ? Emojis.HOUSE : "")}", MapMessageBuilder.GetPacket(x.Id), x.GetMapAreaName().WithMaxLength(50)));

            return new(InteractionManager.SelectComponentPacketBuilder(uniqueIndex), "Sélectionne une map pour l'afficher", options, disable);
        }

        public static DiscordSelectComponent MapSubAreasSelectBuilder(int uniqueIndex, List<MapSubAreaData> mapSubAreasData, bool disable = false)
        {
            IEnumerable<DiscordSelectComponentOption> options = mapSubAreasData.Select(x => new DiscordSelectComponentOption(x.Name.WithMaxLength(100), PaginatedMapMessageBuilder.GetPacket(MapSearchCategory.MapSubArea, x.Id.ToString()), Bot.Instance.Api.Datacenter.MapsData.GetMapAreaNameById(x.MapAreaId)));

            return new(InteractionManager.SelectComponentPacketBuilder(uniqueIndex), "Sélectionne une sous-zone pour afficher ses maps", options, disable);
        }

        public static DiscordSelectComponent MapAreasSelectBuilder(int uniqueIndex, List<MapAreaData> mapAreasData, bool disable = false)
        {
            IEnumerable<DiscordSelectComponentOption> options = mapAreasData.Select(x => new DiscordSelectComponentOption(x.Name.WithMaxLength(100), PaginatedMapMessageBuilder.GetPacket(MapSearchCategory.MapArea, x.Id.ToString()), Bot.Instance.Api.Datacenter.MapsData.GetMapSuperAreaNameById(x.MapSuperAreaId)));

            return new(InteractionManager.SelectComponentPacketBuilder(uniqueIndex), "Sélectionne une zone pour afficher ses maps", options, disable);
        }
    }
}
