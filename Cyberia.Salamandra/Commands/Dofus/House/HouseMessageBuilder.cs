using Cyberia.Api;
using Cyberia.Api.Data;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public sealed class HouseMessageBuilder : ICustomMessageBuilder
    {
        public const string PACKET_HEADER = "H";
        public const int PACKET_VERSION = 1;

        private readonly HouseData _houseData;
        private readonly int _selectedMapIndex;
        private readonly MapData? _outdoorMapData;
        private readonly List<MapData> _mapsData;

        public HouseMessageBuilder(HouseData houseData, int selectedMapIndex = -1)
        {
            _houseData = houseData;
            _outdoorMapData = houseData.GetOutdoorMapData();
            _mapsData = houseData.GetMapsData().ToList();
            _selectedMapIndex = selectedMapIndex;
        }

        public static HouseMessageBuilder? Create(int version, string[] parameters)
        {
            if (version == PACKET_VERSION &&
                parameters.Length > 1 &&
                int.TryParse(parameters[0], out int houseId) &&
                int.TryParse(parameters[1], out int selectedMapIndex))
            {
                HouseData? houseData = DofusApi.Datacenter.HousesData.GetHouseDataById(houseId);
                if (houseData is not null)
                {
                    return new(houseData, selectedMapIndex);
                }
            }

            return null;
        }

        public static string GetPacket(int houseId, int selectedMapIndex = -1)
        {
            return InteractionManager.ComponentPacketBuilder(PACKET_HEADER, PACKET_VERSION, houseId, selectedMapIndex);
        }

        public async Task<T> GetMessageAsync<T>() where T : IDiscordMessageBuilder, new()
        {
            IDiscordMessageBuilder message = new T()
                .AddEmbed(await EmbedBuilder());

            if (_outdoorMapData != null || _mapsData.Count > 0)
            {
                message.AddComponents(HouseMapsSelectBuilder());
            }

            return (T)message;
        }

        private Task<DiscordEmbedBuilder> EmbedBuilder()
        {
            DiscordEmbedBuilder embed = EmbedManager.BuildDofusEmbed(DofusEmbedCategory.Houses, "Agence immobilière")
                .WithTitle($"{_houseData.Name}{(string.IsNullOrEmpty(_houseData.GetCoordinate()) ? "" : $" {_houseData.GetCoordinate()}")} ({_houseData.Id})")
                .WithDescription(string.IsNullOrEmpty(_houseData.Description) ? "" : Formatter.Italic(_houseData.Description))
                .AddField("Pièce :", _houseData.RoomNumber == 0 ? "?" : _houseData.RoomNumber.ToString(), true)
                .AddField("Coffre :", _houseData.ChestNumber == 0 ? "?" : _houseData.ChestNumber.ToString(), true)
                .AddField("Prix :", _houseData.Price == 0 ? "?" : _houseData.Price.ToStringThousandSeparator(), true);

            MapData? currentMapData = _selectedMapIndex == -1 ? _outdoorMapData : _mapsData[_selectedMapIndex];
            if (currentMapData is not null)
            {
                embed.WithImageUrl(currentMapData.GetImagePath());
            }

            return Task.FromResult(embed);
        }

        private DiscordSelectComponent HouseMapsSelectBuilder()
        {
            List<DiscordSelectComponentOption> options = [];

            if (_outdoorMapData is not null)
            {
                options.Add(new("Extérieur", GetPacket(_houseData.Id), isDefault: _selectedMapIndex == -1));
            }

            for (int i = 0; i < _mapsData.Count; i++)
            {
                options.Add(new($"Pièce {i + 1}", GetPacket(_houseData.Id, i), isDefault: i == _selectedMapIndex));
            }

            return new(InteractionManager.SelectComponentPacketBuilder(0), "Sélectionne une pièce pour l'afficher", options);
        }
    }
}
