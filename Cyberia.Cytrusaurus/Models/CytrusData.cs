using System.Text.Json.Serialization;

namespace Cyberia.Cytrusaurus.Models
{
    public sealed class Game
    {
        [JsonPropertyName("name")]
        public string Name { get; init; }

        [JsonPropertyName("order")]
        public int Order { get; init; }

        [JsonPropertyName("gameId")]
        public int GameId { get; init; }

        [JsonPropertyName("assets")]
        public Dictionary<string, Dictionary<string, string>> Assets { get; init; }

        [JsonPropertyName("platforms")]
        public Dictionary<string, Dictionary<string, string>> Platforms { get; init; }

        public Game()
        {
            Name = string.Empty;
            Assets = [];
            Platforms = [];
        }

        public Dictionary<string, string> GetReleasesFromPlatform(string platform)
        {
            return Platforms.Where(x => x.Key.Equals(platform)).FirstOrDefault().Value;
        }

        public string? GetVersionFromPlatformAndRelease(string platform, string release)
        {
            return GetReleasesFromPlatform(platform).Where(x => x.Key.Equals(release)).FirstOrDefault().Value;
        }

        public string GetManifestUrl(string platform, string release)
        {
            return $"{CytrusWatcher.BASE_URL}/{Name}/releases/{release}/{platform}/{GetVersionFromPlatformAndRelease(platform, release)}.manifest";
        }
    }

    public sealed class CytrusData
    {
        [JsonPropertyName("version")]
        public int Version { get; init; }

        [JsonPropertyName("name")]
        public string Name { get; init; }

        [JsonPropertyName("games")]
        public Dictionary<string, Game> Games { get; init; }

        public CytrusData()
        {
            Name = string.Empty;
            Games = [];
        }

        public static string GetGameManifestUrl(string game, string platform, string release, string version)
        {
            return $"{CytrusWatcher.BASE_URL}/{game}/releases/{release}/{platform}/{version}.manifest";
        }
    }
}
