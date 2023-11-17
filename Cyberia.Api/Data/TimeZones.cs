using Cyberia.Api.JsonConverters;

using System.Text.Json.Serialization;

namespace Cyberia.Api.Data
{
    public sealed class TimeZonesData
    {
        private const string FILE_NAME = "timezones.json";

        [JsonPropertyName("T.mspd")]
        public int MilisecondPerDay { get; set; }

        [JsonPropertyName("T.hpd")]
        public int HourPerDay { get; set; }

        [JsonPropertyName("T.z")]
        public int YearLess { get; set; }

        [JsonPropertyName("T.m")]
        [JsonConverter(typeof(DictionaryConverter<int, string>))]
        public Dictionary<int, string> StartDayOfMonths { get; set; }

        [JsonConstructor]
        public TimeZonesData()
        {
            StartDayOfMonths = [];
        }

        internal static TimeZonesData Load()
        {
            return Json.LoadFromFile<TimeZonesData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
        }

        public string GetMonth(int dayOfYear)
        {
            foreach (KeyValuePair<int, string> pair in Enumerable.Reverse(StartDayOfMonths))
            {
                if (dayOfYear > pair.Key)
                {
                    return pair.Value;
                }
            }

            return "";
        }
    }
}
