using Cyberia.Cytrusaurus;
using Cyberia.Cytrusaurus.EventArgs;

using Moq;

using System.Net;

namespace Cyberia.Tests.Cytrusaurus;

[TestClass]
public sealed class CytrusWatcherTests
{
    private CytrusWatcher _cytrusWatcher = default!;

    [TestInitialize]
    public void Initialize()
    {
        _cytrusWatcher = new()
        {
            HttpRetryPolicy = SharedData.HttpRetryPolicy
        };
    }

    [TestCleanup]
    public void Cleanup()
    {
        if (Directory.Exists(CytrusWatcher.OutputPath))
        {
            Directory.Delete(CytrusWatcher.OutputPath, true);
        }
    }

    #region CheckAsync

    [TestMethod]
    public async Task CheckAsync_WhenCalled_ShouldUpdateCytrusData()
    {
        var json = File.ReadAllText(SharedData.CytrusJsonPath);
        var mock = SharedData.SetupMockHttpMessageHandlerForSuccessfullResponse(
            new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(json)
            });

        _cytrusWatcher.HttpClient = new HttpClient(mock.Object)
        {
            BaseAddress = new Uri(CytrusWatcher.BaseUrl)
        };

        Mock<CytrusWatcher.NewCytrusFileDetectedEventHandler> mockHandler = new();
        _cytrusWatcher.NewCytrusFileDetected += mockHandler.Object;

        await _cytrusWatcher.CheckAsync();

        Assert.AreEqual(6, _cytrusWatcher.Cytrus.Version);
        Assert.AreEqual("production", _cytrusWatcher.Cytrus.Name);
        mockHandler.Verify(x => x(It.IsAny<CytrusWatcher>(), It.IsAny<NewCytrusFileDetectedEventArgs>()), Times.Once);
    }

    #endregion
}
