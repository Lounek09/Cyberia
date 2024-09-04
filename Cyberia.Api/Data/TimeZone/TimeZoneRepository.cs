using Cyberia.Api.JsonConverters;

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
        StartDayOfMonths = new Dictionary<int, LocalizedString>();
    }

    public string GetMonth(int dayOfYear)
    {
        foreach (var pair in Enumerable.Reverse(StartDayOfMonths))
        {
            if (dayOfYear > pair.Key)
            {
                return pair.Value;
            }
        }

        return string.Empty;
    }
}
