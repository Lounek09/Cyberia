using Cyberia.Langzilla.Enums;
using Cyberia.Langzilla.EventArgs;

using System.Collections.Concurrent;

namespace Cyberia.Langzilla;

/// <summary>
/// Provides methods for watching updates of langs.
/// </summary>
public static class LangsWatcher
{
    public const string OutputPath = "langs";
    public const string BaseUrl = "https://dofusretro.cdn.ankama.com/";

    /// <summary>
    /// Gets a read-only dictionary of the langs by type and language.
    /// </summary>
    public static IReadOnlyDictionary<(LangType, LangLanguage), LangRepository> LangRepositories => _langRepositories.AsReadOnly();

    internal static HttpClient HttpClient { get; set; } = default!;
    internal static HttpRetryPolicy HttpRetryPolicy { get; set; } = default!;

    private static Dictionary<(LangType, LangLanguage), LangRepository> _langRepositories = [];
    private static ConcurrentDictionary<(LangType, LangLanguage), Timer> _timers = [];

    /// <summary>
    /// Initializes the LangsWatcher.
    /// </summary>
    public static void Initialize()
    {
        Directory.CreateDirectory(OutputPath);

        _langRepositories = [];
        _timers = [];

        foreach (var type in Enum.GetValues<LangType>())
        {
            foreach (var language in Enum.GetValues<LangLanguage>())
            {
                var outputPath = GetOutputPath(type, language);
                if (!Directory.Exists(outputPath))
                {
                    Directory.CreateDirectory(outputPath);
                }

                var filePath = Path.Join(outputPath, LangRepository.FileName);
                var repository = LangRepository.LoadFromFile(filePath);
                _langRepositories.Add((type, language), repository);
            }
        }

        HttpClient = new()
        {
            BaseAddress = new(BaseUrl)
        };
        HttpRetryPolicy = new(5, TimeSpan.FromSeconds(1));
    }

    /// <summary>
    /// Event that is triggered when a lang check is started.
    /// </summary>
    public static event EventHandler<CheckLangStartedEventArgs>? CheckLangStarted;

    /// <summary>
    /// Event that is triggered when a lang check is finished.
    /// </summary>
    public static event EventHandler<CheckLangFinishedEventArgs>? CheckLangFinished;


    /// <summary>
    /// Starts watching for update of langs by type.
    /// </summary>
    /// <param name="type">The type of the langs to check.</param>
    /// <param name="dueTime">The amount of time to delay before the first check.</param>
    /// <param name="interval">The interval between checks.</param>
    public static void Watch(LangType type, TimeSpan dueTime, TimeSpan interval)
    {
        foreach (var language in Enum.GetValues<LangLanguage>())
        {
            var repository = _langRepositories[(type, language)];
            var timer = new Timer(async _ => await CheckAsync(repository), null, dueTime, interval);

            _timers.AddOrUpdate((type, language), timer, (key, oldValue) => oldValue = timer);
        }
    }

    /// <summary>
    /// Asynchronously checks for updates of langs by type and language.
    /// </summary>
    /// <param name="type">The type of the langs to check.</param>
    /// <param name="language">The language of the langs to check.</param>
    /// <param name="force">true to force the update; otherwise, false.</param>
    /// <remarks>
    /// This method performs the following steps:
    /// 1. Triggers the <see cref="CheckLangStarted"/> event.
    /// 2. Fetches the version of the langs.
    /// 3. If the version is empty, triggers the <see cref="CheckLangFinished"/> event and returns.
    /// 4. Gets the updated langs from the versions.
    /// 5. Downloads, extracts, and diffs the updated langs.
    /// 6. Triggers the <see cref="CheckLangFinished"/> event.
    /// </remarks>
    public static async Task CheckAsync(LangRepository repository, bool force = false)
    {
        CheckLangStarted?.Invoke(null, new CheckLangStartedEventArgs(repository));

        var versions = await repository.FetchVersionsAsync(force);
        if (string.IsNullOrEmpty(versions))
        {
            CheckLangFinished?.Invoke(null, new CheckLangFinishedEventArgs(repository, []));
            return;
        }

        var updatedLangs = repository.GetUpdatedLangsFromVersions(versions).ToList();
        foreach (var updatedLang in updatedLangs)
        {
            if (!await updatedLang.DownloadAsync())
            {
                Log.Error("Failed to download {LangType} {LangName} lang in {LangLanguage}",
                    repository.Type, updatedLang.Name, repository.Language);
                continue;
            }

            if (!updatedLang.Extract())
            {
                Log.Error("Failed to extract {LangType} {LangName} lang in {LangLanguage}",
                    repository.Type, updatedLang.Name, repository.Language);
                continue;
            }

            if (!updatedLang.SelfDiff())
            {
                Log.Error("Failed to diff {LangType} {LangName} lang in {LangLanguage}",
                    repository.Type, updatedLang.Name, repository.Language);
            }
        }

        CheckLangFinished?.Invoke(null, new CheckLangFinishedEventArgs(repository, updatedLangs));
    }

    /// <summary>
    /// Gets the base route of the langs.
    /// </summary>
    /// <param name="type">The type of the langs.</param>
    /// <returns>The route of the langs.</returns>
    internal static string GetRoute(LangType type)
    {
        return type switch
        {
            LangType.Beta => "betaenv/lang",
            LangType.Temporis => "ephemeris2releasebucket/lang",
            _ => "lang",
        };
    }

    /// <summary>
    /// Gets the output path of the langs.
    /// </summary>
    /// <param name="type">The type of the langs.</param>
    /// <param name="language">The language of the langs.</param>
    /// <returns>The output path of the langs.</returns>
    internal static string GetOutputPath(LangType type, LangLanguage language)
    {
        return Path.Join(OutputPath, type.ToString().ToLower(), language.ToString().ToLower());
    }
}
