using Cyberia.Api.JsonConverters;

using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Spells;

public sealed class SpellIconData : IDofusData
{
    public const int INDEX_REMASTERED = 0;
    public const int INDEX_CONTRAST = 1;
    public const int INDEX_CLASSIC_ANGELIC = 2;
    public const int INDEX_CLASSIC_DIABOLIC = 3;

    [JsonPropertyName("up")]
    public int UpGfxId { get; init; }

    [JsonPropertyName("pc")]
    [JsonConverter(typeof(ReadOnlyCollectionConverter<int>))]
    public ReadOnlyCollection<int> PrintColors { get; init; }

    [JsonPropertyName("b")]
    public int BackgroundGfxId { get; init; }

    [JsonPropertyName("fc")]
    [JsonConverter(typeof(ReadOnlyCollectionConverter<int>))]
    public ReadOnlyCollection<int> FrameColors { get; init; }

    [JsonPropertyName("bc")]
    [JsonConverter(typeof(ReadOnlyCollectionConverter<int>))]
    public ReadOnlyCollection<int> BackgroundColors { get; init; }

    [JsonConstructor]
    internal SpellIconData()
    {
        PrintColors = ReadOnlyCollection<int>.Empty;
        FrameColors = ReadOnlyCollection<int>.Empty;
        BackgroundColors = ReadOnlyCollection<int>.Empty;
    }
}
