using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Spells;

public sealed class SpellIconData : IDofusData
{
    public const int IndexRemastered = 0;
    public const int IndexContrast = 1;
    public const int IndexClassicfAngelic = 2;
    public const int IndexClassicDiabolic = 3;

    [JsonPropertyName("up")]
    public int UpGfxId { get; init; }

    [JsonPropertyName("pc")]
    public IReadOnlyList<int> PrintColors { get; init; }

    [JsonPropertyName("b")]
    public int BackgroundGfxId { get; init; }

    [JsonPropertyName("fc")]
    public IReadOnlyList<int> FrameColors { get; init; }

    [JsonPropertyName("bc")]
    public IReadOnlyList<int> BackgroundColors { get; init; }

    [JsonConstructor]
    internal SpellIconData()
    {
        PrintColors = [];
        FrameColors = [];
        BackgroundColors = [];
    }
}
