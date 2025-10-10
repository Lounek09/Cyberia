using Cyberia.Langzilla.Enums;

using System.Text;
using System.Text.Json.Serialization;

namespace Cyberia.Langzilla.Models;

/// <summary>
/// Represents a lang data.
/// </summary>
public sealed class Lang
{
    /// <summary>
    /// Gets the name of the lang.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; init; }

    /// <summary>
    /// Gets the version of the lang.
    /// </summary>
    [JsonPropertyName("version")]
    public int Version { get; init; }

    /// <summary>
    /// Gets the type of the lang.
    /// </summary>
    [JsonPropertyName("type")]
    public LangType Type { get; init; }

    /// <summary>
    /// Gets the language of the lang.
    /// </summary>
    [JsonPropertyName("language")]
    public Language Language { get; init; }

    /// <summary>
    /// Gets a value indicating whether the lang is new.
    /// </summary>
    [JsonPropertyName("new")]
    public bool New { get; init; }

    /// <summary>
    /// Gets the output path of the lang file.
    /// </summary>
    [JsonIgnore]
    public string OutputPath => Path.Join(LangsWatcher.GetOutputPath(new LangsIdentifier(Type, Language)), Name);

    /// <summary>
    /// Gets the file name of the lang file.
    /// </summary>
    [JsonIgnore]
    public string FileName => $"{Name}_{Language.ToStringFast()}_{Version}.swf";

    /// <summary>
    /// Gets the route of the lang file.
    /// </summary>
    [JsonIgnore]
    public string FileRoute => $"{LangsWatcher.GetRoute(Type)}/swf/{FileName}";

    /// <summary>
    /// Gets the file path of the lang file.
    /// </summary>
    [JsonIgnore]
    public string FilePath => Path.Join(OutputPath, FileName);

    /// <summary>
    /// Gets the path of the current decompiled lang file.
    /// </summary>
    [JsonIgnore]
    public string CurrentDecompiledFilePath => Path.Join(OutputPath, "current.as");

    /// <summary>
    /// Gets the path of the old decompiled lang file.
    /// </summary>
    [JsonIgnore]
    public string OldDecompiledFilePath => Path.Join(OutputPath, "old.as");

    /// <summary>
    /// Gets the path of the generated diff file.
    /// </summary>
    [JsonIgnore]
    public string DiffFilePath => Path.Join(OutputPath, "diff.as");

    /// <summary>
    /// Initializes a new instance of the <see cref="Lang"/> class.
    /// </summary>
    [JsonConstructor]
    internal Lang()
    {
        Name = string.Empty;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Lang"/> class.
    /// </summary>
    /// <param name="name">The name of the lang.</param>
    /// <param name="version">The version of the lang.</param>
    /// <param name="type">The type of the lange.</param>
    /// <param name="language">The language of the lang.</param>
    internal Lang(string name, int version, LangsIdentifier identifier)
    {
        Name = name;
        Version = version;
        Type = identifier.Type;
        Language = identifier.Language;

        var outputPath = OutputPath;
        New = !Directory.Exists(outputPath);
        if (New)
        {
            Directory.CreateDirectory(outputPath);
        }
    }

    /// <summary>
    /// Compares this instance with the specified model and returns a string that represents the differences.
    /// </summary>
    /// <param name="model">The model to compare with this instance.</param>
    /// <returns>A string that represents the differences.</returns>
    public string Diff(Lang? model)
    {
        if (!File.Exists(CurrentDecompiledFilePath))
        {
            return string.Empty;
        }

        var modelDecompiledFilePath = Equals(model) ? OldDecompiledFilePath : model?.CurrentDecompiledFilePath;
        if (!File.Exists(modelDecompiledFilePath))
        {
            var currentLines = File.ReadLines(CurrentDecompiledFilePath);
            return string.Join('\n', currentLines.Select(x => $"+ {x}"));
        }

        var currentLinesByIndex = File.ReadLines(CurrentDecompiledFilePath)
            .Index()
            .ToDictionary(x => x.Index, x => x.Item);

        var modelLinesByIndex = File.ReadLines(modelDecompiledFilePath)
            .Index()
            .ToDictionary(x => x.Index, x => x.Item);

        List<KeyValuePair<int, string>> diff = new();

        foreach (var row in currentLinesByIndex)
        {
            if (!modelLinesByIndex.RemoveByValue(row.Value))
            {
                diff.Add(new KeyValuePair<int, string>(row.Key, $"+ {row.Value}"));
            }
        }

        foreach (var row in modelLinesByIndex)
        {
            diff.Add(new KeyValuePair<int, string>(row.Key, $"- {row.Value}"));
        }

        return string.Join('\n', diff.OrderBy(x => x.Key).Select(x => x.Value));
    }

    /// <summary>
    /// Compares the current decompiled lang file with the old one and generates a diff file.
    /// </summary>
    /// <returns><see langword="true"/> if the diff file was generated; otherwise, <see langword="false"/>.</returns>
    internal bool SelfDiff()
    {
        var diff = Diff(this);
        if (string.IsNullOrEmpty(diff))
        {
            File.Delete(DiffFilePath);
            return false;
        }

        File.WriteAllText(DiffFilePath, diff, Encoding.UTF8);
        return true;
    }
}
