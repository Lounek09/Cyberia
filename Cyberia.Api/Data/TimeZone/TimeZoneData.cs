using Cyberia.Api.JsonConverters;

using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.TimeZone;

public sealed class TimeZoneData : IDofusData
{
    private const string FILE_NAME = "timezones.json";

    [JsonPropertyName("T.mspd")]
    public int MilisecondPerDay { get; set; }

    [JsonPropertyName("T.hpd")]
    public int HourPerDay { get; set; }

    [JsonPropertyName("T.z")]
    public int YearLess { get; set; }

    [JsonPropertyName("T.m")]
    [JsonConverter(typeof(DictionaryFromArrayConverter<int, string>))]
    public IReadOnlyDictionary<int, string> StartDayOfMonths { get; set; }

    [JsonConstructor]
    internal TimeZoneData()
    {
        StartDayOfMonths = new Dictionary<int, string>();
    }

    internal static TimeZoneData Load()
    {
        return Datacenter.LoadDataFromFile<TimeZoneData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
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
