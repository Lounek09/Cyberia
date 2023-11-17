using System.Text.Json;
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

        [JsonConstructor]
        internal Game()
        {
            Name = string.Empty;
            Assets = [];
            Platforms = [];
        }

        public Dictionary<string, string> GetReleasesByPlatform(string platform)
        {
            return Platforms.Where(x => x.Key.Equals(platform)).FirstOrDefault().Value;
        }

        public string? GetVersionByPlatformAndRelease(string platform, string release)
        {
            return GetReleasesByPlatform(platform).Where(x => x.Key.Equals(release)).FirstOrDefault().Value;
        }

        public string GetManifestUrl(string platform, string release)
        {
            return $"{CytrusWatcher.BASE_URL}/{Name}/releases/{release}/{platform}/{GetVersionByPlatformAndRelease(platform, release)}.manifest";
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

        [JsonConstructor]
        internal CytrusData()
        {
            Name = string.Empty;
            Games = [];
        }

        internal static CytrusData LoadFromFile(string path)
        {
            if (!File.Exists(path))
            {
                return new();
            }

            string json = File.ReadAllText(path);
            return Load(json);
        }

        internal static CytrusData Load(string json)
        {
            CytrusData? data = JsonSerializer.Deserialize<CytrusData>(json);
            if (data is null)
            {
                Log.Error("Failed to deserialize the JSON to initialize {TypeName}", typeof(CytrusData).Name);
                return new();
            }

            return data;
        }

        public static string GetGameManifestUrl(string game, string platform, string release, string version)
        {
            return $"{CytrusWatcher.BASE_URL}/{game}/releases/{release}/{platform}/{version}.manifest";
        }
    }
}
