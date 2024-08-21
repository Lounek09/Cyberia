using System.Text.Json.Serialization;

namespace Cyberia.Cytrusaurus.Models;

/// <summary>
/// Represents a game in Cytrus.
/// </summary>
public sealed class CytrusGame : IComparable<CytrusGame>
{
    public const string MetaAssets = "meta";

    public const string DarwinPlatform = "darwin";
    public const string LinuxPlatform = "linux";
    public const string WindowsPlatform = "windows";

    public const string MainRelease = "main";
    public const string BetaRelease = "beta";

    /// <summary>
    /// Name of the game
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; init; }

    /// <summary>
    /// Order of the game
    /// </summary>
    [JsonPropertyName("order")]
    public int Order { get; init; }

    /// <summary>
    /// ID of the game
    /// </summary>
    [JsonPropertyName("gameId")]
    public int GameId { get; init; }

    /// <summary>
    /// Assets of the game
    /// </summary>
    [JsonPropertyName("assets")]
    public IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> Assets { get; init; }

    /// <summary>
    /// Platforms the game is available on
    /// </summary>
    [JsonPropertyName("platforms")]
    public IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> Platforms { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CytrusGame"/> class.
    /// </summary>
    [JsonConstructor]
    internal CytrusGame()
    {
        Name = string.Empty;
        Assets = new Dictionary<string, IReadOnlyDictionary<string, string>>();
        Platforms = new Dictionary<string, IReadOnlyDictionary<string, string>>();
    }

    /// <summary>
    /// Retrieves assets by their name.
    /// </summary>
    /// <param name="assetsName">The name of the assets.</param>
    /// <returns>The assets if found; otherwise, an empty dictionary.</returns>
    public IReadOnlyDictionary<string, string> GetAssetsByName(string assetName)
    {
        Assets.TryGetValue(assetName, out var asset);
        return asset ?? new Dictionary<string, string>();
    }

    /// <summary>
    /// Retrieves the hash of a specific release of a specific asset.
    /// </summary>
    /// <param name="assetsName">The name of the asset.</param>
    /// <param name="releaseName">The name of the release.</param>
    /// <returns>The hash if found; otherwise, <see cref="string.Empty"/>.</returns>
    public string GetAssetHashByNameAndReleaseName(string assetName, string releaseName)
    {
        if (!Assets.TryGetValue(assetName, out var asset))
        {
            return string.Empty;
        }

        if (!asset.TryGetValue(releaseName, out var hash))
        {
            return string.Empty;
        }

        return hash;
    }

    /// <summary>
    /// Retrieves platforms by their name.
    /// </summary>
    /// <param name="platform">The name of the platform.</param>
    /// <returns>The platform if found; otherwise, an empty dictionary.</returns>
    public IReadOnlyDictionary<string, string> GetReleasesByPlatformName(string platformName)
    {
        Platforms.TryGetValue(platformName, out var releases);
        return releases ?? new Dictionary<string, string>();
    }

    /// <summary>
    /// Retrieves the version of a specific release on a specific platform.
    /// </summary>
    /// <param name="platformName">The name of the platform.</param>
    /// <param name="releaseName">The name of the release.</param>
    /// <returns>The version if found; otherwise, <see cref="string.Empty"/>.</returns>
    public string GetVersionByPlatformNameAndReleaseName(string platformName, string releaseName)
    {
        if (!Platforms.TryGetValue(platformName, out var platform))
        {
            return string.Empty;
        }

        if (!platform.TryGetValue(releaseName, out var version))
        {
            return string.Empty;
        }

        return version;
    }

    public int CompareTo(CytrusGame? other)
    {
        if (other is null)
        {
            return 1;
        }

        return Order.CompareTo(other.Order);
    }
}
