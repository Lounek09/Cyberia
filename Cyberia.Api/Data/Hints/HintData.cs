using Cyberia.Api.Data.Maps;
using Cyberia.Api.Managers;

using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Hints;

public sealed class HintData : IDofusData
{
    [JsonPropertyName("n")]
    public string Name { get; init; }

    [JsonPropertyName("c")]
    public int HintCategoryId { get; init; }

    [JsonPropertyName("g")]
    public int GfxId { get; init; }

    [JsonPropertyName("m")]
    public int MapId { get; init; }

    [JsonConstructor]
    internal HintData()
    {
        Name = string.Empty;
    }

    public async Task<string> GetIconImagePathAsync(CdnImageSize size)
    {
        return await CdnManager.GetImagePathAsync("maps/hints", GfxId, size);
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
