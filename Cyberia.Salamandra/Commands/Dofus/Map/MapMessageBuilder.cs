using Cyberia.Api.DatacenterNS;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public sealed class MapMessageBuilder : ICustomMessageBuilder
    {
        public const string PACKET_HEADER = "MA";
        public const int PACKET_VERSION = 1;

        private readonly Map _map;
        private readonly MapSubArea? _mapSubArea;
        private readonly MapArea? _mapArea;
        private readonly House? _house;

        public MapMessageBuilder(Map map)
        {
            _map = map;
            _mapSubArea = _map.GetMapSubArea();
            _mapArea = _mapSubArea?.GetMapArea();
            _house = _map.GetHouse();
        }

        public static MapMessageBuilder? Create(int version, string[] parameters)
        {
            if (version == PACKET_VERSION &&
                parameters.Length > 0 &&
                int.TryParse(parameters[0], out int mapId))
            {
                Map? map = Bot.Instance.Api.Datacenter.MapsData.GetMapById(mapId);
                if (map is not null)
                    return new(map);
            }

            return null;
        }

        public static string GetPacket(int mapId)
        {
            return InteractionManager.ComponentPacketBuilder(PACKET_HEADER, PACKET_VERSION, mapId);
        }

        public async Task<T> GetMessageAsync<T>() where T : IDiscordMessageBuilder, new()
        {
            IDiscordMessageBuilder message = new T()
                .AddEmbed(await EmbedBuilder());

            List<DiscordButtonComponent> components = ButtonsBuilder();
            if (components.Count > 0)
                message.AddComponents(components);

            return (T)message;
        }

        private Task<DiscordEmbedBuilder> EmbedBuilder()
        {
            DiscordEmbedBuilder embed = EmbedManager.BuildDofusEmbed(DofusEmbedCategory.Map, "Carte du monde")
                .WithTitle($"{_map.GetCoordinate()} ({_map.Id})")
                .WithDescription(_map.GetMapAreaName())
                .WithImageUrl(_map.GetImagePath());

            return Task.FromResult(embed);
        }

        private List<DiscordButtonComponent> ButtonsBuilder()
        {
            List<DiscordButtonComponent> components = new();

            List<Map> maps = Bot.Instance.Api.Datacenter.MapsData.GetMapsByCoordinate(_map.XCoord, _map.YCoord);
            if (maps.Count > 1)
                components.Add(MapComponentsBuilder.PaginatedMapCoordinateButtonBuilder(_map));

            if (_mapSubArea is not null)
                components.Add(MapComponentsBuilder.PaginatedMapMapSubAreaButtonBuilder(_mapSubArea));

            if (_mapArea is not null)
                components.Add(MapComponentsBuilder.PaginatedMapMapAreaButtonBuilder(_mapArea));

            if (_house is not null)
                components.Add(HouseComponentsBuilder.HouseButtonBuilder(_house));

            return components;
        }
    }
}
