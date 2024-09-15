using System.Net;

namespace Cyberia.Tests.Utils.Extensions;

[TestClass]
public sealed class HttpClientExtensionsTests
{
    #region ExistsAsync

    [TestMethod]
    public async Task ExistsAsync_WhenUrlExists_ReturnsTrue()
    {
        var mock = SharedData.SetupMockHttpMessageHandlerForSuccessfullResponse(
            new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
            }
         );

        var httpClient = new HttpClient(mock.Object);

        var result = await httpClient.ExistsAsync("http://example.com/200.txt");


        Assert.IsTrue(result);
    }

    [TestMethod]
    public async Task ExistsAsync_WhenUrlDoesNotExist_ReturnsFalse()
    {
        var mock = SharedData.SetupMockHttpMessageHandlerForSuccessfullResponse(
            new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
            }
        );

        var httpClient = new HttpClient(mock.Object);

        var result = await httpClient.ExistsAsync("http://example.com/404.txt");

        Assert.IsFalse(result);
    }

    [TestMethod]
    public async Task ExistsAsync_WithException_ReturnsFalse()
    {
        var mock = SharedData.SetupMockHttpMessageHandlerForException(new HttpRequestException());

        var httpClient = new HttpClient(mock.Object);

        var result = await httpClient.ExistsAsync("http://example.com/error.txt");

        Assert.IsFalse(result);
    }

    #endregion
}
