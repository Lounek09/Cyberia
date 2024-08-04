using Cyberia.Langzilla.Enums;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Langzilla;

/// <summary>
/// Represents a repository for managing langs.
/// </summary>
public sealed class LangRepository
{
    internal const string FileName = "data.json";

    /// <summary>
    /// Gets the type of the langs.
    /// </summary>
    public LangType Type { get; init; }

    /// <summary>
    /// Gets the language of the langs.
    /// </summary>
    public LangLanguage Language { get; init; }

    /// <summary>
    /// Gets the last modified datetime of the langs.
    /// </summary>
    // TODO: Change the name to LastModified (in the JSON too)
    [JsonInclude]
    public DateTime LastChange { get; internal set; }

    /// <summary>
    /// Gets the langs.
    /// </summary>
    [JsonIgnore]
    public IReadOnlyList<Lang> Langs => LangsCore.AsReadOnly();

    /// <summary>
    /// Gets the name of the version file.
    /// </summary>
    [JsonIgnore]
    public string OutputPath => LangsWatcher.GetOutputPath(Type, Language);

    /// <summary>
    /// Gets the route to the version file.
    /// </summary>
    [JsonIgnore]
    public string VersionFileName => $"versions_{Language.ToStringFast()}.txt";

    /// <summary>
    /// Gets the route to the version file.
    /// </summary>
    [JsonIgnore]
    public string VersionFileRoute => $"{LangsWatcher.GetRoute(Type)}/{VersionFileName}";

    /// <summary>
    /// Gets the path to the version file.
    /// </summary>
    [JsonIgnore]
    public string VersionFilePath => Path.Join(OutputPath, VersionFileName);

    /// <summary>
    /// Gets or sets the core list of langs.
    /// </summary>
    [JsonInclude]
    internal List<Lang> LangsCore { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="LangRepository"/> class.
    /// </summary>
    [JsonConstructor]
    public LangRepository()
    {
        LangsCore = [];
    }

    /// <summary>
    /// Loads a <see cref="LangRepository"/> from a file.
    /// </summary>
    /// <param name="path">The path to the file.</param>
    /// <returns>A new instance of the <see cref="LangRepository"/> class if the file does not exist; otherwise, the data loaded from the file.</returns>
    internal static LangRepository LoadFromFile(string path)
    {
        if (!File.Exists(path))
        {
            return new();
        }

        var json = File.ReadAllText(path);
        return Load(json);
    }

    /// <summary>
    /// Loads a <see cref="LangRepository"/> from a JSON string.
    /// </summary>
    /// <param name="json">The JSON string to load the data from.</param>
    /// <returns>A new instance of the <see cref="LangRepository"/> class if the data could not be deserialized; otherwise, the deserialized data.</returns>
    internal static LangRepository Load(string json)
    {
        var repository = JsonSerializer.Deserialize<LangRepository>(json);
        if (repository is null)
        {
            Log.Error("Failed to deserialize the JSON to initialize {TypeName}", typeof(LangRepository).Name);
            return new();
        }

        return repository;
    }

    /// <summary>
    /// Gets a lang by name.
    /// </summary>
    /// <param name="name">The name of the lang.</param>
    /// <returns>A <see cref="Lang"/>, or null if the lang is not found.</returns>
    public Lang? GetByName(string name)
    {
        return Langs.FirstOrDefault(x => x.Name.Equals(name));
    }

    /// <summary>
    /// Gets all langs that contain the specified name.
    /// </summary>
    /// <param name="name">The name to search for.</param>
    /// <returns>An enumerable collection of langs that contain the specified name.</returns>
    public IEnumerable<Lang> GetAllByName(string name)
    {
        name = name.NormalizeToAscii();

        return Langs.Where(x =>
        {
            return x.Name.NormalizeToAscii().Contains(name, StringComparison.OrdinalIgnoreCase);
        });
    }

    /// <summary>
    /// Asynchronously fetches the versions of the langs.
    /// </summary>
    /// <param name="force">A value that indicates whether to force the fetch operation.</param>
    /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous fetch operation. The value of <see cref="Task{TResult}.Result"/> is a string that represents the version.</returns>
    internal async Task<string> FetchVersionsAsync(bool force = false)
    {
        var versionFileRoute = VersionFileRoute;

        try
        {
            using var response = await LangsWatcher.HttpRetryPolicy.ExecuteAsync(() => LangsWatcher.HttpClient.GetAsync(versionFileRoute));
            response.EnsureSuccessStatusCode();

            var lastModifiedHeader = response.Content.Headers.LastModified!.Value.UtcDateTime;
            if (lastModifiedHeader > LastChange || force)
            {
                LastChange = lastModifiedHeader;

                var versions = await response.Content.ReadAsStringAsync();

                Log.Information("New {LangType} langs detected in {LangLanguage} :\n{Versions}", Type, Language, versions);
                File.WriteAllText(VersionFilePath, versions);

                return versions;
            }
        }
        catch (HttpRequestException e)
        {
            Log.Error(e, "An error occurred while sending Get request to {VersionFileRoute}}", Path.Join(LangsWatcher.BaseUrl, versionFileRoute));
        }

        return string.Empty;
    }

    /// <summary>
    /// Gets the updated langs from the provided versions string.
    /// </summary>
    /// <param name="versions">The versions string, which contains information about the langs.</param>
    /// <returns>An enumerable collection of updated lang.</returns>
    internal IEnumerable<Lang> GetUpdatedLangsFromVersions(string versions)
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

        foreach (var langInfo in langInfos)
        {
            var langParameters = langInfo.Split(',', StringSplitOptions.RemoveEmptyEntries);
            if (langParameters.Length < 3 ||
                !int.TryParse(langParameters[2], out var langVersion))
            {
                continue;
            }

            var lang = new Lang(langParameters[0], langVersion, Type, Language);
            if (!File.Exists(lang.FilePath))
            {
                var index = LangsCore.FindIndex(x => x.Name.Equals(lang.Name));
                if (index == -1)
                {
                    LangsCore.Add(lang);
                }
                else
                {
                    LangsCore[index] = lang;
                }

                yield return lang;
            }
        }

        Save();
    }

    /// <summary>
    /// Saves the repository to a file.
    /// </summary>
    internal void Save()
    {
        var filePath = Path.Join(OutputPath, FileName);
        var json = JsonSerializer.Serialize(this);

        File.WriteAllText(filePath, json);
    }
}
