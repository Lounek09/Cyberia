using Cyberia.Api.Data;
namespace Cyberia.Api.Managers;

public static class CdnManager
{
    private static readonly Dictionary<string, (bool Exists, DateTime LastCheck)> s_cachedUrl = [];

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

    private static async Task<bool> ExistsAsync(string url)
    {
        if (!s_cachedUrl.TryGetValue(url, out var cacheEntry) || (DateTime.Now - cacheEntry.LastCheck).TotalHours > 24)
        {
            cacheEntry = (await DofusApi.HttpClient.ExistsAsync(url), DateTime.Now);
            s_cachedUrl.Add(url, cacheEntry);
        }

        return cacheEntry.Exists;
    }
}
