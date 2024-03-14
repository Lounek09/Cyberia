using Moq;
using Moq.Protected;

using System.Net;

namespace Cyberia.Test.Utils;

[TestClass]
public sealed class ExtendHttpClientTests
{
    #region ExistsAsync

    [TestMethod]
    public async Task ExistsAsync_WhenUrlExists_ReturnsTrue()
    {
        Mock<HttpMessageHandler> mock = new();
        mock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
            })
            .Verifiable();

        var value = new HttpClient(mock.Object);

        var result = await ExtendHttpClient.ExistsAsync(value, "http://example.com/200.txt");

        Assert.IsTrue(result);
    }

    [TestMethod]
    public async Task ExistsAsync_WhenUrlDoesNotExist_ReturnsFalse()
    {
        Mock<HttpMessageHandler> mock = new();
        mock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
            })
            .Verifiable();

        var value = new HttpClient(mock.Object);

        var result = await ExtendHttpClient.ExistsAsync(value, "http://example.com/404.txt");

        Assert.IsFalse(result);
    }

    [TestMethod]
    public async Task ExistsAsync_WithException_ReturnsFalse()
    {
        Mock<HttpMessageHandler> mock = new();
        mock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new HttpRequestException())
            .Verifiable();

        var value = new HttpClient(mock.Object);

        var result = await ExtendHttpClient.ExistsAsync(value, "http://example.com/error.txt");

        Assert.IsFalse(result);
    }

    #endregion
}
