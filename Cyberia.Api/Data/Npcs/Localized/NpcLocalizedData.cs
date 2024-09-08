using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Npcs.Localized;

internal sealed class NpcLocalizedData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public string Name { get; init; }

    [JsonConstructor]
    internal NpcLocalizedData()
    {
        Name = string.Empty;
    }
}
