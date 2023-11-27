using Cyberia.Api.Data;

namespace Cyberia.Api;

public static class DofusApi
{
    internal const string OUTPUT_PATH = "api";
    internal const string CUSTOM_PATH = $"{OUTPUT_PATH}/custom";

    public static ApiConfig Config { get; private set; } = default!;
    public static Datacenter Datacenter { get; private set; } = default!;

    internal static HttpClient HttpClient { get; private set; } = default!;

    public static void Initialize(ApiConfig config)
    {
        Directory.CreateDirectory(OUTPUT_PATH);
        Directory.CreateDirectory(CUSTOM_PATH);

        Config = config;
        HttpClient = new();
        Datacenter = new();
    }

    public static void Reload()
    {
        Datacenter = new();
    }
}
