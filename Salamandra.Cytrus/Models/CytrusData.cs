using System.Text.Json.Serialization;

namespace Salamandra.Cytrus.Models
{
    public class Game
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("order")]
        public int Order { get; set; }

        [JsonPropertyName("gameId")]
        public int GameId { get; set; }

        [JsonPropertyName("assets")]
        public Dictionary<string, Dictionary<string, string>> Assets { get; set; }

        [JsonPropertyName("platforms")]
        public Dictionary<string, Dictionary<string, string>> Platforms { get; set; }

        public Game()
        {
            Name = string.Empty;
            Assets = new();
            Platforms = new();
        }
    }

    public sealed class CytrusData
    {
        [JsonPropertyName("version")]
        public int Version { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("games")]
        public Dictionary<string, Game> Games { get; set; }

        public CytrusData()
        {
            Name = string.Empty;
            Games = new();
        }
    }
}
