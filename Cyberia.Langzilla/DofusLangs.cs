global using Cyberia.Utils;
using Cyberia.Chronicle;
using Cyberia.Langzilla.Enums;

using System.Collections.Concurrent;

namespace Cyberia.Langzilla
{
    public sealed class DofusLangs
    {
        public const string OUTPUT_PATH = "langs";
        public const string BASE_URL = "https://dofusretro.cdn.ankama.com";

        public Logger Logger { get; init; }

        internal List<LangsData> Data { get; init; }
        internal HttpClient HttpClient { get; init; }

        internal static DofusLangs Instance {
            get => _instance is null ? throw new NullReferenceException("Build the Langs before !") : _instance;
        }
        private static DofusLangs? _instance;

        private readonly ConcurrentDictionary<string, Timer> _timers;

        internal DofusLangs()
        {
            Logger = new("langs");
            Data = LoadData();
            HttpClient = new();
            _timers = new();

            Directory.CreateDirectory(OUTPUT_PATH);
        }

        internal static List<LangsData> LoadData()
        {
            List<LangsData> data = new();
            foreach (LangType type in Enum.GetValues<LangType>())
            {
                foreach (Language language in Enum.GetValues<Language>())
                {
                    if (File.Exists($"{GetDirectoryPath(type, language)}/data.json"))
                        data.Add(Json.LoadFromFile<LangsData>($"{GetDirectoryPath(type, language)}/data.json"));
                }
            }

            return data;
        }

        public static DofusLangs Build()
        {
            _instance ??= new();
            return _instance;
        }

        public event EventHandler<CheckLangStartedEventArgs>? CheckLangStarted;
        public event EventHandler<LangUpdatedEventArgs>? LangUpdated;
        public event EventHandler<CheckLangFinishedEventArgs>? CheckLangFinished;

        public void Listen(LangType type, Language language, int dueTime, int interval)
        {
            Timer timer = new(async _ => await LaunchAsync(type, language), null, dueTime, interval);

            _timers.AddOrUpdate($"{type}_{language}", timer, (key, oldValue) => oldValue = timer);
        }

        public void ListenForAllLanguage(LangType type, int dueTime, int interval)
        {
            foreach (Language language in Enum.GetValues<Language>())
            {
                Timer timer = new(async _ => await LaunchAsync(type, language), null, dueTime, interval);

                _timers.AddOrUpdate($"{type}_{language}", timer, (key, oldValue) => oldValue = timer);
            }
        }

        public async Task LaunchAsync(LangType type, Language language, bool force = false)
        {
            CheckLangStarted?.Invoke(this, new CheckLangStartedEventArgs(type, language));

            LangsData data = GetLangsData(type, language);

            List<Lang> updatedLangs = new();
            foreach (string langFileName in await data.GetLangsInfosFormServerAsync(force))
            {
                string[] args = langFileName.Split(',');

                Lang lang = new(args[0], int.Parse(args[2]), type, language);
                if (!File.Exists(lang.GetFilePath()))
                {
                    await lang.DownloadAsync();
                    lang.Extract();
                    lang.Diff();

                    updatedLangs.Add(lang);

                    LangUpdated?.Invoke(this, new(type, language, lang));
                }
            }

            if (updatedLangs.Count > 0)
            {
                data.UpdateLangs(updatedLangs);
                data.Save();
            }

            CheckLangFinished?.Invoke(this, new CheckLangFinishedEventArgs(type, language, updatedLangs));
        }

        public LangsData GetLangsData(LangType type, Language language)
        {
            LangsData? data = Data.Find(x => x.Type == type && x.Language == language);

            if (data is null)
            {
                data = new(type, language);
                Data.Add(data);
            }

            return data;
        }

        internal static string GetDirectoryPath(LangType type, Language language)
        {
            return $"{OUTPUT_PATH}/{type.ToString().ToLower()}/{language.ToString().ToLower()}";
        }

        internal static string GetRoute(LangType type)
        {
            switch (type)
            {
                case LangType.Beta:
                    return "betaenv/lang";
                case LangType.Temporis:
                    return "temporis/lang";
                default:
                    return "lang";
            }
        }
    }

    public sealed class CheckLangStartedEventArgs : EventArgs
    {
        public LangType Type { get; init; }
        public Language Language { get; init; }

        public CheckLangStartedEventArgs(LangType type, Language language)
        {
            Type = type;
            Language = language;
        }
    }

    public sealed class LangUpdatedEventArgs : EventArgs
    {
        public LangType Type { get; init; }
        public Language Language { get; init; }
        public Lang UpdatedLang { get; init; }

        public LangUpdatedEventArgs(LangType type, Language language, Lang updatedLang)
        {
            Type = type;
            Language = language;
            UpdatedLang = updatedLang;
        }
    }

    public sealed class CheckLangFinishedEventArgs : EventArgs
    {
        public LangType Type { get; init; }
        public Language Language { get; init; }
        public List<Lang> UpdatedLangs { get; init; }

        public CheckLangFinishedEventArgs(LangType type, Language language, List<Lang> updatedLangs)
        {
            Type = type;
            Language = language;
            UpdatedLangs = updatedLangs;
        }
    }
}
