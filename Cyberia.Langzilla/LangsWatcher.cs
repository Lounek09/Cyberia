using Cyberia.Langzilla.Enums;

using System.Collections.Concurrent;

namespace Cyberia.Langzilla
{
    public static class LangsWatcher
    {
        public const string OUTPUT_PATH = "langs";
        public const string BASE_URL = "https://dofusretro.cdn.ankama.com";

        public static LangsType Official { get; private set; } = default!;
        public static LangsType Beta { get; private set; } = default!;
        public static LangsType Temporis { get; private set; } = default!;

        internal static HttpClient HttpClient { get; private set; } = default!;
        internal static HttpRetryPolicy HttpRetryPolicy { get; private set; } = default!;

        private static readonly ConcurrentDictionary<string, Timer> _timers = new();

        public static void Initialize()
        {
            Directory.CreateDirectory(OUTPUT_PATH);

            Official = new(LangType.Official);
            Beta = new(LangType.Beta);
            Temporis = new(LangType.Temporis);
            HttpClient = new();
            HttpRetryPolicy = new(5, TimeSpan.FromSeconds(1));
        }

        public static event EventHandler<CheckLangStartedEventArgs>? CheckLangStarted;
        public static event EventHandler<CheckLangFinishedEventArgs>? CheckLangFinished;

        public static void Watch(LangType type, TimeSpan dueTime, TimeSpan interval)
        {
            foreach (Language language in Enum.GetValues<Language>())
            {
                Timer timer = new(async _ => await CheckAsync(type, language), null, dueTime, interval);

                _timers.AddOrUpdate($"{type}_{language}", timer, (key, oldValue) => oldValue = timer);
            }
        }

        public static LangsType GetLangsByType(LangType type)
        {
            return type switch
            {
                LangType.Official => Official,
                LangType.Beta => Beta,
                LangType.Temporis => Temporis,
                _ => throw new NotImplementedException()
            };
        }

        public static async Task CheckAsync(LangType type, Language language, bool force = false)
        {
            CheckLangStarted?.Invoke(null, new CheckLangStartedEventArgs(type, language));

            LangsData data = GetLangsByType(type).GetLangsByLanguage(language);
            List<Lang> updatedLangs = await data.FetchLangsAsync(force);

            CheckLangFinished?.Invoke(null, new CheckLangFinishedEventArgs(type, language, updatedLangs));
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
