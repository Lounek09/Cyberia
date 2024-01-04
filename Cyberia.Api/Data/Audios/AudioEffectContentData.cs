using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Audios;

internal sealed class AudioEffectContentData
    : IDofusData<int>
{
    [JsonPropertyName("id")]
    public string Name { get; init; }

    [JsonPropertyName("v")]
    public int Id { get; init; }

    [JsonConstructor]
    internal AudioEffectContentData()
    {
        Name = string.Empty;
    }
}
