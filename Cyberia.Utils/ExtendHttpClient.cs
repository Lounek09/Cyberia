namespace Cyberia.Utils;

public static class ExtendHttpClient
{
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
