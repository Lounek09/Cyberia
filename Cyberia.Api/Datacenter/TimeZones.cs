using Cyberia.Api.Parser.JsonConverter;

using System.Text.Json.Serialization;

namespace Cyberia.Api.DatacenterNS
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
        [JsonConverter(typeof(DictionaryJsonConverter<int, string>))]
        public Dictionary<int, string> StartDayOfMonths { get; set; }

        public TimeZonesData()
        {
            StartDayOfMonths = new();
        }

        internal static TimeZonesData Build()
        {
            return Json.LoadFromFile<TimeZonesData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
        }

        public string GetMonth(int dayOfYear)
        {
            foreach (KeyValuePair<int, string> month in Enumerable.Reverse(StartDayOfMonths))
            {
                if (dayOfYear > month.Key)
                    return month.Value;
            }

            return "";
        }
    }
}
