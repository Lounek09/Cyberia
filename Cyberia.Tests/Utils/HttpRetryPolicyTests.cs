using System.Net;

namespace Cyberia.Tests.Utils;

[TestClass]
public class HttpRetryPolicyTests
{
    #region ExecuteAsync

    [TestMethod]
    public async Task ExecuteAsync_RetriesOn503ServiceUnavailable_UntilSuccess()
    {
        var maxRetries = 2;
        HttpRetryPolicy policy = new(maxRetries, TimeSpan.FromMilliseconds(10));
        var callCount = 0;

        Task<HttpResponseMessage> operation()
        {
            if (callCount < maxRetries)
            {
                callCount++;
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.ServiceUnavailable));
            }

            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
        }

        var result = await policy.ExecuteAsync(operation);

        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        Assert.AreEqual(maxRetries, callCount);
    }

    [TestMethod]
    public async Task ExecuteAsync_RetriesOnTimeoutException_UntilMaxRetriesExceeded()
    {
        var maxRetries = 1;
        HttpRetryPolicy policy = new(maxRetries, TimeSpan.FromMilliseconds(10));
        var callCount = 0;

        Task<HttpResponseMessage> operation()
        {
            callCount++;
            throw new TaskCanceledException("The operation has timed out.", new TimeoutException());
        }

        await Assert.ThrowsAsync<TaskCanceledException>(() => policy.ExecuteAsync(operation));
        Assert.AreEqual(maxRetries + 1, callCount);
    }

    #endregion
}
