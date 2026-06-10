using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Servers.Localized;

internal sealed class ServerPopulationLocalizedData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("v")]
    public string Name { get; init; }

    [JsonConstructor]
    internal ServerPopulationLocalizedData()
    {
        Name = string.Empty;
    }
}
