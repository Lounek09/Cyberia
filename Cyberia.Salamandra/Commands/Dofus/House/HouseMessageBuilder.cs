using Cyberia.Api;
using Cyberia.Api.Data.Houses;
using Cyberia.Api.Data.Maps;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus.House;

public sealed class HouseMessageBuilder : ICustomMessageBuilder
{
    public const string PacketHeader = "H";
    public const int PacketVersion = 1;

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

    public static HouseMessageBuilder? Create(IServiceProvider _, int version, string[] parameters)
    {
        if (version == PacketVersion &&
            parameters.Length > 1 &&
            int.TryParse(parameters[0], out var houseId) &&
            int.TryParse(parameters[1], out var selectedMapIndex))
        {
            var houseData = DofusApi.Datacenter.HousesRepository.GetHouseDataById(houseId);
            if (houseData is not null)
            {
                return new(houseData, selectedMapIndex);
            }
        }

        return null;
    }

    public static string GetPacket(int houseId, int selectedMapIndex = -1)
    {
        return PacketManager.ComponentBuilder(PacketHeader, PacketVersion, houseId, selectedMapIndex);
    }

    public async Task<T> BuildAsync<T>() where T : IDiscordMessageBuilder, new()
    {
        var message = new T()
            .AddEmbed(await EmbedBuilder());

        if (_outdoorMapData is not null || _mapsData.Count > 0)
        {
            message.AddComponents(HouseMapsSelectBuilder());
        }

        return (T)message;
    }

    private async Task<DiscordEmbedBuilder> EmbedBuilder()
    {
        var embed = EmbedManager.CreateEmbedBuilder(EmbedCategory.Houses, BotTranslations.Embed_House_Author)
            .WithTitle($"{_houseData.Name} {_houseData.GetCoordinate()} ({_houseData.Id})")
            .WithDescription(string.IsNullOrEmpty(_houseData.Description) ? string.Empty : Formatter.Italic(_houseData.Description))
            .AddField(BotTranslations.Embed_Field_Room_Title, _houseData.RoomNumber == 0 ? "?" : _houseData.RoomNumber.ToString(), true)
            .AddField(BotTranslations.Embed_Field_Chest_Title, _houseData.ChestNumber == 0 ? "?" : _houseData.ChestNumber.ToString(), true)
            .AddField(BotTranslations.Embed_Field_Price_Title, _houseData.Price == 0 ? "?" : _houseData.Price.ToFormattedString(), true);

        var currentMapData = _selectedMapIndex == -1 ? _outdoorMapData : _mapsData[_selectedMapIndex];
        if (currentMapData is not null)
        {
            embed.WithImageUrl(await currentMapData.GetImagePathAsync());
        }

        return embed;
    }

    private DiscordSelectComponent HouseMapsSelectBuilder()
    {
        IEnumerable<DiscordSelectComponentOption> OptionsGenerator()
        {
            if (_outdoorMapData is not null)
            {
                yield return new DiscordSelectComponentOption(BotTranslations.Select_HouseMap_Option_Outdoors, GetPacket(_houseData.Id), isDefault: _selectedMapIndex == -1);
            }

            for (var i = 0; i < _mapsData.Count; i++)
            {
                yield return new DiscordSelectComponentOption($"{BotTranslations.Select_HouseMap_Option_Room} {i + 1}", GetPacket(_houseData.Id, i), isDefault: i == _selectedMapIndex);
            }
        }

        return new DiscordSelectComponent(PacketManager.SelectComponentBuilder(0), BotTranslations.Select_HouseMap_Placeholder, OptionsGenerator());
    }
}
