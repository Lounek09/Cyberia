using Cyberia.Langzilla.Enums;

using Serilog;

using System.Collections.Concurrent;

namespace Cyberia.Langzilla
{
    public sealed class LangsWatcher
    {
        public const string OUTPUT_PATH = "langs";
        public const string BASE_URL = "https://dofusretro.cdn.ankama.com";

        public ILogger Log { get; init; }
        public LangsType Official { get; init; }
        public LangsType Beta { get; init; }
        public LangsType Temporis { get; init; }

        internal HttpClient HttpClient { get; init; }

        internal static LangsWatcher Instance
        {
            get => _instance is null ? throw new NullReferenceException("Build the Langs before !") : _instance;
        }
        private static LangsWatcher? _instance;

        private readonly ConcurrentDictionary<string, Timer> _timers;

        internal LangsWatcher(ILogger logger)
        {
            Log = logger;
            Official = new(LangType.Official);
            Beta = new(LangType.Beta);
            Temporis = new(LangType.Temporis);
            HttpClient = new();
            _timers = new();

            Directory.CreateDirectory(OUTPUT_PATH);
        }

        public static LangsWatcher Create(ILogger logger)
        {
            _instance ??= new(logger);
            return _instance;
        }

        public event EventHandler<CheckLangStartedEventArgs>? CheckLangStarted;
        public event EventHandler<CheckLangFinishedEventArgs>? CheckLangFinished;

        public void Watch(LangType type, Language language, TimeSpan dueTime, TimeSpan interval)
        {
            Timer timer = new(async _ => await LaunchAsync(type, language), null, dueTime, interval);

            _timers.AddOrUpdate($"{type}_{language}", timer, (key, oldValue) => oldValue = timer);
        }

        public void WatchAll(LangType type, TimeSpan dueTime, TimeSpan interval)
        {
            foreach (Language language in Enum.GetValues<Language>())
            {
                Timer timer = new(async _ => await LaunchAsync(type, language), null, dueTime, interval);

                _timers.AddOrUpdate($"{type}_{language}", timer, (key, oldValue) => oldValue = timer);
            }
        }

        public LangsType GetLangsByType(LangType type)
        {
            return type switch
            {
                LangType.Official => Official,
                LangType.Beta => Beta,
                LangType.Temporis => Temporis,
                _ => throw new NotImplementedException()
            };
        }

        public async Task LaunchAsync(LangType type, Language language, bool force = false)
        {
            CheckLangStarted?.Invoke(this, new CheckLangStartedEventArgs(type, language));

            LangsData data = GetLangsByType(type).GetLangsByLanguage(language);
            List<Lang> updatedLangs = await data.FetchLangsAsync(force);

            CheckLangFinished?.Invoke(this, new CheckLangFinishedEventArgs(type, language, updatedLangs));
        }

        internal static string GetOutputDirectoryPath(LangType type, Language language)
        {
            return Path.Join(OUTPUT_PATH, type.ToString().ToLower(), language.ToString().ToLower());
        }

        internal static string GetRoute(LangType type)
        {
            return type switch
            {
                LangType.Beta => "betaenv/lang",
                LangType.Temporis => "ephemeris2releasebucket/lang",
                _ => "lang",
            };
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
