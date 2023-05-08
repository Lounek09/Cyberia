using Cyberia.Api.Factories.JsonConverter;
using System.Text.Json.Serialization;

namespace Cyberia.Api.DatacenterNS
{
    public sealed class TimeZonesData
    {
        private const string FILE_NAME = "timezones.json";

        [JsonPropertyName("Tmspd")]
        public int MilisecondPerDay { get; set; }

        [JsonPropertyName("Thpd")]
        public int HourPerDay { get; set; }

        [JsonPropertyName("Tz")]
        public int YearLess { get; set; }

        [JsonPropertyName("Tm")]
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
