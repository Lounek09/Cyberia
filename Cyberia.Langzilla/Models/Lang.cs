using Cyberia.Langzilla.Enums;

using System.Text;
using System.Text.Json.Serialization;

namespace Cyberia.Langzilla;

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
    public LangLanguage Language { get; init; }

    /// <summary>
    /// Gets a value indicating whether the lang is new.
    /// </summary>
    [JsonPropertyName("new")]
    public bool New { get; init; }

    /// <summary>
    /// Gets the output path of the lang file.
    /// </summary>
    [JsonIgnore]
    public string OutputPath => Path.Join(LangsWatcher.GetOutputPath(Type, Language), Name);

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
    internal Lang(string name, int version, LangType type, LangLanguage language)
    {
        Name = name;
        Version = version;
        Type = type;
        Language = language;

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
        var currentDecompiledFilePath = CurrentDecompiledFilePath;
        if (!File.Exists(currentDecompiledFilePath))
        {
            return string.Empty;
        }

        var modelDecompiledFilePath = Equals(model) ? OldDecompiledFilePath : model?.CurrentDecompiledFilePath;
        if (!File.Exists(modelDecompiledFilePath))
        {
            var currentLines = File.ReadLines(currentDecompiledFilePath);
            return string.Join('\n', currentLines.Select(x => $"+ {x}"));
        }

        var currentLinesByIndex = File.ReadLines(currentDecompiledFilePath)
            .Select((line, index) => (Line: line, Index: index)) //TODO: .NET9 Use new Index() instead
            .ToDictionary(x => x.Index, x => x.Line);

        var modelLinesByIndex = File.ReadLines(modelDecompiledFilePath)
            .Select((line, index) => (Line: line, Index: index)) //TODO: .NET9 Use new Index() instead
            .ToDictionary(x => x.Index, x => x.Line);

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
    /// Asynchronously downloads the lang file.
    /// </summary>
    /// <returns>A value indicating whether the download was successful.</returns>
    internal async Task<bool> DownloadAsync()
    {
        Array.ForEach(Directory.GetFiles(OutputPath, "*.swf"), File.Delete);

        var fileRoute = FileRoute;

        try
        {
            using var response = await LangsWatcher.HttpRetryPolicy.ExecuteAsync(() => LangsWatcher.HttpClient.GetAsync(fileRoute));
            response.EnsureSuccessStatusCode();

            using var fileStream = new FileStream(FilePath, FileMode.Create);
            await response.Content.CopyToAsync(fileStream);

            return true;
        }
        catch (HttpRequestException e)
        {
            Log.Error(e, "An error occurred while sending Get request to {Url}", Path.Join(LangsWatcher.OutputPath, fileRoute));
        }

        return false;
    }

    /// <summary>
    /// Extracts the lang file.
    /// </summary>
    /// <returns>A value indicating whether the extraction was successful.</returns>
    internal bool Extract()
    {
        var filePath = FilePath;
        if (!File.Exists(filePath))
        {
            return false;
        }

        if (!Flare.TryExtract(filePath, out var flareOutputFilePath))
        {
            Log.Error("An error occured while decompiling {FilePath}", filePath);
            return false;
        }

        var lines = File.ReadLines(flareOutputFilePath, Encoding.UTF8).Skip(7).SkipLast(3);
        var content = lines
            .Select(x => x.Trim())
            .Where(x => x.Length > 0 && !x.Equals("}") && !x.Equals("frame 1 {"));

        var currentDecompiledFilePath = CurrentDecompiledFilePath;
        var oldDecompiledFilePath = OldDecompiledFilePath;

        if (File.Exists(currentDecompiledFilePath))
        {
            File.Move(currentDecompiledFilePath, oldDecompiledFilePath, true);
        }
        File.WriteAllLines(currentDecompiledFilePath, content, Encoding.UTF8);
        File.Delete(flareOutputFilePath);

        return true;
    }

    /// <summary>
    /// Compares the current decompiled lang file with the old one and generates a diff file.
    /// </summary>
    /// <returns>True if the diff file was generated; otherwise, false.</returns>
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
