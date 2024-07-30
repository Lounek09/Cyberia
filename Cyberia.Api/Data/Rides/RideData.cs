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
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int Color1 { get; init; }

    [JsonPropertyName("c2")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int Color2 { get; init; }

    [JsonPropertyName("c3")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int Color3 { get; init; }

    [JsonConstructor]
    internal RideData()
    {
        Name = LocalizedString.Empty;
    }
}
