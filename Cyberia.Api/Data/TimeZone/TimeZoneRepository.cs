using Cyberia.Api.JsonConverters;

using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.TimeZone;

public sealed class TimeZoneRepository : IDofusRepository
{
    private const string c_fileName = "timezones.json";

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

    internal static TimeZoneRepository Load(string directoryPath)
    {
        var filePath = Path.Join(directoryPath, c_fileName);

        return Datacenter.LoadRepository<TimeZoneRepository>(filePath);
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
