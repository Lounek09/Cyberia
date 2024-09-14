using Cyberia.Cytrusaurus;
using Cyberia.Cytrusaurus.Models;

using System.Net;

namespace Cyberia.Tests.Cytrusaurus;

[TestClass]
public sealed class CytrusManifestFetcherTests
{
    private CytrusWatcher _cytrusWatcher = default!;
    private CytrusManifestFetcher _cytrusManifestFetcher = default!;

    [TestInitialize]
    public void Initialize()
    {
        _cytrusWatcher = new()
        {
            HttpRetryPolicy = SharedData.HttpRetryPolicy
        };
        _cytrusManifestFetcher = new(_cytrusWatcher);
    }

    [TestCleanup]
    public void Cleanup()
    {
        if (Directory.Exists(CytrusWatcher.OutputPath))
        {
            Directory.Delete(CytrusWatcher.OutputPath, true);
        }
    }

    #region GetAsync

    [TestMethod]
    public async Task GetManifestAsync_WhenRequestIsSuccessful_ReturnsManifest()
    {
        var game = "retro";
        var platform = CytrusGame.WindowsPlatform;
        var release = CytrusGame.MainRelease;
        var version = "6.0_1.42.1.3205.227-d31f250";
        var bytes = File.ReadAllBytes(SharedData.CurrentNanifestPath);

        var mock = SharedData.SetupMockHttpMessageHandlerForSuccessfullResponse(
            new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new ByteArrayContent(bytes)
            }
        );

        _cytrusWatcher.HttpClient = new(mock.Object)
        {
            BaseAddress = new Uri(CytrusWatcher.BaseUrl)
        };

        var result = await _cytrusManifestFetcher.GetAsync(game, platform, release, version);

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task GetManifestAsync_WhenRequestFails_ReturnsNull()
    {
        var game = "retro";
        var platform = CytrusGame.WindowsPlatform;
        var release = CytrusGame.MainRelease;
        var version = "6.0_1.42.1.3205.227-d31f250";

        var mock = SharedData.SetupMockHttpMessageHandlerForSuccessfullResponse(
            new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.InternalServerError
            }
        );

        _cytrusWatcher.HttpClient = new HttpClient(mock.Object)
        {
            BaseAddress = new Uri(CytrusWatcher.BaseUrl)
        };

        var result = await _cytrusManifestFetcher.GetAsync(game, platform, release, version);

        Assert.IsNull(result);
    }

    #endregion

    #region GetRoute

    [TestMethod]
    public void GetRoute_ReturnsCorrectRoute()
    {
        var game = "retro";
        var platform = CytrusGame.WindowsPlatform;
        var release = CytrusGame.MainRelease;
        var version = "6.0_1.42.1.3205.227-d31f250";
        var expectedRoute = $"{game}/releases/{release}/{platform}/{version}.manifest";

        var resultUrl = CytrusManifestFetcher.GetRoute(game, platform, release, version);

        Assert.AreEqual(expectedRoute, resultUrl);
    }

    #endregion    
}
