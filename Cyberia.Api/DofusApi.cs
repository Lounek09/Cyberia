using Cyberia.Api.Data;

namespace Cyberia.Api;

public static class DofusApi
{
    public const string OutputPath = "api";

    public static readonly string CustomPath = Path.Join(OutputPath, "custom");

    public static ApiConfig Config { get; private set; } = default!;
    public static Datacenter Datacenter { get; private set; } = default!;

    internal static HttpClient HttpClient { get; private set; } = default!;

    public static async Task InitializeAsync(ApiConfig config)
    {
        Directory.CreateDirectory(OutputPath);
        Directory.CreateDirectory(CustomPath);

        Config = config;
        HttpClient = new();
        Datacenter = await Datacenter.LoadAsync();
    }

    public static async Task ReloadAsync()
    {
        Datacenter = await Datacenter.LoadAsync();
    }
}
