using Cyberia.Api.Data.Maps;

using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Hints;

public sealed class HintData
    : IDofusData
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

    public HintCategoryData? GetHintCategory()
    {
        return DofusApi.Datacenter.HintsData.GetHintCategory(HintCategoryId);
    }

    public MapData? GetMap()
    {
        return DofusApi.Datacenter.MapsData.GetMapDataById(MapId);
    }
}
