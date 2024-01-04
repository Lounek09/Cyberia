using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Guilds;

public sealed class GuildsData
    : IDofusData
{
    private const string FILE_NAME = "guilds.json";

    [JsonPropertyName("GU.b")]
    public GuildData Guild { get; init; }

    [JsonConstructor]
    internal GuildsData()
    {
        Guild = new();
    }

    internal static GuildsData Load()
    {
        return Datacenter.LoadDataFromFile<GuildsData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
    }
}
