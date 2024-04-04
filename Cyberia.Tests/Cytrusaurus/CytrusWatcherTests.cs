using Cyberia.Cytrusaurus;
using Cyberia.Cytrusaurus.EventArgs;

using Moq;

using System.Net;

namespace Cyberia.Tests.Cytrusaurus;

[TestClass]
public sealed class CytrusWatcherTests
{
    [TestInitialize]
    public void Initialize()
    {
        CytrusWatcher.Initialize();
        CytrusWatcher.HttpRetryPolicy = SharedData.HttpRetryPolicy;
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

        CytrusWatcher.HttpClient = new HttpClient(mock.Object)
        {
            BaseAddress = new Uri(CytrusWatcher.BaseUrl)
        };

        Mock<EventHandler<NewCytrusDetectedEventArgs>> mockHandler = new();
        CytrusWatcher.NewCytrusDetected += mockHandler.Object;

        await CytrusWatcher.CheckAsync();

        Assert.AreEqual(6, CytrusWatcher.Cytrus.Version);
        Assert.AreEqual("production", CytrusWatcher.Cytrus.Name);
        mockHandler.Verify(x => x(It.IsAny<object>(), It.IsAny<NewCytrusDetectedEventArgs>()), Times.Once);
    }

    #endregion
}
