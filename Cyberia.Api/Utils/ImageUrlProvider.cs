using Cyberia.Api.Data;

using System.Collections.Concurrent;

namespace Cyberia.Api.Utils;

/// <summary>
/// Provides URLs for CDN-hosted images with caching and verification.
/// </summary>
public static class ImageUrlProvider
{
    private static readonly ConcurrentDictionary<string, (bool Exists, DateTime LastCheck)> s_cachedUrl = [];
    private static readonly TimeSpan s_cacheDuration = TimeSpan.FromHours(24);

    /// <summary>
    /// Gets the verified image path for a resource by its directory and ID.
    /// </summary>
    /// <param name="directory">The directory of the resource</param>
    /// <param name="id">The ID of the resource</param>
    /// <param name="size">The image size</param>
    /// <param name="ext">The image file extension (default: png)</param>
    /// <returns>A valid image URL, or the fallback URL if not found</returns>
    public static async Task<string> GetImagePathAsync(string directory, int id, CdnImageSize size, string ext = "png")
    {
        return await GetImagePathAsync(directory, id.ToString(), size, ext);
    }

    /// <summary>
    /// Gets the verified image path for a resource by its directory and name.
    /// </summary>
    /// <param name="directory">The directory of the resource</param>
    /// <param name="name">The name of the resource</param>
    /// <param name="size">The image size</param>
    /// <param name="ext">The image file extension (default: png)</param>
    /// <returns>A valid image URL, or the fallback URL if not found</returns>
    public static async Task<string> GetImagePathAsync(string directory, string name, CdnImageSize size, string ext = "png")
    {
        var url = $"{DofusApi.Config.CdnUrl}/images/dofus/{directory}/{(int)size}/{name}.{ext}";
        var exists = await ExistsAsync(url);

        return exists ? url : $"{DofusApi.Config.CdnUrl}/images/dofus/others/{(int)size}/unknown.png";
    }

    /// <inheritdoc cref="GetImagePathAsync(string, int, CdnImageSize, string)"/>
    public static async Task<string> GetImagePathAsync(string directory, int id, string ext = "png")
    {
        return await GetImagePathAsync(directory, id.ToString(), ext);
    }

    /// <inheritdoc cref="GetImagePathAsync(string, string, CdnImageSize, string)"/>
    public static async Task<string> GetImagePathAsync(string directory, string name, string ext = "png")
    {
        var url = $"{DofusApi.Config.CdnUrl}/images/dofus/{directory}/{name}.{ext}";
        var exists = await ExistsAsync(url);

        return exists ? url : $"{DofusApi.Config.CdnUrl}/images/dofus/others/{(int)CdnImageSize.Size128}/unknown.png";
    }

    /// <summary>
    /// Clears the URL cache.
    /// </summary>
    internal static void ClearCache()
    {
        s_cachedUrl.Clear();
    }

    /// <summary>
    /// Checks if a URL exists with caching.
    /// </summary>
    /// <param name="url">The URL to check</param>
    /// <returns><see langword="true"/> if the URL exists; otherwise, <see langword="false"/></returns>
    private static async Task<bool> ExistsAsync(string url)
    {
        var now = DateTime.Now;

        if (s_cachedUrl.TryGetValue(url, out var cacheEntry) &&
            (now - cacheEntry.LastCheck) < s_cacheDuration)
        {
            return cacheEntry.Exists;
        }

        var exists = await DofusApi.HttpClient.ExistsAsync(url);
        s_cachedUrl[url] = (exists, now);

        return exists;
    }
}
