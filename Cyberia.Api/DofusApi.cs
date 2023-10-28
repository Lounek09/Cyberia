using Cyberia.Api.Data;
using Cyberia.Langzilla;

namespace Cyberia.Api
{
    public static class DofusApi
    {
        internal const string OUTPUT_PATH = "api";
        internal const string CUSTOM_PATH = $"{OUTPUT_PATH}/custom";

        public static ApiConfig Config { get; private set; } = default!;
        public static Datacenter Datacenter { get; private set; } = default!;

        internal static LangsWatcher LangsWatcher { get; private set; } = default!;
        internal static HttpClient HttpClient { get; private set; } = default!;

        public static void Initialize(ApiConfig config, LangsWatcher langsWatcher)
        {
            Directory.CreateDirectory(OUTPUT_PATH);
            Directory.CreateDirectory(CUSTOM_PATH);

            Config = config;
            LangsWatcher = langsWatcher;
            HttpClient = new();
            Datacenter = new();
        }

        public static void Reload()
        {
            Datacenter = new();
        }
    }
}
