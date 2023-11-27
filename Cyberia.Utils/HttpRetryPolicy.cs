using System.Net;

namespace Cyberia.Utils;

public sealed class HttpRetryPolicy
{
    private readonly int _maxRetries;
    private readonly TimeSpan _retryInterval;

    public HttpRetryPolicy(int maxRetries, TimeSpan retryInterval)
    {
        _maxRetries = maxRetries;
        _retryInterval = retryInterval;
    }

    public async Task<HttpResponseMessage> ExecuteAsync(Func<Task<HttpResponseMessage>> operation, int retryCount = 0)
    {
        var curentRetryInterval = _retryInterval * Math.Pow(2, retryCount) + TimeSpan.FromMilliseconds(Random.Shared.Next(1000));

        try
        {
            var response = await operation();

            if (ShouldRetry(response))
            {
                if (retryCount < _maxRetries)
                {
                    Log.Warning("The request failed with status {HttpStatusCode}. Retrying in {RetryInterval}ms.", response.StatusCode, curentRetryInterval.TotalMilliseconds);
                    await Task.Delay(curentRetryInterval);
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
                Log.Warning("The request timed out. Retrying in {RetryInterval}ms.", curentRetryInterval.TotalMilliseconds);
                await Task.Delay(curentRetryInterval);
                return await ExecuteAsync(operation, retryCount + 1);
            }

            throw;
        }
        catch (HttpRequestException e) when (e.InnerException is IOException)
        {
            if (retryCount < _maxRetries)
            {
                Log.Warning("Connection reset by peer. Retrying in {RetryInterval}ms.", curentRetryInterval.TotalMilliseconds);
                await Task.Delay(curentRetryInterval);
                return await ExecuteAsync(operation, retryCount + 1);
            }

            throw;
        }
        catch (Exception)
        {
            throw;
        }
    }

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
