using Cyberia.Langzilla.Enums;
using Cyberia.Langzilla.EventArgs;
using Cyberia.Langzilla.Models;

using System.Text;

namespace Cyberia.Langzilla;

/// <summary>
/// Represents a service that watches for updates of langs.
/// </summary>
public interface ILangsWatcher
{
    /// <summary>
    /// Gets the lang repository from its type and language.
    /// </summary>
    /// <param name="identifier">The identifier of the repository.</param>
    /// <returns>The lang repository.</returns>
    LangsRepository GetRepository(LangsIdentifier identifier);

    /// <summary>
    /// Starts watching for update of langs by type.
    /// </summary>
    /// <param name="type">The type of the langs to check.</param>
    /// <param name="dueTime">The amount of time to delay before the first check.</param>
    /// <param name="interval">The interval between checks.</param>
    void Watch(LangType type, TimeSpan dueTime, TimeSpan interval);

    /// <summary>
    /// Asynchronously checks for updates of langs for the specified repository.
    /// </summary>
    /// <param name="repository">The repository to check.</param>
    /// <param name="force">Force the check without checking the last update time.</param>
    /// <remarks>
    /// This method performs the following steps:
    /// <list type="number">
    ///     <item>Triggers the <see cref="CheckLangStarted"/> event.</item>
    ///     <item>Fetches the version of the langs.</item>
    ///     <item>If the version is empty, triggers the <see cref="NewLangFilesDetected"/> event and returns.</item>
    ///     <item>Gets the updated langs from the versions.</item>
    ///     <item>Downloads, extracts, and diffs the updated langs.</item>
    ///     <item>Triggers the <see cref="NewLangFilesDetected"/> event.</item>
    /// </list>
    /// </remarks>
    Task CheckAsync(LangsRepository repository, bool force = false);

    /// <summary>
    /// Delegate for the <see cref="NewLangFilesDetected"/> event.
    /// </summary>
    delegate ValueTask NewLangFilesDetectedEventHandler(ILangsWatcher sender, NewLangFilesDetectedEventArgs eventArgs);

    /// <summary>
    /// Event that is triggered when new lang files are detected.
    /// </summary>
    event NewLangFilesDetectedEventHandler NewLangFilesDetected;

    /// <summary>
    /// Delegate for the <see cref="LangsErrored"/> event.
    /// </summary>
    delegate ValueTask LangsErroredEventHandler(ILangsWatcher sender, LangsErroredEventArgs eventArgs);

    /// <summary>
    /// Event that is triggered when an error occurs while checking for updates of langs.
    /// </summary>
    event LangsErroredEventHandler LangsErrored;
}

public sealed class LangsWatcher : ILangsWatcher
{
    /// <summary>
    /// The root output directory.
    /// </summary>
    public const string OutputPath = "langs";

    /// <summary>
    /// The base URL of the langs.
    /// </summary>
    // TODO: Make it configurable
    public const string BaseUrl = "https://dofusretro.cdn.ankama.com/";

    private readonly HttpClient _httpClient;
    private readonly HttpRetryPolicy _httpRetryPolicy;
    private readonly Dictionary<LangsIdentifier, LangsRepository> _langsRepositories = [];
    private readonly Dictionary<LangsIdentifier, Timer> _timers = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="LangsWatcher"/> class.
    /// </summary>
    public LangsWatcher()
    {
        Directory.CreateDirectory(OutputPath);

        _langsRepositories = [];
        _timers = [];

        foreach (var type in Enum.GetValues<LangType>())
        {
            foreach (var language in Enum.GetValues<Language>())
            {
                LangsIdentifier identifier = new(type, language);

                var outputPath = GetOutputPath(identifier);
                Directory.CreateDirectory(outputPath);

                var filePath = Path.Join(outputPath, LangsRepository.FileName);
                var repository = LangsRepository.LoadFromFile(filePath);

                _langsRepositories.Add(identifier, repository);
            }
        }

        _httpClient = new()
        {
            BaseAddress = new(BaseUrl)
        };
        _httpRetryPolicy = new(5, TimeSpan.FromSeconds(1));
    }

