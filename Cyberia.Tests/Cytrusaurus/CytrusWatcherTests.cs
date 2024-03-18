using Cyberia.Cytrusaurus;
using Cyberia.Cytrusaurus.EventArgs;

using Moq;
using Moq.Protected;

using System.Net;

namespace Cyberia.Tests.Cytrusaurus;

[TestClass]
public sealed class CytrusWatcherTests
{
    private const string CYTRUS_JSON = "cytrus.json";

    private Mock<HttpMessageHandler> _mockHttpMessageHandler = default!;

    [TestInitialize]
    public void Initialize()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();

        CytrusWatcher.Initialize();
        CytrusWatcher.HttpRetryPolicy = SharedData.HttpRetryPolicy;
    }

    [TestCleanup]
    public void Cleanup()
    {
        if (Directory.Exists(CytrusWatcher.OUTPUT_PATH))
        {
            Directory.Delete(CytrusWatcher.OUTPUT_PATH, true);
        }
    }

    #region CheckAsync

    [TestMethod]
    public async Task CheckAsync_WhenCalled_ShouldUpdateCytrusData()
    {
        var cytrus = File.ReadAllText(Path.Combine(SharedData.DATA_DIRECTORY, CYTRUS_JSON));

        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(cytrus)
            });

        CytrusWatcher.HttpClient = new HttpClient(_mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri(CytrusWatcher.BASE_URL)
        };

        var mockHandler = new Mock<EventHandler<NewCytrusDetectedEventArgs>>();
        CytrusWatcher.NewCytrusDetected += mockHandler.Object;

        await CytrusWatcher.CheckAsync();

        Assert.AreEqual(6, CytrusWatcher.CytrusData.Version);
        Assert.AreEqual("production", CytrusWatcher.CytrusData.Name);
        mockHandler.Verify(handler => handler(It.IsAny<object>(), It.IsAny<NewCytrusDetectedEventArgs>()), Times.Once);
    }

    #endregion
}
