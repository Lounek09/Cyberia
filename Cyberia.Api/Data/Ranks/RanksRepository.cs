using Cyberia.Api.Data.Ranks.Localized;
using Cyberia.Api.JsonConverters;
using Cyberia.Langzilla.Enums;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Ranks;

public sealed class RanksRepository : DofusRepository, IDofusRepository
{
    public static string FileName => "ranks.json";

    [JsonPropertyName("R")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, GuildRankData>))]
    public FrozenDictionary<int, GuildRankData> GuildRanks { get; init; }

    [JsonConstructor]
    internal RanksRepository()
    {
        GuildRanks = FrozenDictionary<int, GuildRankData>.Empty;
    }

    public GuildRankData? GetGuildRankDataById(int id)
    {
        GuildRanks.TryGetValue(id, out var rank);
        return rank;
    }

    protected override void LoadLocalizedData(LangType type, LangLanguage language)
    {
        var twoLetterISOLanguageName = language.ToCultureInfo().TwoLetterISOLanguageName;
        var localizedRepository = DofusLocalizedRepository.Load<RanksLocalizedRepository>(type, language);

        foreach (var guildRankLocalizedData in localizedRepository.GuildRanks)
        {
            var guildRankData = GetGuildRankDataById(guildRankLocalizedData.Id);
            guildRankData?.Name.Add(twoLetterISOLanguageName, guildRankLocalizedData.Name);
        }
    }
}