    public LangsRepository GetRepository(LangsIdentifier identifier)
    {
        return _langsRepositories[identifier];
    }

    public void Watch(LangType type, TimeSpan dueTime, TimeSpan interval)
    {
        foreach (var language in Enum.GetValues<Language>())
        {
            LangsIdentifier identifier = new(type, language);
            var repository = GetRepository(identifier);

            _timers[identifier] = new Timer(async _ => await CheckAsync(repository), null, dueTime, interval);
        }
    }

    public async Task CheckAsync(LangsRepository repository, bool force = false)
    {
        var versions = await FetchVersionsAsync(repository, force);
        if (string.IsNullOrEmpty(versions))
        {
            await OnNewLangFilesDetected(new NewLangFilesDetectedEventArgs(repository, []));
            return;
        }

        var updatedLangs = GetUpdatedLangs(repository, versions).ToList();
        foreach (var updatedLang in updatedLangs)
        {
            if (!await DownloadLangAsync(updatedLang))
            {
                Log.Error("Failed to download {LangType} {LangName} lang in {LangLanguage}", repository.Type, updatedLang.Name, repository.Language);
                continue;
            }

            if (!ExtractLang(updatedLang))
            {
                Log.Error("Failed to extract {LangType} {LangName} lang in {LangLanguage}", repository.Type, updatedLang.Name, repository.Language);
                continue;
            }

            updatedLang.SelfDiff();
        }

        if (updatedLangs.Count > 0)
        {
            await OnNewLangFilesDetected(new NewLangFilesDetectedEventArgs(repository, updatedLangs));
        }
    }

    /// <summary>
    /// Gets the base route of the langs.
    /// </summary>
    /// <param name="type">The type of the langs.</param>
    /// <returns>The route of the langs.</returns>
    // TODO: Make it configurable
    internal static string GetRoute(LangType type)
    {
        return type switch
        {
            LangType.Beta => "static-data/638/lang",
            LangType.Temporis => "t3mporis-release/lang",
            _ => "/static-data/612/lang",
        };
    }

    /// <summary>
    /// Gets the output path of the langs.
    /// </summary>
    /// <param name="identifier">The identifier of the langs.</param>
    /// <returns>The output path of the langs.</returns>
    internal static string GetOutputPath(LangsIdentifier identifier)
    {
        return Path.Join(OutputPath, identifier.Type.ToStringFast().ToLower(), identifier.Language.ToStringFast());
    }

    /// <summary>
    /// Fetches the versions of the langs.
    /// </summary>
    /// <param name="repository">The repository to fetch the versions from.</param>
    /// <param name="force">Force the fetch without checking the last update time.</param>
    /// <returns>A string that contains the versions of the langs.</returns>
    internal async Task<string> FetchVersionsAsync(LangsRepository repository, bool force = false)
    {
        try
        {
            using var response = await _httpRetryPolicy.ExecuteAsync(() => _httpClient.GetAsync(repository.VersionFileRoute));
            response.EnsureSuccessStatusCode();

            var lastModifiedHeader = response.Content.Headers.LastModified!.Value.UtcDateTime;
            if (lastModifiedHeader > repository.LastChange || force)
            {
                repository.LastChange = lastModifiedHeader;

                var versions = await response.Content.ReadAsStringAsync();

                Log.Information("New {LangType} langs detected in {LangLanguage} :\n{Versions}", repository.Type, repository.Language, versions);
                File.WriteAllText(repository.VersionFilePath, versions);

                return versions;
            }
        }
        catch (HttpRequestException e)
        {
            Log.Error(e, "An error occurred while sending Get request to {VersionFileUrl}", Path.Join(BaseUrl, repository.VersionFileRoute));
        }

        return string.Empty;
    }

