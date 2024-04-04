using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Guilds;

public sealed class GuildsData
    : IDofusData
{
    private const string c_fileName = "guilds.json";

    private static readonly string s_filePath = Path.Join(DofusApi.OutputPath, c_fileName);

    [JsonPropertyName("GU.b")]
    public GuildData Guild { get; init; }

    [JsonConstructor]
    internal GuildsData()
    {
        Guild = new();
    }

    internal static async Task<GuildsData> LoadAsync()
    {
        return await Datacenter.LoadDataAsync<GuildsData>(s_filePath);
    }
}
