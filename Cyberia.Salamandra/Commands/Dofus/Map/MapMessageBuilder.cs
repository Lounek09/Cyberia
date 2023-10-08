using Cyberia.Api.DatacenterNS;
using Cyberia.Salamandra.Managers;

using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public sealed class MapMessageBuilder : ICustomMessageBuilder
    {
        public const string PACKET_HEADER = "MA";
        public const int PACKET_VERSION = 1;

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
            if (version == PACKET_VERSION &&
                parameters.Length > 0 &&
                int.TryParse(parameters[0], out int mapId))
            {
                MapData? mapData = Bot.Instance.Api.Datacenter.MapsData.GetMapDataById(mapId);
                if (mapData is not null)
                    return new(mapData);
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
                .WithTitle($"{_mapData.GetCoordinate()} ({_mapData.Id})")
                .WithDescription(_mapData.GetMapAreaName())
                .WithImageUrl(_mapData.GetImagePath());

            return Task.FromResult(embed);
        }

        private List<DiscordButtonComponent> ButtonsBuilder()
        {
            List<DiscordButtonComponent> components = new();

            List<MapData> mapsData = Bot.Instance.Api.Datacenter.MapsData.GetMapsDataByCoordinate(_mapData.XCoord, _mapData.YCoord);
            if (mapsData.Count > 1)
                components.Add(MapComponentsBuilder.PaginatedMapCoordinateButtonBuilder(_mapData));

            if (_mapSubAreaData is not null)
                components.Add(MapComponentsBuilder.PaginatedMapMapSubAreaButtonBuilder(_mapSubAreaData));

            if (_mapAreaData is not null)
                components.Add(MapComponentsBuilder.PaginatedMapMapAreaButtonBuilder(_mapAreaData));

            if (_houseData is not null)
                components.Add(HouseComponentsBuilder.HouseButtonBuilder(_houseData));

            return components;
        }
    }
}
