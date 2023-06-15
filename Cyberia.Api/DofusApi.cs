global using Cyberia.Utils;
using Cyberia.Api.DatacenterNS;
using Cyberia.Chronicle;
using Cyberia.Langzilla;

namespace Cyberia.Api
{
    public sealed class DofusApi
    {
        internal const string OUTPUT_PATH = "api";
        internal const string CUSTOM_PATH = $"{OUTPUT_PATH}/custom";

        public Logger Logger { get; init; }
        public string CdnUrl { get; init; }
        public bool Temporis { get; init; }

        public Datacenter Datacenter { get; internal set; }

        internal LangsWatcher LangsWatcher { get; init; }
        internal FormatType FormatType { get; init; }
        internal HttpClient HttpClient { get; init; }

        internal static DofusApi Instance {
            get => _instance is null ? throw new NullReferenceException("Build the Api before !") : _instance;
            private set => _instance = value;
        }
        private static DofusApi? _instance;

        internal DofusApi(string cdnUrl, bool temporis, LangsWatcher langsWatcher, FormatType formatType)
        {
            Directory.CreateDirectory(OUTPUT_PATH);
            Directory.CreateDirectory(CUSTOM_PATH);

            Logger = new("api");
            CdnUrl = cdnUrl;
            Temporis = temporis;
            LangsWatcher = langsWatcher;
            FormatType = formatType;
            HttpClient = new();
            Datacenter = new();
        }

        public static DofusApi Build(string cdnUrl, bool temporis, LangsWatcher langsWatcher, FormatType formatType)
        {
            _instance ??= new(cdnUrl, temporis, langsWatcher, formatType);
            return _instance;
        }

        public void Reload()
        {
            Datacenter = new();
        }
    }
}
