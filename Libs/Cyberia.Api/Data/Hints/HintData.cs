using Cyberia.Api.Data.Maps;
using Cyberia.Api.Utils;
using Cyberia.Langzilla.Primitives;

using System.Globalization;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Hints;

public sealed class HintData : IDofusData
{
    [JsonPropertyName("n")]
    public LocalizedString Name { get; init; }

    [JsonPropertyName("c")]
    public int HintCategoryId { get; init; }

    [JsonPropertyName("g")]
    public int GfxId { get; init; }

    [JsonPropertyName("m")]
    public int MapId { get; init; }

    [JsonConstructor]
    internal HintData()
    {
        Name = LocalizedString.Empty;
    }

    public async Task<string> GetIconImagePathAsync(CdnImageSize size)
    {
        return await ImageUrlProvider.GetImagePathAsync("maps/hints", GfxId, size);
    }

    public string GetName(Language language)
    {
        return GetName(language.ToCulture());
    }

    public string GetName(CultureInfo culture)
    {
        var name = Name.ToString(culture);

        if (int.TryParse(name, out var dungeonId))
        {
            // TODO: Parse the dungeon lang and get the name from it
            return name;
        }

        return name;
    }

    public HintCategoryData? GetHintCategoryData()
    {
        return DofusApi.Datacenter.HintsRepository.GetHintCategoryDataById(HintCategoryId);
    }

    public MapData? GetMap()
    {
        return DofusApi.Datacenter.MapsRepository.GetMapDataById(MapId);
    }
}
