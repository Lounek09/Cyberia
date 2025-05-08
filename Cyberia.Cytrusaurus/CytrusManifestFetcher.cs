using Cyberia.Cytrusaurus.Models.FlatBuffers;

using Google.FlatBuffers;

namespace Cyberia.Cytrusaurus;

/// <summary>
/// Represents a service for fetching game manifests from Cytrus.
/// </summary>
public interface ICytrusManifestFetcher
{
    /// <summary>
    /// Gets the manifest for the specified game, platform, release, and version.
    /// </summary>
    /// <param name="game">The name of the game.</param>
    /// <param name="platform">The platform for which the game is released.</param>
    /// <param name="release">The release name of the game.</param>
    /// <param name="version">The version of the game.</param>
    /// <returns>The fetched manifest; otherwise, <see langword="null"/>.</returns>
    Task<Manifest?> GetAsync(string game, string platform, string release, string version);
}

public sealed class CytrusManifestFetcher : ICytrusManifestFetcher
{
    private readonly HttpClient _httpClient;
    private readonly HttpRetryPolicy _httpRetryPolicy;

    /// <summary>
    /// Initializes a new instance of the <see cref="CytrusManifestFetcher"/> class.
    /// </summary>
    public CytrusManifestFetcher()
    {
        _httpClient = new()
        {
            BaseAddress = new Uri(CytrusWatcher.BaseUrl)
        };
        _httpRetryPolicy = new(5, TimeSpan.FromSeconds(1));
    }

    /// <summary>
    /// Gets the manifest for the specified game, platform, release, and version.
    /// </summary>
    /// <param name="game">The name of the game.</param>
    /// <param name="platform">The platform for which the game is released.</param>
    /// <param name="release">The release name of the game.</param>
    /// <param name="version">The version of the game.</param>
    /// <returns>The fetched manifest; otherwise, <see langword="null"/>.</returns>
    public async Task<Manifest?> GetAsync(string game, string platform, string release, string version)
    {
        var route = GetRoute(game, platform, release, version);

        try
        {
            using var response = await _httpRetryPolicy.ExecuteAsync(() => _httpClient.GetAsync(route));
            response.EnsureSuccessStatusCode();

            var metafile = await response.Content.ReadAsByteArrayAsync();
            ByteBuffer buffer = new(metafile);

            return Manifest.GetRootAsManifest(buffer);
        }
        catch (HttpRequestException e)
        {
            Log.Error(e, "An error occurred while sending Get request to {CytrusManifestUrl}", $"{CytrusWatcher.BaseUrl}/{route}");
        }

        return null;
    }

    /// <summary>
    /// Gets the route of the manifest.
    /// </summary>
    /// <param name="game">The name of the game.</param>
    /// <param name="platform">The platform for which the game is released.</param>
    /// <param name="release">The release name of the game.</param>
    /// <param name="version">The version of the game.</param>
    /// <returns>The route of the manifest.</returns>
    internal static string GetRoute(string game, string platform, string release, string version)
    {
        return $"{game}/releases/{release}/{platform}/{version}.manifest";
    }
}
