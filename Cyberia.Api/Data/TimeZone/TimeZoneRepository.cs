using Cyberia.Api.Data.TimeZone.Localized;
using Cyberia.Api.JsonConverters;
using Cyberia.Langzilla.Enums;

using System.Collections.ObjectModel;
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

    public string GetMonth(int dayOfYear)
    {
        foreach (var pair in StartDayOfMonths)
        {
            if (dayOfYear > pair.Key)
            {
                return pair.Value;
            }
        }

        return string.Empty;
    }

    protected override void LoadCustomData()
    {
        StartDayOfMonths = StartDayOfMonths.Reverse().ToDictionary();
    }

    protected override void LoadLocalizedData(LangType type, LangLanguage language)
    {
        var twoLetterISOLanguageName = language.ToCulture().TwoLetterISOLanguageName;
        var localizedRepository = DofusLocalizedRepository.Load<TimeZoneLocalizedRepository>(type, language);

        if (localizedRepository.StartDayOfMonths.Count == StartDayOfMonths.Count)
        {
            foreach (var pair in localizedRepository.StartDayOfMonths.Reverse())
            {
                StartDayOfMonths[pair.Key].Add(twoLetterISOLanguageName, pair.Value);
            }
        }
    }
}
