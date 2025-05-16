using Cyberia.Api.JsonConverters;

using System.Drawing;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Spells;

public sealed class SpellIconData : IDofusData
{
    public const int IndexRemastered = 0;
    public const int IndexContrast = 1;
    public const int IndexClassicAngelic = 2;
    public const int IndexClassicDiabolic = 3;

    [JsonPropertyName("up")]
    public int UpGfxId { get; init; }

    [JsonPropertyName("pc")]
    [JsonConverter(typeof(ColorReadOnlyListConverter))]
    public IReadOnlyList<Color?> PrintColors { get; init; }

    [JsonPropertyName("b")]
    public int BackgroundGfxId { get; init; }

    [JsonPropertyName("fc")]
    [JsonConverter(typeof(ColorReadOnlyListConverter))]
    public IReadOnlyList<Color?> FrameColors { get; init; }

    [JsonPropertyName("bc")]
    [JsonConverter(typeof(ColorReadOnlyListConverter))]
    public IReadOnlyList<Color?> BackgroundColors { get; init; }

    [JsonConstructor]
    internal SpellIconData()
    {
        PrintColors = [];
        FrameColors = [];
        BackgroundColors = [];
    }
}
