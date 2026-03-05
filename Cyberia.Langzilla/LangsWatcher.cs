using Cyberia.Database.Models;
using Cyberia.Database.Repositories;
using Cyberia.Langzilla.EventArgs;
using Cyberia.Langzilla.Extensions;
using Cyberia.Langzilla.Primitives;

using System.Collections.ObjectModel;
using System.Text;

namespace Cyberia.Langzilla;

/// <summary>
/// Represents a service that watches for updates of langs.
/// </summary>
public interface ILangsWatcher
{
    /// <summary>
    /// Gets when the langs were last modified.
    /// </summary>
    public ReadOnlyDictionary<LangsIdentifier, DateTime> LastModifieds { get; }

    /// <summary>
    /// Initializes the data of the <see cref="ILangsWatcher"/> instance.
    /// </summary>
    /// <remarks>
    /// This need to be called before everything else.
    /// </remarks>
    Task InitializeAsync();

    /// <summary>
    /// Starts watching for update of langs by type.
    /// </summary>
    /// <param name="type">The type of the langs to check.</param>
    /// <param name="dueTime">The amount of time to delay before the first check.</param>
    /// <param name="interval">The interval between checks.</param>
    void Watch(LangType type, TimeSpan dueTime, TimeSpan interval);

    /// <summary>
    /// Checks for updates of langs.
    /// </summary>
    /// <param name="identifier">The identifier of the langs to check.</param>
    /// <param name="force">Force the check without checking the last modified time.</param>
    Task CheckAsync(LangsIdentifier identifier, bool force = false);

    /// <summary>
    /// Delegate for the <see cref="NewLangsDetected"/> event.
    /// </summary>
    delegate ValueTask NewLangsDetectedEventHandler(ILangsWatcher sender, NewLangsDetectedEventArgs eventArgs);

    /// <summary>
    /// Event that is triggered when new lang are detected.
    /// </summary>
    event NewLangsDetectedEventHandler NewLangsDetected;

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

    public ReadOnlyDictionary<LangsIdentifier, DateTime> LastModifieds => _lastModifieds.AsReadOnly();

    private readonly LangRepository _langRepository;
    private readonly MonitoredFileRepository _monitoredFileRepository;
    private readonly HttpClient _httpClient;
    private readonly HttpRetryPolicy _httpRetryPolicy;
    private readonly Dictionary<LangsIdentifier, DateTime> _lastModifieds;
    private readonly Dictionary<LangsIdentifier, Timer> _timers = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="LangsWatcher"/> class.
    /// </summary>
    public LangsWatcher(LangRepository langRepository, MonitoredFileRepository monitoredFileRepository)
    {
        _langRepository = langRepository;
        _monitoredFileRepository = monitoredFileRepository;
        _httpClient = new()
        {
            BaseAddress = new(BaseUrl)
        };
        _httpRetryPolicy = new(5, TimeSpan.FromSeconds(1));
        _lastModifieds = [];
        _timers = [];
    }

    public async Task InitializeAsync()
    {
        Directory.CreateDirectory(OutputPath);

        foreach (var type in Enum.GetValues<LangType>())
        {
            foreach (var language in Enum.GetValues<Language>())
            {
                LangsIdentifier identifier = new(type, language);

                var outputPath = LangRepository.GetOutputPath(identifier);
                Directory.CreateDirectory(outputPath);

                var lastModified = await _monitoredFileRepository.GetLastModifiedByIdAsync(identifier.ToString());
                _lastModifieds[identifier] = lastModified;
            }
        }
    }

    public void Watch(LangType type, TimeSpan dueTime, TimeSpan interval)
    {
        foreach (var language in Enum.GetValues<Language>())
        {
            LangsIdentifier identifier = new(type, language);

            _timers[identifier] = new Timer(async _ => await CheckAsync(identifier), null, dueTime, interval);
        }
    }

