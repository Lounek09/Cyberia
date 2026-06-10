using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Npcs.Localized;

internal sealed class NpcActionLocalizedData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("v")]
    public string Name { get; init; }

    [JsonConstructor]
    internal NpcActionLocalizedData()
    {
        Name = string.Empty;
    }
}

