using Cyberia.Api.DatacenterNS;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public sealed class HouseMessageBuilder : ICustomMessageBuilder
    {
        public const string PACKET_HEADER = "H";
        public const int PACKET_VERSION = 1;

        private readonly House _house;
        private readonly int _selectedMapIndex;
        private readonly Map? _outdoorMap;
        private readonly List<Map> _maps;

        public HouseMessageBuilder(House house, int selectedMapIndex = -1)
        {
            _house = house;
            _outdoorMap = house.GetOutdoorMap();
            _maps = house.GetMaps();
            _selectedMapIndex = selectedMapIndex;
        }

        public static HouseMessageBuilder? Create(int version, string[] parameters)
        {
            if (version == PACKET_VERSION &&
                parameters.Length > 1 &&
                int.TryParse(parameters[0], out int houseId) &&
                int.TryParse(parameters[1], out int selectedMapIndex))
            {
                House? house = Bot.Instance.Api.Datacenter.HousesData.GetHouseById(houseId);
                if (house is not null)
                    return new(house, selectedMapIndex);
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

            if (_outdoorMap != null || _maps.Count > 0)
                message.AddComponents(HouseMapsSelectBuilder());

            return (T)message;
        }

        private Task<DiscordEmbedBuilder> EmbedBuilder()
        {
            DiscordEmbedBuilder embed = EmbedManager.BuildDofusEmbed(DofusEmbedCategory.Houses, "Agence immobilière")
                .WithTitle($"{_house.Name}{(string.IsNullOrEmpty(_house.GetCoordinate()) ? "" : $" {_house.GetCoordinate()}")} ({_house.Id})")
                .WithDescription(string.IsNullOrEmpty(_house.Description) ? "" : Formatter.Italic(_house.Description))
                .AddField("Pièce :", _house.RoomNumber == 0 ? "?" : _house.RoomNumber.ToString(), true)
                .AddField("Coffre :", _house.ChestNumber == 0 ? "?" : _house.ChestNumber.ToString(), true)
                .AddField("Prix :", _house.Price == 0 ? "?" : _house.Price.ToStringThousandSeparator(), true);

            Map? currentMap = _selectedMapIndex == -1 ? _outdoorMap : _maps[_selectedMapIndex];
            if (currentMap is not null)
                embed.WithImageUrl(currentMap.GetImagePath());

            return Task.FromResult(embed);
        }

        private DiscordSelectComponent HouseMapsSelectBuilder()
        {
            List<DiscordSelectComponentOption> options = new();

            if (_outdoorMap is not null)
                options.Add(new("Extérieur", GetPacket(_house.Id), isDefault: _selectedMapIndex == -1));

            for (int i = 0; i < _maps.Count; i++)
                options.Add(new($"Pièce {i + 1}", GetPacket(_house.Id, i), isDefault: i == _selectedMapIndex));

            return new(InteractionManager.SelectComponentPacketBuilder(0), "Sélectionne une pièce pour l'afficher", options);
        }
    }
}
