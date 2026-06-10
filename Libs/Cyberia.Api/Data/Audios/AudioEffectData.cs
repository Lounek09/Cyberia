using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Audios;

public sealed class AudioEffectData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("f")]
    public string FileName { get; init; }

    [JsonPropertyName("v")]
    public int BaseVolume { get; init; }

    [JsonPropertyName("l")]
    public bool Loop { get; init; }

    [JsonPropertyName("s")]
    public bool Streaming { get; init; }

    [JsonPropertyName("o")]
    public int Offset { get; init; }

    [JsonIgnore]
    public string Name { get; internal set; }

    [JsonConstructor]
    internal AudioEffectData()
    {
        FileName = string.Empty;
        Name = string.Empty;
    }
}
