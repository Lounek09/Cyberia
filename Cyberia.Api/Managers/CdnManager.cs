using Cyberia.Api.Data;
namespace Cyberia.Api.Managers;

public static class CdnManager
{
    private static readonly Dictionary<string, bool> s_cachedUrl = [];

    /// <summary>
    /// Asynchronously retrieves the URL of the specified image in the given directory and size.
    /// </summary>
    /// <param name="category">The directory path for the image.</param>
    /// <param name="id">The ID of the image.</param>
    /// <param name="size">The desired size of the image.</param>
    /// <param name="ext">The file extension of the image.</param>
    /// <returns>
    /// The URL of the image if it exists; otherwise, the URL of the default image.
    /// </returns>
    public static async Task<string> GetImagePathAsync(string category, int id, CdnImageSize size, string ext = "png")
    {
        var url = $"{DofusApi.Config.CdnUrl}/images/dofus/{category}/{(int)size}/{id}.{ext}";

        if (!s_cachedUrl.TryGetValue(url, out var exists))
        {
            exists = await DofusApi.HttpClient.ExistsAsync(url);
            s_cachedUrl.Add(url, exists);
        }

        return exists ? url : $"{DofusApi.Config.CdnUrl}/images/dofus/others/{(int)size}/unknown.png";
    }

    /// <summary>
    /// Asynchronously retrieves the URL of the specified image in the given directory and size.
    /// </summary>
    /// <param name="category">The directory path for the image.</param>
    /// <param name="id">The ID of the image.</param>
    /// <param name="ext">The file extension of the image.</param>
    /// <returns>
    /// The URL of the image if it exists; otherwise, the URL of the default image.
    /// </returns>
    public static async Task<string> GetImagePathAsync(string category, int id, string ext = "png")
    {
        var url = $"{DofusApi.Config.CdnUrl}/images/dofus/{category}/{id}.png";

        if (!s_cachedUrl.TryGetValue(url, out var exists))
        {
            exists = await DofusApi.HttpClient.ExistsAsync(url);
            s_cachedUrl.Add(url, exists);
        }

        return exists ? url : $"{DofusApi.Config.CdnUrl}/images/dofus/others/{(int)CdnImageSize.Size128}/unknown.png";
    }
}
