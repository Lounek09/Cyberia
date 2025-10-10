namespace Cyberia.Utils.Extensions;

/// <summary>
/// Provides extension methods for <see cref="HttpClient"/>.
/// </summary>
public static class HttpClientExtensions
{
    /// <summary>
    /// Checks if the specified URL exists by sending a HEAD request and checking the response status.
    /// </summary>
    /// <param name="httpClient">The HttpClient instance.</param>
    /// <param name="url">The URL to check.</param>
    /// <returns><see langword="true"/> if the URL return a success status code; otherwise, <see langword="false"/>.</returns>
    public static async Task<bool> ExistsAsync(this HttpClient httpClient, string url)
    {
        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Head, url);
            using var response = await httpClient.SendAsync(request);

            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}
