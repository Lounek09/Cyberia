using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Guilds;

public sealed class GuildsData
    : IDofusData
{
    private const string FILE_NAME = "guilds.json";
    private static readonly string FILE_PATH = Path.Join(DofusApi.OUTPUT_PATH, FILE_NAME);

    [JsonPropertyName("GU.b")]
    public GuildData Guild { get; init; }

    [JsonConstructor]
    internal GuildsData()
    {
        Guild = new();
    }

    internal static async Task<GuildsData> LoadAsync()
    {
        return await Datacenter.LoadDataAsync<GuildsData>(FILE_PATH);
    }
}
