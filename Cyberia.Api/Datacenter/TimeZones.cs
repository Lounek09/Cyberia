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
            return Json.LoadFromFile<TimeZonesData>($"{DofusApi.OUTPUT_PATH}/{FILE_NAME}");
        }

        public int GetYear()
        {
            return DateTime.Now.Year + YearLess;
        }

        public string GetMonth()
        {
            foreach (KeyValuePair<int, string> month in Enumerable.Reverse(StartDayOfMonths))
            {
                if (DateTime.Now.DayOfYear > month.Key)
                    return month.Value;
            }

            return "";
        }

        public static int GetDay()
        {
            return DateTime.Now.Day;
        }

        public string GetDate()
        {
            return $"{GetDay()} {GetMonth()} {GetYear()}";
        }
    }
}
