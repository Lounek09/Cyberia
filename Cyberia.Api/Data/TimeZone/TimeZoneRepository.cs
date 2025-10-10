using Cyberia.Api.Data.TimeZone.Localized;
using Cyberia.Api.JsonConverters;
using Cyberia.Langzilla.Enums;

using System.Collections.ObjectModel;
using System.Globalization;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.TimeZone;

public sealed class TimeZoneRepository : DofusRepository, IDofusRepository
{
    public static string FileName => "timezones.json";

    [JsonPropertyName("T.mspd")]
    public int MilisecondPerDay { get; set; }

    [JsonPropertyName("T.hpd")]
    public int HourPerDay { get; set; }

    [JsonPropertyName("T.z")]
    public int YearLess { get; set; }

    [JsonPropertyName("T.m")]
    [JsonConverter(typeof(ReadOnlyDictionaryFromArrayConverter<int, LocalizedString>))]
    public IReadOnlyDictionary<int, LocalizedString> StartDayOfMonths { get; set; }

    [JsonConstructor]
    internal TimeZoneRepository()
    {
        StartDayOfMonths = ReadOnlyDictionary<int, LocalizedString>.Empty;
    }

    public string GetMonthNameById(int id, Language language)
    {
        return GetMonthNameById(id, language.ToCulture());
    }

    public string GetMonthNameById(int id, CultureInfo? culture = null)
    {
        var indexMax = StartDayOfMonths.Count - 1;
        if (id < 0 || id > indexMax)
        {
            return Translation.UnknownData(id, culture);
        }

        return StartDayOfMonths.ElementAt(indexMax - id).Value.ToString(culture);
    }

    public string GetMonthNameByDayOfYear(int dayOfYear, Language language)
    {
        return GetMonthNameByDayOfYear(dayOfYear, language.ToCulture());
    }

    public string GetMonthNameByDayOfYear(int dayOfYear, CultureInfo? culture = null)
    {
        foreach (var pair in StartDayOfMonths)
        {
            if (dayOfYear > pair.Key)
            {
                return pair.Value.ToString(culture);
            }
        }

        return Translation.UnknownData(dayOfYear, culture);
    }

    protected override void LoadLocalizedData(LangsIdentifier identifier)
    {
        var twoLetterISOLanguageName = identifier.Language.ToStringFast();
        var localizedRepository = DofusLocalizedRepository.Load<TimeZoneLocalizedRepository>(identifier);

        if (localizedRepository.StartDayOfMonths.Count == StartDayOfMonths.Count)
        {
            foreach (var pair in localizedRepository.StartDayOfMonths)
            {
                StartDayOfMonths[pair.Key].TryAdd(twoLetterISOLanguageName, pair.Value);
            }
        }
    }

    protected override void FinalizeLoading()
    {
        StartDayOfMonths = StartDayOfMonths.Reverse().ToDictionary();
    }
}
