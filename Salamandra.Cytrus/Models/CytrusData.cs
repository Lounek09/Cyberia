using System.Text.Json.Serialization;

namespace Salamandra.Cytrus.Models
{
    public sealed class Game
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

        public Dictionary<string, string> GetReleasesFromPlatform(string platform)
        {
            return Platforms.Where(x => x.Key.Equals(platform)).FirstOrDefault().Value;
        }

        public string GetVersionFromPlatformAndRelease(string platform, string release)
        {
            return GetReleasesFromPlatform(platform).Where(x => x.Key.Equals(release)).FirstOrDefault().Value;
        }

        public string GetManifestUrl(string platform, string release)
        {
            return $"{Constant.BASE_ADRESS}/{Name}/releases/{release}/{platform}/{GetVersionFromPlatformAndRelease(platform, release)}.manifest";
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

        public static string GetGameManifestUrl(string game, string platform, string release, string version)
        {
            return $"{Constant.BASE_ADRESS}/{game}/releases/{release}/{platform}/{version}.manifest";
        }
    }
}
