using Moq;
using Moq.Protected;

namespace Cyberia.Tests;

public static class SharedData
{
    public const string OUTPUT_PATH = "tests";

    //Cytrusaurus
    public static readonly string CYTRUS_JSON_PATH = Path.Join(OUTPUT_PATH, "cytrus.json");
    public static readonly string CURRENT_MANIFEST_PATH = Path.Join(OUTPUT_PATH, "current.manifest");
    public static readonly string MODEL_MANIFEST_PATH = Path.Join(OUTPUT_PATH, "model.manifest");
    public static readonly string MANIFEST_DIFF_PATH = Path.Join(OUTPUT_PATH, "manifest.diff");

    //Langzilla
    public static readonly string VERSIONS_PATH = Path.Join(OUTPUT_PATH, "versions_fr.txt");
    public static readonly string LANG_REPOSITORY_DATA_PATH = Path.Join(OUTPUT_PATH, "lang_repository_data.json");
    public static readonly string RANKS_SWF_PATH = Path.Join(OUTPUT_PATH, "ranks_fr_1178.swf");
    public static readonly string RANKS_CURRENT_PATH = Path.Join(OUTPUT_PATH, "ranks_fr_1178_current.as");
    public static readonly string RANKS_OLD_PATH = Path.Join(OUTPUT_PATH, "ranks_fr_1178_old.as");
    public static readonly string RANKS_DIFF_PATH = Path.Join(OUTPUT_PATH, "ranks_fr_1178_diff.as");

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
