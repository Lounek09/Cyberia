using System.Drawing;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Rides;

public sealed class RideData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public LocalizedString Name { get; init; }

    [JsonPropertyName("g")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int GfxId { get; init; }

    [JsonPropertyName("c1")]
    [JsonConverter(typeof(JsonConverters.ColorConverter))]
    public Color? Color1 { get; init; }

    [JsonPropertyName("c2")]
    [JsonConverter(typeof(JsonConverters.ColorConverter))]
    public Color? Color2 { get; init; }

    [JsonPropertyName("c3")]
    [JsonConverter(typeof(JsonConverters.ColorConverter))]
    public Color? Color3 { get; init; }

    [JsonConstructor]
    internal RideData()
    {
        Name = LocalizedString.Empty;
    }
}
