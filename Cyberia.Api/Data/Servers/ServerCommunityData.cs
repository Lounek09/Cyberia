using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Servers;

public sealed class ServerCommunityData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public LocalizedString Name { get; init; }

    [JsonPropertyName("d")]
    public bool Visible { get; init; }

    [JsonPropertyName("i")]
    [JsonInclude]
    internal int Id2 { get; init; }

    [JsonPropertyName("c")]
    public IReadOnlyList<string> Countries { get; init; }

    [JsonConstructor]
    internal ServerCommunityData()
    {
        Name = LocalizedString.Empty;
        Countries = [];
    }
}
