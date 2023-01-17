global using Salamandra.Cytrus.Models;
global using Salamandra.Utils;

namespace Salamandra.Cytrus
{
    public sealed class AnkamaCytrus
    {
        public Logger Logger { get; private set; }
        public CytrusData CytrusData { get; set; }
        public CytrusData OldCytrusData { get; set; }

        internal HttpClient HttpClient { get; private set; }

        internal static AnkamaCytrus Instance {
            get => _instance is null ? throw new NullReferenceException("Build Cytrus before !") : _instance;
        }
        private static AnkamaCytrus? _instance;

        internal AnkamaCytrus()
        {
            Logger = new("cytrus");
            CytrusData = File.Exists(Constant.CYTRUS_PATH) ? Json.LoadFromFile<CytrusData>(Constant.CYTRUS_PATH) : new();
            OldCytrusData = File.Exists(Constant.OLD_CYTRUS_PATH) ? Json.LoadFromFile<CytrusData>(Constant.OLD_CYTRUS_PATH) : new();
            HttpClient = new()
            {
                BaseAddress = new Uri(Constant.BASE_ADRESS)
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


        /// <summary>
        /// Checks if cytrus has been updated.
        /// </summary>
        public async Task Launch()
        {
            CheckCytrusStarted?.Invoke(this, new());

            string? cytrus = null;
            try
            {
                using (HttpResponseMessage response = await HttpClient.GetAsync(Constant.CYTRUS_FILE_NAME).ConfigureAwait(false))
                {
                    response.EnsureSuccessStatusCode();
                    cytrus = await response.Content.ReadAsStringAsync();
                }
            }
            catch (HttpRequestException e)
            {
                Logger.Error(e);
                return;
            }

            if (cytrus is null)
                return;

            string lastCytrus = "{}";
            if (File.Exists(Constant.CYTRUS_PATH))
                lastCytrus = File.ReadAllText(Constant.CYTRUS_PATH);

            OldCytrusData = CytrusData;
            CytrusData = Json.Load<CytrusData>(cytrus);

            string diff = Json.Diff(cytrus, lastCytrus);
            if (string.IsNullOrEmpty(diff))
                return;

            Logger.Info($"Cytrus update detected :\n{diff}");
            NewCytrusDetected?.Invoke(this, new NewCytrusDetectedEventArgs(CytrusData, OldCytrusData, diff));

            if (File.Exists(Constant.CYTRUS_PATH))
                File.Move(Constant.CYTRUS_PATH, Constant.OLD_CYTRUS_PATH, true);
            File.WriteAllText(Constant.CYTRUS_PATH, cytrus);

            CheckCytrusFinished?.Invoke(this, new());
        }
    }

    public sealed class NewCytrusDetectedEventArgs : EventArgs
    {
        public CytrusData CytrusData { get; set; }
        public CytrusData OldCytrusData { get; set; }
        public string Diff { get; set; }

        public NewCytrusDetectedEventArgs(CytrusData cytrusData, CytrusData oldCytrusData, string diff)
        {
            CytrusData = cytrusData;
            OldCytrusData = oldCytrusData;
            Diff = diff;
        }
    }
}