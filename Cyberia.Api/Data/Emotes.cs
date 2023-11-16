using System.Text.Json.Serialization;

namespace Cyberia.Api.Data
{
    public sealed class EmoteData
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("n")]
        public string Name { get; init; }

        [JsonPropertyName("s")]
        public string Shortcut { get; init; }

        [JsonConstructor]
        internal EmoteData()
        {
            Name = string.Empty;
            Shortcut = string.Empty;
        }
    }

    public sealed class EmotesData
    {
        private const string FILE_NAME = "emotes.json";

        [JsonPropertyName("EM")]
        public List<EmoteData> Emotes { get; init; }

        [JsonConstructor]
        public EmotesData()
        {
            Emotes = [];
        }

        internal static EmotesData Load()
        {
            return Json.LoadFromFile<EmotesData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
        }

        public EmoteData? GetEmoteById(int id)
        {
            return Emotes.Find(x => x.Id == id);
        }

        public string GetEmoteNameById(int id)
        {
            EmoteData? emoteData = GetEmoteById(id);

            return emoteData is null ? PatternDecoder.Description(Resources.Unknown_Data, id) : emoteData.Name;
        }
    }
}
