using Cyberia.Api.Data;
using Cyberia.Langzilla;

namespace Cyberia.Api
{
    public sealed class DofusApi
    {
        internal const string OUTPUT_PATH = "api";
        internal const string CUSTOM_PATH = $"{OUTPUT_PATH}/custom";

        public ApiConfig Config { get; init; }
        public Datacenter Datacenter { get; internal set; }

        internal LangsWatcher LangsWatcher { get; init; }
        internal HttpClient HttpClient { get; init; }

        internal static DofusApi Instance
        {
            get => _instance is null ? throw new NullReferenceException("Build the Api before !") : _instance;
            private set => _instance = value;
        }
        private static DofusApi? _instance;

        internal DofusApi(ApiConfig config, LangsWatcher langsWatcher)
        {
            Directory.CreateDirectory(OUTPUT_PATH);
            Directory.CreateDirectory(CUSTOM_PATH);

            Config = config;
            LangsWatcher = langsWatcher;
            HttpClient = new();
            Datacenter = new();
        }

        public static DofusApi Build(ApiConfig config, LangsWatcher langsWatcher)
        {
            _instance ??= new(config, langsWatcher);
            return _instance;
        }

        public void Reload()
        {
            Datacenter = new();
        }
    }
}
