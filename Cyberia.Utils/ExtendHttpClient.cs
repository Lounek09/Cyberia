namespace Cyberia.Utils
{
    public static class ExtendHttpClient
    {
        public static async Task<bool> CheckIfPageExistsAsync(this HttpClient httpClient, string url)
        {
            try
            {
                HttpResponseMessage result = await httpClient.GetAsync(url).ConfigureAwait(false);

                return result.IsSuccessStatusCode;
            }
            catch (HttpRequestException)
            {
                return false;
            }
        }
    }
}
