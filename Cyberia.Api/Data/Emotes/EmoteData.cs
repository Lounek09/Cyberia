using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Emotes;

public sealed class EmoteData
    : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public string Name { get; init; }

    [JsonPropertyName("s")]
    public string Shortcut { get; init; }

    [JsonConstructor]
    internal EmoteData()
    {
        Name = string.Empty;
        Shortcut = string.Empty;
    }
}
