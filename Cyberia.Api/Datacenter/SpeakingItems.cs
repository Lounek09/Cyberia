using System.Text.Json.Serialization;

namespace Cyberia.Api.DatacenterNS
{
    public sealed class SpeakingItemData
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("m")]
        public string Message { get; init; }

        [JsonPropertyName("s")]
        public int SoundId { get; init; }

        [JsonPropertyName("r")]
        public string ItemsIdCanUse { get; init; }

        [JsonPropertyName("l")]
        public int RequiredLevel { get; init; }

        [JsonPropertyName("p")]
        public double Probability { get; init; }

        public SpeakingItemData()
        {
            Message = string.Empty;
            ItemsIdCanUse = string.Empty;
        }
    }

    public sealed class SpeakingItemsData
    {
        private const string FILE_NAME = "speakingitems.json";

        [JsonPropertyName("SIM")]
        public List<SpeakingItemData> SpeakingItems { get; init; }

        //TODO: SIT in SpeakingItems lang

        public SpeakingItemsData()
        {
            SpeakingItems = new();
        }

        internal static SpeakingItemsData Build()
        {
            return Json.LoadFromFile<SpeakingItemsData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
        }
    }
}
