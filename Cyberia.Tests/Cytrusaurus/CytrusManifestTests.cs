using Cyberia.Cytrusaurus;
using Cyberia.Cytrusaurus.Models;
using Cyberia.Cytrusaurus.Models.FlatBuffers;

using Google.FlatBuffers;

using System.Net;

namespace Cyberia.Tests.Cytrusaurus;

[TestClass]
public sealed class CytrusManifestTests
{
    private Manifest _currentManifest = default!;
    private Manifest _modelManifest = default!;

    [TestInitialize]
    public void Initialize()
    {
        var bytes = File.ReadAllBytes(SharedData.CurrentNanifestPath);
        ByteBuffer buffer = new(bytes);
        _currentManifest = Manifest.GetRootAsManifest(buffer);

        bytes = File.ReadAllBytes(SharedData.ModelManifestPath);
        buffer = new(bytes);
        _modelManifest = Manifest.GetRootAsManifest(buffer);

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

    #region GetManifestAsync

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

        CytrusWatcher.HttpClient = new(mock.Object)
        {
            BaseAddress = new Uri(CytrusWatcher.BaseUrl)
        };

        var result = await CytrusManifest.GetManifestAsync(game, platform, release, version);

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

        CytrusWatcher.HttpClient = new HttpClient(mock.Object)
        {
            BaseAddress = new Uri(CytrusWatcher.BaseUrl)
        };

        var result = await CytrusManifest.GetManifestAsync(game, platform, release, version);

        Assert.IsNull(result);
    }

    #endregion

    #region Diff

    [TestMethod]
    public void Diff_WhithDifferentManifest_ReturnsCorrectDifferences()
    {
        var result = CytrusManifest.Diff(_currentManifest, _modelManifest);

        var expected = File.ReadAllText(SharedData.ManifestDiffPath);

        Assert.AreEqual(expected, result);
    }

    #endregion

    #region GetGameManifestRoute

    [TestMethod]
    public void GetGameManifestRoute_ReturnsCorrectRoute()
    {
        var game = "retro";
        var platform = CytrusGame.WindowsPlatform;
        var release = CytrusGame.MainRelease;
        var version = "6.0_1.42.1.3205.227-d31f250";
        var expectedRoute = $"{game}/releases/{release}/{platform}/{version}.manifest";

        var resultUrl = CytrusManifest.GetManifestRoute(game, platform, release, version);

        Assert.AreEqual(expectedRoute, resultUrl);
    }

    #endregion

    #region DiffFragment

    [TestMethod]
    public void DiffFragment_WithDifferentFragment_ReturnsCorrectDifferences()
    {
        var currentFragment = _currentManifest.Fragments(0)!.Value;
        var modelFragment = _modelManifest.Fragments(0)!.Value;

        var diff = CytrusManifest.DiffFragment(currentFragment, modelFragment);

        var expected = """
            // CLASSIC \\
            - resources/app/retroclient/clips/items/15/492.swf
            + resources/app/retroclient/clips/items/69/492.swf
            """;

        Assert.AreEqual(expected, diff);
    }

    #endregion

    #region GetFragments

    [TestMethod]
    public void GetFragments_ReturnsCorrectNumberOfFragments()
    {
        var fragments = _currentManifest.GetFragments();

        Assert.AreEqual(_currentManifest.FragmentsLength, fragments.Count());
    }

    #endregion

    #region GetGameFiles

    [TestMethod]
    public void GetGameFiles_ReturnsCorrectNumberOfGameFiles()
    {
        var fragment = _currentManifest.Fragments(0)!.Value;
        var gameFiles = fragment.GetGameFiles();

        Assert.AreEqual(fragment.FilesLength, gameFiles.Count());
    }

    #endregion
}
