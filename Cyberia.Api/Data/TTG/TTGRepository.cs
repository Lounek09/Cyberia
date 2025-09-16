using Cyberia.Api.Data.TTG.Localized;
using Cyberia.Api.JsonConverters;
using Cyberia.Langzilla.Enums;

using System.Collections.Frozen;
using System.Globalization;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.TTG;

public sealed class TTGRepository : DofusRepository, IDofusRepository
{
    public static string FileName => "ttg.json";

    [JsonPropertyName("TTG.c")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, TTGCardData>))]
    public FrozenDictionary<int, TTGCardData> TTGCards { get; init; }

    [JsonPropertyName("TTG.e")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, TTGEntityData>))]
    public FrozenDictionary<int, TTGEntityData> TTGEntities { get; init; }

    [JsonPropertyName("TTG.f")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, TTGFamilyData>))]
    public FrozenDictionary<int, TTGFamilyData> TTGFamilies { get; init; }

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

    public string GetTTGEntityNameById(int id, Language language)
    {
        return GetTTGEntityNameById(id, language.ToCulture());
    }

    public string GetTTGEntityNameById(int id, CultureInfo? culture = null)
    {
        var ttgEntityData = GetTTGEntityDataById(id);

        return ttgEntityData is null
            ? Translation.UnknownData(id, culture)
            : ttgEntityData.Name.ToString(culture);
    }

    public TTGFamilyData? GetTTGFamilyDataById(int id)
    {
        TTGFamilies.TryGetValue(id, out var ttgFamilyData);
        return ttgFamilyData;
    }

    public string GetTTGFamilyNameById(int id, Language language)
    {
        return GetTTGFamilyNameById(id, language.ToCulture());
    }

    public string GetTTGFamilyNameById(int id, CultureInfo? culture = null)
    {
        var ttgFamilyData = GetTTGFamilyDataById(id);

        return ttgFamilyData is null
            ? Translation.UnknownData(id, culture)
            : ttgFamilyData.Name.ToString(culture);
    }

    protected override void LoadLocalizedData(LangType type, Language language)
    {
        var twoLetterISOLanguageName = language.ToStringFast();
        var localizedRepository = DofusLocalizedRepository.Load<TTGLocalizedRepository>(type, language);

        foreach (var ttgEntityLocalizedData in localizedRepository.TTGEntities)
        {
            var ttgEntityData = GetTTGEntityDataById(ttgEntityLocalizedData.Id);
            ttgEntityData?.Name.TryAdd(twoLetterISOLanguageName, ttgEntityLocalizedData.Name);
        }

        foreach (var ttgFamilyLocalizedData in localizedRepository.TTGFamilies)
        {
            var ttgFamilyData = GetTTGFamilyDataById(ttgFamilyLocalizedData.Id);
            ttgFamilyData?.Name.TryAdd(twoLetterISOLanguageName, ttgFamilyLocalizedData.Name);
        }
    }
}
