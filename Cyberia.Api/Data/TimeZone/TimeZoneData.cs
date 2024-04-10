using Cyberia.Api.JsonConverters;

using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.TimeZone;

public sealed class TimeZoneData : IDofusData
{
    private const string c_fileName = "timezones.json";

    private static readonly string s_filePath = Path.Join(DofusApi.OutputPath, c_fileName);

    [JsonPropertyName("T.mspd")]
    public int MilisecondPerDay { get; set; }

    [JsonPropertyName("T.hpd")]
    public int HourPerDay { get; set; }

    [JsonPropertyName("T.z")]
    public int YearLess { get; set; }

    [JsonPropertyName("T.m")]
    [JsonConverter(typeof(ReadOnlyDictionaryFromArrayConverter<int, string>))]
    public IReadOnlyDictionary<int, string> StartDayOfMonths { get; set; }

    [JsonConstructor]
    internal TimeZoneData()
    {
        StartDayOfMonths = new Dictionary<int, string>();
    }

    internal static async Task<TimeZoneData> LoadAsync()
    {
        return await Datacenter.LoadDataAsync<TimeZoneData>(s_filePath);
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
