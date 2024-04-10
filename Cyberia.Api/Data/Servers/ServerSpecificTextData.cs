using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Servers;

public sealed class ServerSpecificTextData : IDofusData<string>
{
    [JsonPropertyName("id")]
    public string Id { get; init; }

    [JsonPropertyName("v")]
    public string Description { get; init; }

    [JsonConstructor]
    internal ServerSpecificTextData()
    {
        Id = string.Empty;
        Description = string.Empty;
    }
}
