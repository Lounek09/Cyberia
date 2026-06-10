using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Servers.Localized;

internal sealed class ServerCommunityLocalizedData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public string Name { get; init; }

    [JsonConstructor]
    internal ServerCommunityLocalizedData()
    {
        Name = string.Empty;
    }
}
