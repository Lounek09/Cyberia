using Cyberia.Langzilla.Enums;
using Cyberia.Langzilla.EventArgs;

using System.Collections.Concurrent;

namespace Cyberia.Langzilla;

public static class LangsWatcher
{
    public const string OUTPUT_PATH = "langs";
    public const string BASE_URL = "https://dofusretro.cdn.ankama.com";

    public static IReadOnlyDictionary<(LangType, LangLanguage), LangDataCollection> Langs => _langs.AsReadOnly();

    internal static HttpClient HttpClient { get; private set; } = default!;
    internal static HttpRetryPolicy HttpRetryPolicy { get; private set; } = default!;

    private static readonly Dictionary<(LangType, LangLanguage), LangDataCollection> _langs = [];
    private static readonly ConcurrentDictionary<(LangType, LangLanguage), Timer> _timers = [];

    public static void Initialize()
    {
        Directory.CreateDirectory(OUTPUT_PATH);

        foreach (var type in Enum.GetValues<LangType>())
        {
            foreach (var language in Enum.GetValues<LangLanguage>())
            {
                var langsData = LangDataCollection.Load(type, language);
                _langs.Add((type, language), langsData);
            }
        }

        HttpClient = new();
        HttpRetryPolicy = new(5, TimeSpan.FromSeconds(1));
    }

    public static event EventHandler<CheckLangStartedEventArgs>? CheckLangStarted;
    public static event EventHandler<CheckLangFinishedEventArgs>? CheckLangFinished;

    public static void Watch(LangType type, TimeSpan dueTime, TimeSpan interval)
    {
        foreach (var language in Enum.GetValues<LangLanguage>())
        {
            var timer = new Timer(async _ => await CheckAsync(type, language), null, dueTime, interval);

            _timers.AddOrUpdate((type, language), timer, (key, oldValue) => oldValue = timer);
        }
    }

    public static async Task CheckAsync(LangType type, LangLanguage language, bool force = false)
    {
        CheckLangStarted?.Invoke(null, new CheckLangStartedEventArgs(type, language));

        var langsData = _langs[(type, language)];
        var updatedLangsData = await langsData.FetchLangsAsync(force);

        CheckLangFinished?.Invoke(null, new CheckLangFinishedEventArgs(type, language, updatedLangsData));
    }

    internal static string GetOutputDirectoryPath(LangType type, LangLanguage language)
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
