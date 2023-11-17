using Cyberia.Cytrusaurus.Models;

namespace Cyberia.Cytrusaurus
{
    public static class CytrusWatcher
    {
        internal const string OUTPUT_PATH = "cytrus";
        internal const string CYTRUS_FILE_NAME = "cytrus.json";
        internal const string CYTRUS_PATH = $"{OUTPUT_PATH}/{CYTRUS_FILE_NAME}";
        internal const string OLD_CYTRUS_PATH = $"{OUTPUT_PATH}/old_{CYTRUS_FILE_NAME}";
        internal const string BASE_URL = "https://cytrus.cdn.ankama.com";

        public static CytrusData Data { get; internal set; } = default!;
        public static CytrusData OldData { get; internal set; } = default!;

        internal static HttpClient HttpClient { get; private set; } = default!;
        internal static HttpRetryPolicy HttpRetryPolicy { get; private set; } = default!;


#pragma warning disable IDE0052 // Remove unread private members
        private static Timer? _timer;
#pragma warning restore IDE0052 // Remove unread private members

        public static void Initialize()
        {
            Directory.CreateDirectory(OUTPUT_PATH);

            Data = CytrusData.LoadFromFile(CYTRUS_PATH);
            OldData = CytrusData.LoadFromFile(OLD_CYTRUS_PATH);
            HttpClient = new()
            {
                BaseAddress = new Uri(BASE_URL)
            };
            HttpRetryPolicy = new(5, TimeSpan.FromSeconds(1));
        }

        public static event EventHandler<NewCytrusDetectedEventArgs>? NewCytrusDetected;

        public static void Watch(TimeSpan dueTime, TimeSpan interval)
        {
            _timer = new(async _ => await CheckAsync(), null, dueTime, interval);
        }

        public static async Task CheckAsync()
        {
            string cytrus = "{}";
            try
            {
                using HttpResponseMessage response = await HttpRetryPolicy.ExecuteAsync(() => HttpClient.GetAsync(CYTRUS_FILE_NAME));
                response.EnsureSuccessStatusCode();

                cytrus = await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException e)
            {
                Log.Error(e, "An error occurred while sending Get request to {CytrusUrl}", $"{BASE_URL}/{CYTRUS_FILE_NAME}");
                return;
            }

            string oldCytrus = File.Exists(CYTRUS_PATH) ? File.ReadAllText(CYTRUS_PATH) : "{}";

            OldData = Data;
            Data = CytrusData.Load(cytrus);

            string diff = Json.Diff(cytrus, oldCytrus);
            if (string.IsNullOrEmpty(diff))
            {
                return;
            }

            Log.Information("Cytrus update detected :\n{CytrusDiff}", diff);
            NewCytrusDetected?.Invoke(null, new NewCytrusDetectedEventArgs(Data, OldData, diff));

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
