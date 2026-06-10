using System.Net;

namespace Cyberia.Utils;

/// <summary>
/// Defines a policy for retrying HTTP requests.
/// </summary>
public sealed class HttpRetryPolicy
{
    private readonly int _maxRetries;
    private readonly TimeSpan _startInterval;

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpRetryPolicy"/> class.
    /// </summary>
    /// <param name="maxRetries">The maximum number of retries.</param>
    /// <param name="startInterval">The initial interval between retries.</param>
    public HttpRetryPolicy(int maxRetries, TimeSpan startInterval)
    {
        _maxRetries = maxRetries;
        _startInterval = startInterval;
    }

    /// <summary>
    /// Executes the specified operation according to the retry policy.
    /// </summary>
    /// <param name="operation">The operation to execute.</param>
    /// <param name="retryCount">The current retry count. Do not set.</param>
    /// <returns>The HTTP response message.</returns>
    public async Task<HttpResponseMessage> ExecuteAsync(Func<Task<HttpResponseMessage>> operation, int retryCount = 0)
    {
        var curentInterval = _startInterval * Math.Pow(2, retryCount);

        try
        {
            var response = await operation();

            if (ShouldRetry(response))
            {
                if (retryCount < _maxRetries)
                {
                    Log.Warning("The request failed with status {HttpStatusCode}. Retrying in {RetryInterval}ms.", response.StatusCode, curentInterval.TotalMilliseconds);
                    await Task.Delay(curentInterval);
                    return await ExecuteAsync(operation, retryCount + 1);
                }

                throw new HttpRequestException($"The request failed after the maximum number of retries.", null, response.StatusCode);
            }

            return response;
        }
        catch (TaskCanceledException e) when (e.InnerException is TimeoutException)
        {
            if (retryCount < _maxRetries)
            {
                Log.Warning("The request timed out. Retrying in {RetryInterval}ms.", curentInterval.TotalMilliseconds);
                await Task.Delay(curentInterval);
                return await ExecuteAsync(operation, retryCount + 1);
            }

            throw;
        }
        catch (HttpRequestException e) when (e.InnerException is IOException)
        {
            if (retryCount < _maxRetries)
            {
                Log.Warning("Connection reset by peer. Retrying in {RetryInterval}ms.", curentInterval.TotalMilliseconds);
                await Task.Delay(curentInterval);
                return await ExecuteAsync(operation, retryCount + 1);
            }

            throw;
        }
        catch (Exception)
        {
            throw;
        }
    }

    /// <summary>
    /// Determines whether the specified HTTP response should be retried.
    /// </summary>
    /// <param name="response">The HTTP response.</param>
    /// <returns>True if the response should be retried; otherwise, false.</returns>
    private static bool ShouldRetry(HttpResponseMessage response)
    {
        return response.StatusCode is HttpStatusCode.RequestTimeout
            or HttpStatusCode.TooManyRequests
            or HttpStatusCode.InternalServerError
            or HttpStatusCode.BadGateway
            or HttpStatusCode.ServiceUnavailable
            or HttpStatusCode.GatewayTimeout;
    }
}
