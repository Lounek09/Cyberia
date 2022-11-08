global using Salamandra.Cytrus.Models;
global using Salamandra.Utils;

namespace Salamandra.Cytrus
{
    public class AnkamaCytrus
    {
        public CytrusData CytrusData { get; set; }

        internal Logger Logger { get; private set; }
        internal HttpClient HttpClient { get; private set; }

        internal static AnkamaCytrus Instance {
            get => _instance is null ? throw new NullReferenceException("Build Cytrus before !") : _instance;
            private set => _instance = value;
        }
        private static AnkamaCytrus? _instance;

        internal AnkamaCytrus(Logger logger)
        {
            Logger = logger;
            CytrusData = File.Exists(Constant.CYTRUS_PATH) ? Json.LoadFromFile<CytrusData>(Constant.CYTRUS_PATH) : new();
            HttpClient = new()
            {
                BaseAddress = new Uri(Constant.BASE_ADRESS)
            };
        }

        public static AnkamaCytrus Build(Logger logger)
        {
            _instance = new(logger);
            return _instance;
        }

        public event EventHandler? CheckCytrusStarted;
        public event EventHandler<NewCytrusDetectedEventArgs>? NewCytrusDetected;
        public event EventHandler? CheckCytrusFinished;

        public async Task Launch()
        {
            CheckCytrusStarted?.Invoke(this, new());

            if (Directory.Exists(Constant.CYTRUS_PATH))
                Directory.CreateDirectory(Constant.CYTRUS_PATH);

            string cytrus = "";
            try
            {
                using (HttpResponseMessage response = await HttpClient.GetAsync(Constant.CYTRUS_FILE_NAME))
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

            if (string.IsNullOrEmpty(cytrus))
                return;

            string lastCytrus = "{}";
            if (File.Exists(Constant.CYTRUS_PATH))
                lastCytrus = File.ReadAllText(Constant.CYTRUS_PATH);

            string diff = Json.Diff(cytrus, lastCytrus);

            if (string.IsNullOrEmpty(diff))
                return;

            Logger.Info($"Cytrus update detected :\n{diff}");
            CytrusData = Json.Load<CytrusData>(cytrus);
            NewCytrusDetected?.Invoke(this, new NewCytrusDetectedEventArgs(CytrusData, diff));

            if (File.Exists(Constant.CYTRUS_PATH))
                File.Move(Constant.CYTRUS_PATH, $"{Constant.OUTPUT_PATH}/cytrus_{DateTime.Now:MMddyyyyHHmm}.json");
            File.WriteAllText(Constant.CYTRUS_PATH, cytrus);

            CheckCytrusFinished?.Invoke(this, new());
        }
    }

    public sealed class NewCytrusDetectedEventArgs : EventArgs
    {
        public CytrusData CytrusData { get; set; }
        public string Diff { get; set; }

        public NewCytrusDetectedEventArgs(CytrusData cytrusData, string diff)
        {
            CytrusData = cytrusData;
            Diff = diff;
        }
    }
}