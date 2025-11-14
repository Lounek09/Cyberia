namespace Cyberia.Utils.Extensions;

/// <summary>
/// Provides extension methods for <see cref="HttpClient"/>.
/// </summary>
public static class HttpClientExtensions
{
    extension(HttpClient client)
    {
        /// <summary>
        /// Checks if the specified URL exists by sending a HEAD request and checking the response status.
        /// </summary>
        /// <param name="url">The URL to check.</param>
        /// <returns><see langword="true"/> if the URL return a success status code; otherwise, <see langword="false"/>.</returns>
        public async Task<bool> ExistsAsync(string url)
        {
            try
            {
                using HttpRequestMessage request = new(HttpMethod.Head, url);
                using var response = await client.SendAsync(request);

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
