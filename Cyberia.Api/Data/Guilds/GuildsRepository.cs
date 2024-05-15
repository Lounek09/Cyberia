using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Guilds;

public sealed class GuildsRepository : IDofusRepository
{
    private const string c_fileName = "guilds.json";

    [JsonPropertyName("GU.b")]
    public GuildData Guild { get; init; }

    [JsonConstructor]
    internal GuildsRepository()
    {
        Guild = new();
    }

    internal static GuildsRepository Load(string directoryPath)
    {
        var filePath = Path.Join(directoryPath, c_fileName);

        return Datacenter.LoadRepository<GuildsRepository>(filePath);
    }
}
