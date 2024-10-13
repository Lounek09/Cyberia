using Cyberia.Api.Data.Servers.Localized;
using Cyberia.Api.JsonConverters;
using Cyberia.Langzilla.Enums;

using System.Collections.Frozen;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Servers;

public sealed class ServersRepository : DofusRepository, IDofusRepository
{
    public static string FileName => "servers.json";

    [JsonPropertyName("SR")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, ServerData>))]
    public FrozenDictionary<int, ServerData> Servers { get; init; }

    [JsonPropertyName("SRP")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, ServerPopulationData>))]
    public FrozenDictionary<int, ServerPopulationData> ServerPopulations { get; init; }

    [JsonPropertyName("SRPW")]
    [JsonInclude]
    internal IReadOnlyList<ServerPopulationWeightData> ServerPopulationsWeight { get; init; }

    [JsonPropertyName("SRC")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, ServerCommunityData>))]
    public FrozenDictionary<int, ServerCommunityData> ServerCommunities { get; init; }

    [JsonPropertyName("SRVT")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, DefaultServerSpecificTextData>))]
    public FrozenDictionary<int, DefaultServerSpecificTextData> DefaultServerSpecificTexts { get; init; }

    [JsonPropertyName("SRVC")]
    public IReadOnlyDictionary<string, string> ServerSpecificTexts { get; init; }

    [JsonConstructor]
    internal ServersRepository()
    {
        Servers = FrozenDictionary<int, ServerData>.Empty;
        ServerPopulations = FrozenDictionary<int, ServerPopulationData>.Empty;
        ServerPopulationsWeight = [];
        ServerCommunities = FrozenDictionary<int, ServerCommunityData>.Empty;
        DefaultServerSpecificTexts = FrozenDictionary<int, DefaultServerSpecificTextData>.Empty;
        ServerSpecificTexts = ReadOnlyDictionary<string, string>.Empty;
    }

    public ServerData? GetServerDataById(int id)
    {
        Servers.TryGetValue(id, out var serverData);
        return serverData;
    }

    public string GetServerNameById(int id)
    {
        var serverData = GetServerDataById(id);

        return serverData is null
            ? Translation.Format(ApiTranslations.Unknown_Data, id)
            : serverData.Name;
    }

    public ServerPopulationData? GetServerPopulationDataById(int id)
    {
        ServerPopulations.TryGetValue(id, out var serverPopulationData);
        return serverPopulationData;
    }

    public ServerCommunityData? GetServerCommunityDataById(int id)
    {
        ServerCommunities.TryGetValue(id, out var serverCommunityData);
        return serverCommunityData;
    }

    public DefaultServerSpecificTextData? GetDefaultServerSpecificTextDataById(int id)
    {
        DefaultServerSpecificTexts.TryGetValue(id, out var defaultServerSpecificTextData);
        return defaultServerSpecificTextData;
    }

    protected override void LoadLocalizedData(LangType type, Language language)
    {
        var twoLetterISOLanguageName = language.ToStringFast();
        var localizedRepository = DofusLocalizedRepository.Load<ServersLocalizedRepository>(type, language);

        foreach (var serverLocalizedData in localizedRepository.Servers)
        {
            var serverData = GetServerDataById(serverLocalizedData.Id);
            if (serverData is not null)
            {
                serverData.Name.Add(twoLetterISOLanguageName, serverLocalizedData.Name);
                serverData.Description.Add(twoLetterISOLanguageName, serverLocalizedData.Description);
            }
        }

        foreach (var serverPopulationLocalizedData in localizedRepository.ServerPopulations)
        {
            var serverPopulationData = GetServerPopulationDataById(serverPopulationLocalizedData.Id);
            serverPopulationData?.Name.Add(twoLetterISOLanguageName, serverPopulationLocalizedData.Name);
        }

        foreach (var serverCommunityLocalizedData in localizedRepository.ServerCommunities)
        {
            var serverCommunityData = GetServerCommunityDataById(serverCommunityLocalizedData.Id);
            serverCommunityData?.Name.Add(twoLetterISOLanguageName, serverCommunityLocalizedData.Name);
        }

        foreach (var defaultServerSpecificTextLocalizedData in localizedRepository.DefaultServerSpecificTexts)
        {
            var defaultServerSpecificTextData = GetDefaultServerSpecificTextDataById(defaultServerSpecificTextLocalizedData.Id);
            defaultServerSpecificTextData?.Description.Add(twoLetterISOLanguageName, defaultServerSpecificTextLocalizedData.Description);
        }
    }

    protected override void FinalizeLoading()
    {
        foreach (var serverPopulationWeightData in ServerPopulationsWeight)
        {
            var serverPopulationData = GetServerPopulationDataById(serverPopulationWeightData.Id);
            if (serverPopulationData is not null)
            {
                serverPopulationData.Weight = serverPopulationWeightData.Weight;
            }
        }
    }
}
