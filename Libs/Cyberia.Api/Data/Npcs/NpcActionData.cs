using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Npcs;

public sealed class NpcActionData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("v")]
    public LocalizedString Name { get; init; }

    [JsonConstructor]
    internal NpcActionData()
    {
        Name = LocalizedString.Empty;
    }
}

