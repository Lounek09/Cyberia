using System.Text.Json.Serialization;

namespace Cyberia.Api.DatacenterNS
{
    public sealed class Emote
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("n")]
        public string Name { get; init; }

        [JsonPropertyName("s")]
        public string Shortcut { get; init; }

        public Emote()
        {
            Name = string.Empty;
            Shortcut = string.Empty;
        }
    }

    public sealed class EmotesData
    {
        private const string FILE_NAME = "emotes.json";

        [JsonPropertyName("EM")]
        public List<Emote> Emotes { get; init; }

        public EmotesData()
        {
            Emotes = new();
        }

        internal static EmotesData Build()
        {
            return Json.LoadFromFile<EmotesData>($"{DofusApi.OUTPUT_PATH}/{FILE_NAME}");
        }

        public Emote? GetEmoteById(int id)
        {
            return Emotes.Find(x => x.Id == id);
        }

        public string GetEmoteNameById(int id)
        {
            Emote? emote = GetEmoteById(id);

            return emote is null ? $"Inconnu ({id})" : emote.Name;
        }
    }
}