    /// <summary>
    /// Gets the updated langs from the fetched versions of the langs.
    /// </summary>
    /// <param name="versions">The versions of the langs.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="Lang"/> that contains the updated langs.</returns>
    internal static IEnumerable<Lang> GetUpdatedLangs(LangsRepository repository, string versions)
    {
        if (versions.Length < 4)
        {
            yield break;
        }

        var langInfos = versions[3..].Split("|", StringSplitOptions.RemoveEmptyEntries);
        if (langInfos.Length == 0)
        {
            yield break;
        }

        LangsIdentifier identifier = new(repository.Type, repository.Language);

        foreach (var langInfo in langInfos)
        {
            //TODO: Use Span
            var langParameters = langInfo.Split(',', StringSplitOptions.RemoveEmptyEntries);
            if (langParameters.Length < 3 ||
                !int.TryParse(langParameters[2], out var langVersion))
            {
                continue;
            }

            var lang = new Lang(langParameters[0], langVersion, identifier);
            if (!File.Exists(lang.FilePath))
            {
                repository.AddOrUpdate(lang);

                yield return lang;
            }
        }

        repository.Save();
    }

    /// <summary>
    /// Downloads the lang file.
    /// </summary>
    /// <param name="lang">The lang to download.</param>
    /// <returns><see langword="true"/> if the download was successful; otherwise, <see langword="false"/>.</returns>
    internal async Task<bool> DownloadLangAsync(Lang lang)
    {
        Array.ForEach(Directory.GetFiles(lang.OutputPath, "*.swf"), File.Delete);

        try
        {
            using var response = await _httpRetryPolicy.ExecuteAsync(() => _httpClient.GetAsync(lang.FileRoute));
            response.EnsureSuccessStatusCode();

            using var fileStream = new FileStream(lang.FilePath, FileMode.Create);
            await response.Content.CopyToAsync(fileStream);

            return true;
        }
        catch (HttpRequestException e)
        {
            Log.Error(e, "An error occurred while sending Get request to {Url}", Path.Join(BaseUrl, lang.FileRoute));
        }

        return false;
    }

    /// <summary>
    /// Extracts the lang file.
    /// </summary>
    /// <param name="lang">The lang to extract.</param>
    /// <returns><see langword="true"/> if the extraction was successful; otherwise, <see langword="false"/>.</returns>
    internal static bool ExtractLang(Lang lang)
    {
        if (!File.Exists(lang.FilePath))
        {
            return false;
        }

        if (!Flare.TryExtract(lang.FilePath, out var flareOutputFilePath))
        {
            Log.Error("An error occured while decompiling {FilePath}", lang.FilePath);
            return false;
        }

        var lines = File.ReadLines(flareOutputFilePath, Encoding.UTF8).Skip(7).SkipLast(3);
        var content = lines
            .Select(x => x.Trim())
            .Where(x => x.Length > 0 && !x.Equals("}") && !x.Equals("frame 1 {"));

        var currentDecompiledFilePath = lang.CurrentDecompiledFilePath;
        var oldDecompiledFilePath = lang.OldDecompiledFilePath;

        if (File.Exists(currentDecompiledFilePath))
        {
            File.Move(currentDecompiledFilePath, oldDecompiledFilePath, true);
        }
        File.WriteAllLines(currentDecompiledFilePath, content, Encoding.UTF8);
        File.Delete(flareOutputFilePath);

        return true;
    }

    #region Events

    public event ILangsWatcher.NewLangFilesDetectedEventHandler? NewLangFilesDetected;

    /// <summary>
    /// Triggers the <see cref="NewLangFilesDetected"/> event.
    /// </summary>
    internal async ValueTask OnNewLangFilesDetected(NewLangFilesDetectedEventArgs eventArgs)
    {
        var handler = NewLangFilesDetected;
        if (handler is not null)
        {
            await handler.Invoke(this, eventArgs);
        }
    }

    public event ILangsWatcher.LangsErroredEventHandler? LangsErrored;

    internal async ValueTask OnLangsErroredAsync(LangsErroredEventArgs eventArgs)
    {
        var handler = LangsErrored;
        if (handler is not null)
        {
            await handler.Invoke(this, eventArgs);
        }
    }

    #endregion
}
