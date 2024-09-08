using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Servers.Localized;

internal sealed class ServersLocalizedRepository : DofusLocalizedRepository, IDofusRepository
{
    public static string FileName => ServersRepository.FileName;

    [JsonPropertyName("SR")]
    public IReadOnlyList<ServerLocalizedData> Servers { get; init; }

    [JsonPropertyName("SRP")]
    public IReadOnlyList<ServerPopulationLocalizedData> ServerPopulations { get; init; }

    [JsonPropertyName("SRC")]
    public IReadOnlyList<ServerCommunityLocalizedData> ServerCommunities { get; init; }

    [JsonPropertyName("SRVT")]
    public IReadOnlyList<DefaultServerSpecificTextLocalizedData> DefaultServerSpecificTexts { get; init; }

    [JsonConstructor]
    internal ServersLocalizedRepository()
    {
        Servers = [];
        ServerPopulations = [];
        ServerCommunities = [];
        DefaultServerSpecificTexts = [];
    }
}
