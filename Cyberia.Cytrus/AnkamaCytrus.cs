﻿global using Cyberia.Utils;
using Cyberia.Chronicle;
using Cyberia.Cytrus.Models;

namespace Cyberia.Cytrus
{
    public sealed class AnkamaCytrus
    {
        internal const string OUTPUT_PATH = "cytrus";
        internal const string CYTRUS_FILE_NAME = "cytrus.json";
        internal const string CYTRUS_PATH = $"{OUTPUT_PATH}/{CYTRUS_FILE_NAME}";
        internal const string OLD_CYTRUS_PATH = $"{OUTPUT_PATH}/old_{CYTRUS_FILE_NAME}";
        internal const string BASE_URL = "https://cytrus.cdn.ankama.com";

        public Logger Logger { get; init; }
        public CytrusData CytrusData { get; internal set; }
        public CytrusData OldCytrusData { get; internal set; }

        internal HttpClient HttpClient { get; init; }

        internal static AnkamaCytrus Instance {
            get => _instance is null ? throw new NullReferenceException("Build Cytrus before !") : _instance;
        }
        private static AnkamaCytrus? _instance;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "<Pending>")]
        private Timer? _timer;

        internal AnkamaCytrus()
        {
            if (!Directory.Exists(OUTPUT_PATH))
                Directory.CreateDirectory(OUTPUT_PATH);

            Logger = new("cytrus");
            CytrusData = File.Exists(CYTRUS_PATH) ? Json.LoadFromFile<CytrusData>(CYTRUS_PATH) : new();
            OldCytrusData = File.Exists(OLD_CYTRUS_PATH) ? Json.LoadFromFile<CytrusData>(OLD_CYTRUS_PATH) : new();
            HttpClient = new()
            {
                BaseAddress = new Uri(BASE_URL)
            };
        }

        public static AnkamaCytrus Build()
        {
            _instance ??= new();
            return _instance;
        }

        public event EventHandler? CheckCytrusStarted;
        public event EventHandler<NewCytrusDetectedEventArgs>? NewCytrusDetected;
        public event EventHandler? CheckCytrusFinished;

        public void Listen(int dueTime, int interval)
        {
            _timer = new(async _ => await LaunchAsync(), null, dueTime, interval);
        }

        /// <summary>
        /// Checks if cytrus has been updated.
        /// </summary>
        public async Task LaunchAsync()
        {
            CheckCytrusStarted?.Invoke(this, new());

            string? cytrus = null;
            try
            {
                using (HttpResponseMessage response = await HttpClient.GetAsync(CYTRUS_FILE_NAME).ConfigureAwait(false))
                {
                    response.EnsureSuccessStatusCode();
                    cytrus = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception e) when (e is HttpRequestException or TaskCanceledException)
            {
                Logger.Error(e);
                return;
            }

            if (cytrus is null)
                return;

            string lastCytrus = "{}";
            if (File.Exists(CYTRUS_PATH))
                lastCytrus = File.ReadAllText(CYTRUS_PATH);

            OldCytrusData = CytrusData;
            CytrusData = Json.Load<CytrusData>(cytrus);

            string diff = Json.Diff(cytrus, lastCytrus);
            if (string.IsNullOrEmpty(diff))
                return;

            Logger.Info($"Cytrus update detected :\n{diff}");
            NewCytrusDetected?.Invoke(this, new NewCytrusDetectedEventArgs(CytrusData, OldCytrusData, diff));

            if (File.Exists(CYTRUS_PATH))
                File.Move(CYTRUS_PATH, OLD_CYTRUS_PATH, true);
            File.WriteAllText(CYTRUS_PATH, cytrus);

            CheckCytrusFinished?.Invoke(this, new());
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