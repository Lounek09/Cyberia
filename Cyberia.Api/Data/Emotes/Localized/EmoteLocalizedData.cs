using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Emotes.Localized;

internal sealed class EmoteLocalizedData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public string Name { get; init; }

    [JsonConstructor]
    internal EmoteLocalizedData()
    {
        Name = string.Empty;
    }
}
