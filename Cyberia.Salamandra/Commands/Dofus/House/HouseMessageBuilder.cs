using Cyberia.Api;
using Cyberia.Api.Data.Houses;
using Cyberia.Api.Data.Maps;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Formatters;
using Cyberia.Salamandra.Services;

using DSharpPlus;
using DSharpPlus.Entities;

using Microsoft.Extensions.DependencyInjection;

using System.Globalization;

namespace Cyberia.Salamandra.Commands.Dofus.House;

public sealed class HouseMessageBuilder : ICustomMessageBuilder
{
    public const string PacketHeader = "H";
    public const int PacketVersion = 1;

    private readonly EmbedBuilderService _embedBuilderService;
    private readonly HouseData _houseData;
    private readonly int _selectedMapIndex;
    private readonly MapData? _outdoorMapData;
    private readonly List<MapData> _mapsData;
    private readonly CultureInfo? _culture;

    public HouseMessageBuilder(EmbedBuilderService embedBuilderService, HouseData houseData, CultureInfo? culture, int selectedMapIndex = -1)
    {
        _embedBuilderService = embedBuilderService;
        _houseData = houseData;
        _outdoorMapData = houseData.GetOutdoorMapData();
        _mapsData = houseData.GetMapsData().ToList();
        _selectedMapIndex = selectedMapIndex;
        _culture = culture;
    }

    public static HouseMessageBuilder? Create(IServiceProvider provider, int version, CultureInfo? culture, string[] parameters)
    {
        if (version == PacketVersion &&
            parameters.Length > 1 &&
            int.TryParse(parameters[0], out var houseId) &&
            int.TryParse(parameters[1], out var selectedMapIndex))
        {
            var houseData = DofusApi.Datacenter.HousesRepository.GetHouseDataById(houseId);
            if (houseData is not null)
            {
                var embedBuilderService = provider.GetRequiredService<EmbedBuilderService>();

                return new(embedBuilderService, houseData, culture, selectedMapIndex);
            }
        }

        return null;
    }

    public static string GetPacket(int houseId, int selectedMapIndex = -1)
    {
        return PacketFormatter.Action(PacketHeader, PacketVersion, houseId, selectedMapIndex);
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
        var embed = _embedBuilderService.CreateEmbedBuilder(EmbedCategory.Houses, Translation.Get<BotTranslations>("Embed.House.Author", _culture))
            .WithTitle($"{_houseData.Name.ToString(_culture)} {_houseData.GetCoordinate()} ({_houseData.Id})")
            .AddField(
                Translation.Get<BotTranslations>("Embed.Field.Room.Title", _culture),
                _houseData.RoomNumber == 0 ? "?" : _houseData.RoomNumber.ToString(),
                true)
            .AddField(
                Translation.Get<BotTranslations>("Embed.Field.Chest.Title", _culture),
                _houseData.ChestNumber == 0 ? "?" : _houseData.ChestNumber.ToString(),
                true)
            .AddField(
                Translation.Get<BotTranslations>("Embed.Field.Price.Title", _culture),
                _houseData.Price == 0 ? "?" : _houseData.Price.ToFormattedString(_culture),
                true);

        var description = _houseData.Description.ToString(_culture);
        if (!string.IsNullOrEmpty(description))
        {
            embed.WithDescription(Formatter.Italic(description));
        }

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
                yield return new DiscordSelectComponentOption(
                    Translation.Get<BotTranslations>("Select.HouseMap.Option.Outdoors", _culture),
                    GetPacket(_houseData.Id),
                    isDefault: _selectedMapIndex == -1);
            }

            for (var i = 0; i < _mapsData.Count; i++)
            {
                yield return new DiscordSelectComponentOption(
                    $"{Translation.Get<BotTranslations>("Select.HouseMap.Option.Room", _culture)} {i + 1}",
                    GetPacket(_houseData.Id, i),
                    isDefault: i == _selectedMapIndex);
            }
        }

        return new DiscordSelectComponent(
            PacketFormatter.Select(0),
            Translation.Get<BotTranslations>("Select.HouseMap.Placeholder", _culture),
            OptionsGenerator());
    }
}
