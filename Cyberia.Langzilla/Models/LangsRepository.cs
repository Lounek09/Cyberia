using Cyberia.Langzilla.Enums;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Langzilla.Models;

/// <summary>
/// Represents a repository for managing langs.
/// </summary>
public sealed class LangsRepository
{
    internal const string FileName = "data.json";

    /// <summary>
    /// Gets the type of the langs.
    /// </summary>
    public LangType Type { get; init; }

    /// <summary>
    /// Gets the language of the langs.
    /// </summary>
    public Language Language { get; init; }

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
    private List<Lang> LangsCore { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="LangsRepository"/> class.
    /// </summary>
    [JsonConstructor]
    public LangsRepository()
    {
        LangsCore = [];
    }

    /// <summary>
    /// Loads a <see cref="LangsRepository"/> from a file.
    /// </summary>
    /// <param name="path">The path to the file.</param>
    /// <returns>A new instance of the <see cref="LangsRepository"/> class if the file does not exist; otherwise, the data loaded from the file.</returns>
    internal static LangsRepository LoadFromFile(string path)
    {
        if (!File.Exists(path))
        {
            return new();
        }

        var json = File.ReadAllText(path);

        var repository = JsonSerializer.Deserialize<LangsRepository>(json);
        if (repository is null)
        {
            Log.Error("Failed to deserialize the JSON to initialize {TypeName}", typeof(LangsRepository).Name);
            return new();
        }

        return repository;
    }

    /// <summary>
    /// Gets a lang by name.
    /// </summary>
    /// <param name="name">The name of the lang.</param>
    /// <returns>A <see cref="Lang"/>, or <see langword="null"/> if the lang is not found.</returns>
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
    /// Adds or updates a lang by its name.
    /// </summary>
    /// <param name="lang">The lang to add or update.</param>
    internal void AddOrUpdate(Lang lang)
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
