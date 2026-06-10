using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Guilds;

public sealed class GuildsRepository : DofusRepository, IDofusRepository
{
    public static string FileName => "guilds.json";

    [JsonPropertyName("GU.b")]
    public GuildData Guild { get; init; }

    [JsonConstructor]
    internal GuildsRepository()
    {
        Guild = new();
    }
}
