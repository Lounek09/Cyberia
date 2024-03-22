namespace Cyberia.Tests;

public static class SharedData
{
    public const string DATA_DIRECTORY = "tests";

    public static readonly string CYTRUS_JSON_PATH = Path.Join(DATA_DIRECTORY, "cytrus.json");
    public static readonly string CURRENT_MANIFEST_PATH = Path.Join(DATA_DIRECTORY, "current.manifest");
    public static readonly string MODEL_MANIFEST_PATH = Path.Join(DATA_DIRECTORY, "model.manifest");

    public static readonly HttpRetryPolicy HttpRetryPolicy = new(1, TimeSpan.FromMilliseconds(10));
}
