using Cyberia.Api.Data;
using Cyberia.Api.Managers;
using Cyberia.Langzilla.Enums;

namespace Cyberia.Api;

public static class DofusApi
{
    public const string OutputPath = "api";

    public static readonly string CustomPath = Path.Join(OutputPath, "custom");

    public static ApiConfig Config { get; private set; } = default!;
    public static Datacenter Datacenter { get; private set; } = default!;

    internal static HttpClient HttpClient { get; private set; } = default!;

    public static void Initialize(ApiConfig config)
    {
        Directory.CreateDirectory(CustomPath);

        Config = config;
        HttpClient = new();
        Datacenter = new Datacenter(config.Type);
    }

    public static void Reload(LangType type)
    {
        Datacenter = new Datacenter(type);
        CdnManager.ClearCache();
    }
}
