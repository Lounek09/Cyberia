using Moq;
using Moq.Protected;

namespace Cyberia.Tests;

public static class SharedData
{
    public const string OutputPath = "tests";

    //Cytrusaurus
    public static readonly string CytrusJsonPath = Path.Join(OutputPath, "cytrus.json");
    public static readonly string CurrentNanifestPath = Path.Join(OutputPath, "current.manifest");
    public static readonly string ModelManifestPath = Path.Join(OutputPath, "model.manifest");
    public static readonly string FragmentDiffPath = Path.Join(OutputPath, "fragment.diff");
    public static readonly string ManifestDiffPath = Path.Join(OutputPath, "manifest.diff");

    //Langzilla
    public static readonly string VersionsPath = Path.Join(OutputPath, "versions_fr.txt");
    public static readonly string LangRepositoryDataPath = Path.Join(OutputPath, "lang_repository_data.json");
    public static readonly string RanksSwfPath = Path.Join(OutputPath, "ranks_fr_1178.swf");
    public static readonly string RanksCurrentPath = Path.Join(OutputPath, "ranks_fr_1178_current.as");
    public static readonly string RanksOldPath = Path.Join(OutputPath, "ranks_fr_1178_old.as");
    public static readonly string RanksDiffPath = Path.Join(OutputPath, "ranks_fr_1178_diff.as");

    //Mock
    public static Mock<HttpMessageHandler> SetupMockHttpMessageHandlerForSuccessfullResponse(HttpResponseMessage message)
    {
        Mock<HttpMessageHandler> mock = new();

        mock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(message);

        return mock;
    }

    public static Mock<HttpMessageHandler> SetupMockHttpMessageHandlerForException(Exception exception)
    {
        Mock<HttpMessageHandler> mock = new();

        mock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(exception);

        return mock;
    }

    //Other
    public static readonly HttpRetryPolicy HttpRetryPolicy = new(1, TimeSpan.FromMilliseconds(10));
}
