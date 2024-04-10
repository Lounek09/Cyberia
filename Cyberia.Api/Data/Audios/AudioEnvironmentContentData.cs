using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Audios;

internal sealed class AudioEnvironmentContentData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public string Name { get; init; }

    [JsonPropertyName("v")]
    public int Id { get; init; }

    [JsonConstructor]
    internal AudioEnvironmentContentData()
    {
        Name = string.Empty;
    }
}
