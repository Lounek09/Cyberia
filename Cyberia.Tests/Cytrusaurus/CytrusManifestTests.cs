using Cyberia.Cytrusaurus;
using Cyberia.Cytrusaurus.Models;
using Cyberia.Cytrusaurus.Models.FlatBuffers;

using Google.FlatBuffers;

using Moq;
using Moq.Protected;

using System.Net;

namespace Cyberia.Tests.Cytrusaurus;

[TestClass]
public sealed class CytrusManifestTests
{
    private const string CURRENT_MANIFEST = "current.manifest";
    private const string MODEL_MANIFEST = "model.manifest";

    private Mock<HttpMessageHandler> _mockHttpMessageHandler = default!;
    private Manifest _currentManifest = default!;
    private Manifest _modelManifest = default!;

    [TestInitialize]
    public void Initialize()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();

        var bytes = File.ReadAllBytes(Path.Join(SharedData.DATA_DIRECTORY, CURRENT_MANIFEST));
        ByteBuffer buffer = new(bytes);
        _currentManifest = Manifest.GetRootAsManifest(buffer);

        bytes = File.ReadAllBytes(Path.Join(SharedData.DATA_DIRECTORY, MODEL_MANIFEST));
        buffer = new(bytes);
        _modelManifest = Manifest.GetRootAsManifest(buffer);

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

    #region GetManifestAsync

    [TestMethod]
    public async Task GetManifestAsync_ReturnsManifest_WhenRequestIsSuccessful()
    {
        var game = "retro";
        var platform = CytrusGame.WINDOWS_PLATFORM;
        var release = CytrusGame.MAIN_RELEASE;
        var version = "6.0_1.42.1.3205.227-d31f250";
        var bytes = File.ReadAllBytes(Path.Join(SharedData.DATA_DIRECTORY, CURRENT_MANIFEST));

        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new ByteArrayContent(bytes)
            });

        CytrusWatcher.HttpClient = new(_mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri(CytrusWatcher.BASE_URL)
        };

        var result = await CytrusManifest.GetManifestAsync(game, platform, release, version);

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task GetManifestAsync_ReturnsNull_WhenRequestFails()
    {
        var game = "retro";
        var platform = CytrusGame.WINDOWS_PLATFORM;
        var release = CytrusGame.MAIN_RELEASE;
        var version = "6.0_1.42.1.3205.227-d31f250";

        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.InternalServerError
            });

        CytrusWatcher.HttpClient = new HttpClient(_mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri(CytrusWatcher.BASE_URL)
        };

        var result = await CytrusManifest.GetManifestAsync(game, platform, release, version);

        Assert.IsNull(result);
    }

    #endregion

    #region Diff

    [TestMethod]
    public void Diff_ReturnsCorrectDifferences()
    {
        var diff = CytrusManifest.Diff(_currentManifest, _modelManifest);

        var expected = """
            // CLASSIC \\
            - resources/app/retroclient/clips/items/15/492.swf
            + resources/app/retroclient/clips/items/69/492.swf

            // REMASTERED \\
            - resources/app/retroclient/clips/items/15/492.swf
            + resources/app/retroclient/clips/items/69/492.swf

            // WINDOWS \\
            ~ Dofus Retro.exe
            ~ resources/app/main.jsc (13006296 -> 13000992)
            ~ resources/app/retroclient/js/D1Chat.js (640764 -> 629735)
            ~ resources/app/retroclient/js/D1Console.js (435697 -> 420482)
            ~ resources/app/retroclient/js/D1ElectronLauncher.js (605254 -> 608267)

            """;

        Assert.AreEqual(expected, diff);
    }

    #endregion

    #region GetGameManifestRoute

    [TestMethod]
    public void GetGameManifestRoute_ReturnsCorrectRoute()
    {
        var game = "retro";
        var platform = CytrusGame.WINDOWS_PLATFORM;
        var release = CytrusGame.MAIN_RELEASE;
        var version = "6.0_1.42.1.3205.227-d31f250";
        var expectedRoute = $"{game}/releases/{release}/{platform}/{version}.manifest";

        var resultUrl = CytrusManifest.GetManifestRoute(game, platform, release, version);

        Assert.AreEqual(expectedRoute, resultUrl);
    }

    #endregion

    #region DiffFragment

    [TestMethod]
    public void DiffFragment_ReturnsCorrectDifferences()
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
        if (BitConverter.IsLittleEndian)
        {
            Console.WriteLine("System uses little-endian byte order.");
        }
        else
        {
            Console.WriteLine("System uses big-endian byte order.");
        }

        var fragment = _currentManifest.Fragments(0)!.Value;
        var gameFiles = fragment.GetGameFiles();

        Assert.AreEqual(fragment.FilesLength, gameFiles.Count());
    }

    #endregion
}
