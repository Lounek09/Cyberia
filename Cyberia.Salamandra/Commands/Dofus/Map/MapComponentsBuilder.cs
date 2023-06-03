using Cyberia.Api.DatacenterNS;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public static class MapComponentsBuilder
    {
        public static DiscordButtonComponent MapButtonBuilder(Map map, bool disable = false)
        {
            return new(ButtonStyle.Success, MapMessageBuilder.GetPacket(map.Id), map.GetCoordinate(), disable);
        }

        public static DiscordButtonComponent PaginatedMapCoordinateButtonBuilder(Map map, bool disable = false)
        {
            return new(ButtonStyle.Success, PaginatedMapMessageBuilder.GetPacket(MapSearchCategory.Coordinate, $"{map.XCoord}{InteractionManager.PACKET_PARAMETER_SEPARATOR}{map.YCoord}"), map.GetCoordinate(), disable);
        }

        public static DiscordButtonComponent PaginatedMapMapSubAreaButtonBuilder(MapSubArea mapSubArea, bool disable = false)
        {
            return new(ButtonStyle.Success, PaginatedMapMessageBuilder.GetPacket(MapSearchCategory.MapSubArea, mapSubArea.Id.ToString()), mapSubArea.Name, disable);
        }

        public static DiscordButtonComponent PaginatedMapMapAreaButtonBuilder(MapArea mapArea, bool disable = false)
        {
            return new(ButtonStyle.Success, PaginatedMapMessageBuilder.GetPacket(MapSearchCategory.MapArea, mapArea.Id.ToString()), mapArea.Name, disable);
        }

        public static DiscordSelectComponent MapsSelectBuilder(int uniqueIndex, List<Map> maps, bool disable = false)
        {
            IEnumerable<DiscordSelectComponentOption> options = maps.Select(x => new DiscordSelectComponentOption($"{x.GetCoordinate()} ({x.Id}) {(x.IsHouse() ? Emojis.HOUSE : "")}", MapMessageBuilder.GetPacket(x.Id), x.GetMapAreaName().WithMaxLength(50)));

            return new(InteractionManager.SelectComponentPacketBuilder(uniqueIndex), "Sélectionne une map pour l'afficher", options, disable);
        }

        public static DiscordSelectComponent MapSubAreasSelectBuilder(int uniqueIndex, List<MapSubArea> mapSubAreas, bool disable = false)
        {
            IEnumerable<DiscordSelectComponentOption> options = mapSubAreas.Select(x => new DiscordSelectComponentOption(x.Name.WithMaxLength(100), PaginatedMapMessageBuilder.GetPacket(MapSearchCategory.MapSubArea, x.Id.ToString()), Bot.Instance.Api.Datacenter.MapsData.GetMapAreaNameById(x.MapAreaId)));
            
            return new(InteractionManager.SelectComponentPacketBuilder(uniqueIndex), "Sélectionne une sous-zone pour afficher ses maps", options, disable);
        }

        public static DiscordSelectComponent MapAreasSelectBuilder(int uniqueIndex, List<MapArea> mapAreas, bool disable = false)
        {
            IEnumerable<DiscordSelectComponentOption> options = mapAreas.Select(x => new DiscordSelectComponentOption(x.Name.WithMaxLength(100), PaginatedMapMessageBuilder.GetPacket(MapSearchCategory.MapArea, x.Id.ToString()), Bot.Instance.Api.Datacenter.MapsData.GetMapSuperAreaNameById(x.MapSuperAreaId)));
            
            return new(InteractionManager.SelectComponentPacketBuilder(uniqueIndex), "Sélectionne une zone pour afficher ses maps", options, disable);
        }
    }
}