    public async Task CheckAsync(LangsIdentifier identifier, bool force = false)
    {
        var versionsFileContent = await FetchVersionsAsync(identifier, force);
        if (versionsFileContent is null)
        {
            return;
        }

        var updatedLangs = await GetUpdatedLangsAsync(identifier, versionsFileContent);
        if (updatedLangs.Count == 0)
        {
            return;
        }

        foreach (var updatedLang in updatedLangs)
        {
            if (!await DownloadLangAsync(updatedLang))
            {
                return;
            }

            if (!await ExtractLangAsync(updatedLang))
            {
                return;
            }

            var diff = Lang.Diff(updatedLang, updatedLang);
            if (string.IsNullOrEmpty(diff))
            {
                File.Delete(updatedLang.GetDiffFilePath());
                continue;
            }

            File.WriteAllText(updatedLang.GetDiffFilePath(), diff, Encoding.UTF8);
        }

        await OnNewLangsDetected(new NewLangsDetectedEventArgs(identifier, updatedLangs));
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
    /// <param name="identifier">The identifier of the langs.</param>
    /// <param name="force">Force the fetch without checking the last modified time.</param>
    /// <returns>A string that contains the versions of the langs.</returns>
    internal async Task<string?> FetchVersionsAsync(LangsIdentifier identifier, bool force)
    {
        var route = LangRepository.GetVersionsFileRoute(identifier);

        try
        {
            using var response = await _httpRetryPolicy.ExecuteAsync(() => _httpClient.GetAsync(route));
            response.EnsureSuccessStatusCode();

            var lastModified = response.Content.Headers.LastModified?.UtcDateTime;
            if (lastModified is null || lastModified.Value <= _lastModifieds[identifier] && !force)
            {
                return null;
            }

            _lastModifieds[identifier] = lastModified.Value;
            await _monitoredFileRepository.UpsertAsync(new MonitoredFile
            {
                Id = identifier.ToString(),
                LastModified = lastModified.Value
            });

            var content = await response.Content.ReadAsStringAsync();

            Log.Information("New {Type} langs detected in {Language} :\n{Content}", identifier.Type, identifier.Language, content);
            File.WriteAllText(LangRepository.GetVersionsFilePath(identifier), content);

            return content;
        }
        catch (HttpRequestException e)
        {
            var url = $"{BaseUrl}/{route}";

            Log.Error(e, "An error occurred while sending Get request to {Url}", url);
            await OnLangsErroredAsync(new LangsErroredEventArgs(e, $"An error occurred while sending a GET request to '{url}', see the logs for more details."));
        }

        return null;
    }

    /// <summary>
    /// Gets the updated langs from the fetched versions file.
    /// </summary>
    /// <param name="versionsFileContent">The content of the versions file.</param>
    /// <returns>A <see cref="ReadOnlyCollection{T}"/> of <see cref="Lang"/> that contains the updated langs.</returns>
    internal async Task<ReadOnlyCollection<Lang>> GetUpdatedLangsAsync(LangsIdentifier identifier, string versionsFileContent)
    {
        if (!versionsFileContent.StartsWith("&f="))
        {
            return ReadOnlyCollection<Lang>.Empty;
        }

        var content = versionsFileContent.AsSpan(3);
        List<(string Name, int Version)> parsedLangs = [];
        Span<Range> fieldRanges = stackalloc Range[3];

        foreach (var contentRange in content.Split('|'))
        {
            var fields = content[contentRange];
            if (fields.IsEmpty)
            {
                continue;
            }

            var fieldCount = fields.Split(fieldRanges, ',', StringSplitOptions.RemoveEmptyEntries);
            if (fieldCount != 3)
            {
                continue;
            }

            if (!int.TryParse(fields[fieldRanges[2]], out var version))
            {
                continue;
            }

            var name = fields[fieldRanges[0]].ToString();

            parsedLangs.Add(new(name, version));
        }

        List<Lang> updatedLangs = new(parsedLangs.Count);

        foreach (var (name, version) in parsedLangs)
        {
            var lang = await _langRepository.GetByIdentifierAndNameAsync(identifier, name);

            if (lang?.Version == version)
            {
                continue;
            }

            lang ??= new()
            {
                Type = identifier.Type,
                Language = identifier.Language,
                Name = name,
                Version = version,
                IsNew = true
            };

            if (lang.Version != version)
            {
                lang.Version = version;
            }

            updatedLangs.Add(lang);
        }

        await _langRepository.UpsertManyAsync(updatedLangs);

        return updatedLangs.AsReadOnly();
    }

    /// <summary>
    /// Downloads the lang file.
    /// </summary>
    /// <param name="lang">The lang to download.</param>
    /// <returns><see langword="true"/> if the download was successful; otherwise, <see langword="false"/>.</returns>
    internal async Task<bool> DownloadLangAsync(Lang lang)
    {
        var outputPath = lang.GetOutputPath();
        Directory.CreateDirectory(outputPath);

        foreach (var filePath in Directory.EnumerateFiles(lang.GetOutputPath(), "*.swf"))
        {
            File.Delete(filePath);
        }

        var route = lang.GetRoute();

        try
        {
            using var response = await _httpRetryPolicy.ExecuteAsync(() => _httpClient.GetAsync(route));
            response.EnsureSuccessStatusCode();

            using var fileStream = new FileStream(lang.GetFilePath(), FileMode.Create);
            await response.Content.CopyToAsync(fileStream);

            return true;
        }
        catch (HttpRequestException e)
        {
            var url = $"{BaseUrl}/{route}";

            Log.Error(e, "An error occurred while sending Get request to {Url}", url);
            await OnLangsErroredAsync(new LangsErroredEventArgs(e, $"An error occurred while sending a GET request to '{url}', see the logs for more details."));
        }

        return false;
    }

    /// <summary>
    /// Extracts the lang file using flare.
    /// </summary>
    /// <param name="lang">The lang to extract.</param>
    /// <returns><see langword="true"/> if the extraction was successful; otherwise, <see langword="false"/>.</returns>
    internal async Task<bool> ExtractLangAsync(Lang lang)
    {
        var filePath = lang.GetFilePath();

        if (!File.Exists(filePath))
        {
            Log.Error("Trying to extract a missing lang on disk: {FilePath}", filePath);
            await OnLangsErroredAsync(new LangsErroredEventArgs($"Trying to extract a missing lang on disk: '{filePath}'"));

            return false;
        }

        if (!Flare.TryExtract(filePath, out var flareOutputFilePath))
        {
            Log.Error("An error occurred while trying to extract {FilePath}", filePath);
            await OnLangsErroredAsync(new LangsErroredEventArgs($"An error occurred while trying to extract '{filePath}'"));

            return false;
        }

        var decompiledFilePath = lang.GetDecompiledFilePath();
        var oldDecompiledFilePath = lang.GetOldDecompiledFilePath();

        if (File.Exists(decompiledFilePath))
        {
            File.Move(decompiledFilePath, oldDecompiledFilePath, true);
        }

        using (StreamReader reader = new(flareOutputFilePath, Encoding.UTF8))
        {
            using StreamWriter writer = new(decompiledFilePath, false, Encoding.UTF8);

            for (var i = 0; i < 7; i++)
            {
                reader.ReadLine();
            }

            string? raw;
            while ((raw = reader.ReadLine()) is not null)
            {
                var line = raw.AsSpan().TrimStart();

                if (line.IsEmpty || line[0] == '}' || line.StartsWith("frame"))
                {
                    continue;
                }

                if (line.StartsWith("FILE_END"))
                {
                    break;
                }

                writer.Write(line);
                writer.Write('\n');
            }
        }

        File.Delete(flareOutputFilePath);

        return true;
    }

    #region Events

    public event ILangsWatcher.NewLangsDetectedEventHandler? NewLangsDetected;

    /// <summary>
    /// Triggers the <see cref="NewLangsDetected"/> event.
    /// </summary>
    internal async ValueTask OnNewLangsDetected(NewLangsDetectedEventArgs eventArgs)
    {
        var handler = NewLangsDetected;
        if (handler is not null)
        {
            await handler.Invoke(this, eventArgs);
        }
    }

    public event ILangsWatcher.LangsErroredEventHandler? LangsErrored;

    /// <summary>
    /// Triggers the <see cref="LangsErrored"/> event.
    /// </summary>
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
