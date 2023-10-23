using Cyberia.Cytrusaurus.Models;

namespace Cyberia.Cytrusaurus
{
    public sealed class CytrusWatcher
    {
        internal const string OUTPUT_PATH = "cytrus";
        internal const string CYTRUS_FILE_NAME = "cytrus.json";
        internal const string CYTRUS_PATH = $"{OUTPUT_PATH}/{CYTRUS_FILE_NAME}";
        internal const string OLD_CYTRUS_PATH = $"{OUTPUT_PATH}/old_{CYTRUS_FILE_NAME}";
        internal const string BASE_URL = "https://cytrus.cdn.ankama.com";

        public CytrusData CytrusData { get; internal set; }
        public CytrusData OldCytrusData { get; internal set; }

        internal HttpClient HttpClient { get; init; }
        internal HttpRetryPolicy HttpRetryPolicy { get; init; }

        internal static CytrusWatcher Instance
        {
            get => _instance is null ? throw new NullReferenceException("Build Cytrus before !") : _instance;
        }
        private static CytrusWatcher? _instance;

#pragma warning disable IDE0052 // Remove unread private members
        private Timer? _timer;
#pragma warning restore IDE0052 // Remove unread private members

        internal CytrusWatcher()
        {
            Directory.CreateDirectory(OUTPUT_PATH);

            CytrusData = File.Exists(CYTRUS_PATH) ? Json.LoadFromFile<CytrusData>(CYTRUS_PATH) : new();
            OldCytrusData = File.Exists(OLD_CYTRUS_PATH) ? Json.LoadFromFile<CytrusData>(OLD_CYTRUS_PATH) : new();
            HttpClient = new()
            {
                BaseAddress = new Uri(BASE_URL)
            };
            HttpRetryPolicy = new(5, TimeSpan.FromSeconds(1));
        }

        public static CytrusWatcher Create()
        {
            _instance ??= new();
            return _instance;
        }

        public event EventHandler<NewCytrusDetectedEventArgs>? NewCytrusDetected;

        public void Listen(TimeSpan dueTime, TimeSpan interval)
        {
            _timer = new(async _ => await LaunchAsync(), null, dueTime, interval);
        }

        public async Task LaunchAsync()
        {
            string? cytrus = null;
            try
            {
                using HttpResponseMessage response = await HttpRetryPolicy.ExecuteAsync(() => HttpClient.GetAsync(CYTRUS_FILE_NAME));
                response.EnsureSuccessStatusCode();

                cytrus = await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException e)
            {
                Log.Error(e, "An error occured while sending Get request to {url}}", $"{BASE_URL}/{CYTRUS_FILE_NAME}");
                return;
            }

            if (cytrus is null)
            {
                return;
            }

            string lastCytrus = "{}";
            if (File.Exists(CYTRUS_PATH))
            {
                lastCytrus = File.ReadAllText(CYTRUS_PATH);
            }

            OldCytrusData = CytrusData;
            CytrusData = Json.Load<CytrusData>(cytrus);

            string diff = Json.Diff(cytrus, lastCytrus);
            if (string.IsNullOrEmpty(diff))
            {
                return;
            }

            Log.Information("Cytrus update detected :\n{diff}", diff);
            NewCytrusDetected?.Invoke(this, new NewCytrusDetectedEventArgs(CytrusData, OldCytrusData, diff));

            if (File.Exists(CYTRUS_PATH))
            {
                File.Move(CYTRUS_PATH, OLD_CYTRUS_PATH, true);
            }
            File.WriteAllText(CYTRUS_PATH, cytrus);
        }
    }

    public sealed class NewCytrusDetectedEventArgs : EventArgs
    {
        public CytrusData CytrusData { get; init; }
        public CytrusData OldCytrusData { get; init; }
        public string Diff { get; init; }

        public NewCytrusDetectedEventArgs(CytrusData cytrusData, CytrusData oldCytrusData, string diff)
        {
            CytrusData = cytrusData;
            OldCytrusData = oldCytrusData;
            Diff = diff;
        }
    }
}
