using Moq;
using Moq.Protected;

using System.Net;

namespace Cyberia.Test.Utils;

[TestClass]
public sealed class ExtendHttpClientTests
{
    private Mock<HttpMessageHandler> _mockHttpMessageHandler = default!;

    [TestInitialize]
    public void Initialize()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
    }

    #region ExistsAsync

    [TestMethod]
    public async Task ExistsAsync_WhenUrlExists_ReturnsTrue()
    {
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
            })
            .Verifiable();

        var value = new HttpClient(_mockHttpMessageHandler.Object);

        var result = await value.ExistsAsync("http://example.com/200.txt");


        Assert.IsTrue(result);
    }

    [TestMethod]
    public async Task ExistsAsync_WhenUrlDoesNotExist_ReturnsFalse()
    {
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
            })
            .Verifiable();

        var value = new HttpClient(_mockHttpMessageHandler.Object);

        var result = await value.ExistsAsync("http://example.com/404.txt");

        Assert.IsFalse(result);
    }

    [TestMethod]
    public async Task ExistsAsync_WithException_ReturnsFalse()
    {
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new HttpRequestException())
            .Verifiable();

        var value = new HttpClient(_mockHttpMessageHandler.Object);

        var result = await value.ExistsAsync("http://example.com/error.txt");

        Assert.IsFalse(result);
    }

    #endregion
}
