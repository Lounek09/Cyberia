using Cyberia.Api.Data.TTG.Localized;
using Cyberia.Api.JsonConverters;
using Cyberia.Langzilla.Enums;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.TTG;

public sealed class TTGRepository : DofusRepository, IDofusRepository
{
    public static string FileName => "ttg.json";

    [JsonPropertyName("TTG.c")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, TTGCardData>))]
    public FrozenDictionary<int, TTGCardData> TTGCards { get; set; }

    [JsonPropertyName("TTG.e")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, TTGEntityData>))]
    public FrozenDictionary<int, TTGEntityData> TTGEntities { get; set; }

    [JsonPropertyName("TTG.f")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, TTGFamilyData>))]
    public FrozenDictionary<int, TTGFamilyData> TTGFamilies { get; set; }

    [JsonConstructor]
    internal TTGRepository()
    {
        TTGCards = FrozenDictionary<int, TTGCardData>.Empty;
        TTGEntities = FrozenDictionary<int, TTGEntityData>.Empty;
        TTGFamilies = FrozenDictionary<int, TTGFamilyData>.Empty;
    }

    public TTGCardData? GetTTGCardDataById(int id)
    {
        TTGCards.TryGetValue(id, out var ttgCardData);
        return ttgCardData;
    }

    public TTGEntityData? GetTTGEntityDataById(int id)
    {
        TTGEntities.TryGetValue(id, out var ttgEntityData);
        return ttgEntityData;
    }

    public string GetTTGEntityNameById(int id)
    {
        var ttgEntityData = GetTTGEntityDataById(id);

        return ttgEntityData is null
            ? Translation.Format(ApiTranslations.Unknown_Data, id)
            : ttgEntityData.Name;
    }

    public TTGFamilyData? GetTTGFamilyDataById(int id)
    {
        TTGFamilies.TryGetValue(id, out var ttgFamilyData);
        return ttgFamilyData;
    }

    public string GetTTGFamilyNameById(int id)
    {
        var ttgFamilyData = GetTTGFamilyDataById(id);

        return ttgFamilyData is null
            ? Translation.Format(ApiTranslations.Unknown_Data, id)
            : ttgFamilyData.Name;
    }

    protected override void LoadLocalizedData(LangType type, LangLanguage language)
    {
        var twoLetterISOLanguageName = language.ToStringFast();
        var localizedRepository = DofusLocalizedRepository.Load<TTGLocalizedRepository>(type, language);

        foreach (var ttgEntityLocalizedData in localizedRepository.TTGEntities)
        {
            var ttgEntityData = GetTTGEntityDataById(ttgEntityLocalizedData.Id);
            ttgEntityData?.Name.Add(twoLetterISOLanguageName, ttgEntityLocalizedData.Name);
        }

        foreach (var ttgFamilyLocalizedData in localizedRepository.TTGFamilies)
        {
            var ttgFamilyData = GetTTGFamilyDataById(ttgFamilyLocalizedData.Id);
            ttgFamilyData?.Name.Add(twoLetterISOLanguageName, ttgFamilyLocalizedData.Name);
        }
    }
}
