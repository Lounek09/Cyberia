using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Ranks.Localized;

internal sealed class RanksLocalizedRepository : DofusLocalizedRepository, IDofusRepository
{
    public static string FileName => RanksRepository.FileName;

    [JsonPropertyName("R")]
    public IReadOnlyList<GuildRankLocalizedData> GuildRanks { get; init; }

    [JsonConstructor]
    internal RanksLocalizedRepository()
    {
        GuildRanks = ReadOnlyCollection<GuildRankLocalizedData>.Empty;
    }
}
