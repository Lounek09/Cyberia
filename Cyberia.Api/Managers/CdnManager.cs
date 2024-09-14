using Cyberia.Api.Data;
using Cyberia.Utils.Extensions;
using System.Collections.Concurrent;

namespace Cyberia.Api.Managers;

public static class CdnManager
{
    private static readonly ConcurrentDictionary<string, (bool Exists, DateTime LastCheck)> s_cachedUrl = [];
    private static readonly TimeSpan s_cacheDuration = TimeSpan.FromHours(24);

    public static async Task<string> GetImagePathAsync(string category, int id, CdnImageSize size, string ext = "png")
    {
        var url = $"{DofusApi.Config.CdnUrl}/images/dofus/{category}/{(int)size}/{id}.{ext}";
        var exists = await ExistsAsync(url);

        return exists ? url : $"{DofusApi.Config.CdnUrl}/images/dofus/others/{(int)size}/unknown.png";
    }

    public static async Task<string> GetImagePathAsync(string category, int id, string ext = "png")
    {
        var url = $"{DofusApi.Config.CdnUrl}/images/dofus/{category}/{id}.{ext}";
        var exists = await ExistsAsync(url);

        return exists ? url : $"{DofusApi.Config.CdnUrl}/images/dofus/others/{(int)CdnImageSize.Size128}/unknown.png";
    }

    public static void ClearCache()
    {
        s_cachedUrl.Clear();
    }

    private static async Task<bool> ExistsAsync(string url)
    {
        var now = DateTime.Now;

        if (s_cachedUrl.TryGetValue(url, out var cacheEntry) && (now - cacheEntry.LastCheck) < s_cacheDuration)
        {
            return cacheEntry.Exists;
        }

        var exists = await DofusApi.HttpClient.ExistsAsync(url);
        s_cachedUrl.TryAdd(url, (exists, now));

        return exists;
    }
}
