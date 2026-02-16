using Cyberia.Api.Data.SpeakingItems.Localized;
using Cyberia.Api.Enums;
using Cyberia.Api.JsonConverters;
using Cyberia.Langzilla.Primitives;

using System.Collections.Frozen;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.SpeakingItems;

public sealed class SpeakingItemsRepository : DofusRepository, IDofusRepository
{
    public static string FileName => "speakingitems.json";

    [JsonPropertyName("SIM")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, SpeakingItemsMessageData>))]
    public FrozenDictionary<int, SpeakingItemsMessageData> SpeakingItemsMessages { get; init; }

    [JsonPropertyName("SIT")]
    public IReadOnlyList<SpeakingItemsTriggerData> SpeakingItemsTriggers { get; init; }


    [JsonConstructor]
    internal SpeakingItemsRepository()
    {
        SpeakingItemsMessages = FrozenDictionary<int, SpeakingItemsMessageData>.Empty;
        SpeakingItemsTriggers = ReadOnlyCollection<SpeakingItemsTriggerData>.Empty;
    }

    public SpeakingItemsMessageData? GetSpeakingItemsMessageData(int id)
    {
        SpeakingItemsMessages.TryGetValue(id, out var speakingItemData);
        return speakingItemData;
    }

    public SpeakingItemsTriggerData? GetSpeakingItemsTriggerData(int triggerId, Corpulence corpulence)
    {
        return SpeakingItemsTriggers.FirstOrDefault(x => x.TriggerId == triggerId && x.Corpulence == corpulence);
    }

    protected override void LoadLocalizedData(LangsIdentifier identifier)
    {
        var twoLetterISOLanguageName = identifier.Language.ToStringFast();
        var localizedRepository = DofusLocalizedRepository.Load<SpeakingItemsLocalizedRepository>(identifier);

        foreach (var speakingItemMessageLocalizedData in localizedRepository.SpeakingItemsMessages)
        {
            var speakingItemMessageData = GetSpeakingItemsMessageData(speakingItemMessageLocalizedData.Id);
            speakingItemMessageData?.Message.TryAdd(twoLetterISOLanguageName, speakingItemMessageLocalizedData.Message);
        }
    }
}
