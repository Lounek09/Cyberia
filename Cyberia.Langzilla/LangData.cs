using Cyberia.Langzilla.Enums;

using System.Text;
using System.Text.Json.Serialization;

namespace Cyberia.Langzilla;

public sealed class LangData
{
    [JsonPropertyName("name")]
    public string Name { get; init; }

    [JsonPropertyName("version")]
    public int Version { get; init; }

    [JsonPropertyName("type")]
    public LangType Type { get; init; }

    [JsonPropertyName("language")]
    public LangLanguage Language { get; init; }

    [JsonPropertyName("new")]
    public bool New { get; init; }

    [JsonConstructor]
    public LangData()
    {
        Name = string.Empty;
    }

    internal LangData(string name, int version, LangType type, LangLanguage language)
    {
        Name = name;
        Version = version;
        Type = type;
        Language = language;

        var directoryPath = GetDirectoryPath();
        New = !Directory.Exists(directoryPath);
        Directory.CreateDirectory(directoryPath);
    }

    public string GetDirectoryPath()
    {
        return Path.Join(LangsWatcher.GetOutputDirectoryPath(Type, Language), Name);
    }

    public string GetFileName()
    {
        return $"{Name}_{Language.ToString().ToLower()}_{Version}.swf";
    }

    public string GetFileUrl()
    {
        return $"{LangsWatcher.BASE_URL}/{LangsWatcher.GetRoute(Type)}/swf/{GetFileName()}";
    }

    public string GetFilePath()
    {
        return Path.Join(GetDirectoryPath(), GetFileName());
    }

    public string GetCurrentDecompiledFilePath()
    {
        return Path.Join(GetDirectoryPath(), "current.as");
    }

    public string GetOldDecompiledFilePath()
    {
        return Path.Join(GetDirectoryPath(), "old.as");
    }

    public string GetDiffFilePath()
    {
        return Path.Join(GetDirectoryPath(), "diff.as");
    }

    public string GenerateDiff(LangData langData)
    {
        var currentDecompiledFilePath = GetCurrentDecompiledFilePath();
        if (!File.Exists(currentDecompiledFilePath))
        {
            return "";
        }
        var currentLines = File.ReadAllLines(currentDecompiledFilePath);

        var modelDecompiledFilePath = langData == this ? GetOldDecompiledFilePath() : langData.GetCurrentDecompiledFilePath();
        if (!File.Exists(modelDecompiledFilePath))
        {
            return string.Join('\n', currentLines.Select(x => $"+ {x}"));
        }
        var modelLines = File.ReadAllLines(modelDecompiledFilePath);

        List<(int Index, string Row)> diff = [];

        var index = 0;
        var currentRows = currentLines.ToDictionary(x => index++);

        index = 0;
        var modelRows = modelLines.ToDictionary(x => index++);

        foreach (var row in currentRows)
        {
            if (!modelRows.RemoveByValue(row.Value, true))
            {
                diff.Add((row.Key, $"+ {row.Value}"));
            }
        }

        foreach (var row in modelRows)
        {
            diff.Add((row.Key, $"- {row.Value}"));
        }

        return string.Join('\n', diff.OrderBy(x => x.Index).Select(x => x.Row));
    }

    public async Task DownloadExtractAndDiffAsync()
    {
        await DownloadAsync();
        Extract();
        Diff();
    }

    private async Task DownloadAsync()
    {
        Array.ForEach(Directory.GetFiles(GetDirectoryPath(), "*.swf"), File.Delete);

        var fileUrl = GetFileUrl();

        try
        {
            using var response = await LangsWatcher.HttpRetryPolicy.ExecuteAsync(() => LangsWatcher.HttpClient.GetAsync(fileUrl));
            response.EnsureSuccessStatusCode();

            using FileStream fileStream = new(GetFilePath(), FileMode.Create);
            await response.Content.CopyToAsync(fileStream);

            return;
        }
        catch (HttpRequestException e)
        {
            Log.Error(e, "An error occurred while sending Get request to {Url}", fileUrl);
        }
    }

    private void Extract()
    {
        var filePath = GetFilePath();
        if (!File.Exists(filePath))
        {
            return;
        }

        var currentDecompiledFilePath = GetCurrentDecompiledFilePath();
        var oldDecompiledFilePath = GetOldDecompiledFilePath();

        if (!Flare.TryExtractSwf(filePath, out var flareOutputFilePath))
        {
            Log.Error("Error when decompiling {FilePath}", filePath);
            return;
        }

        List<string> content = [];
        foreach (var line in File.ReadAllLines(flareOutputFilePath, Encoding.UTF8).Skip(7).SkipLast(3))
        {
            var temp = line.Trim();

            if (temp.Length == 0 || temp.Equals("}") || temp.Equals("frame 1 {"))
            {
                continue;
            }

            content.Add(temp);
        }

        if (File.Exists(currentDecompiledFilePath))
        {
            File.Move(currentDecompiledFilePath, oldDecompiledFilePath, true);
        }
        File.WriteAllLines(currentDecompiledFilePath, content, Encoding.UTF8);
        File.Delete(flareOutputFilePath);
    }

    private void Diff()
    {
        var diff = GenerateDiff(this);
        if (string.IsNullOrEmpty(diff))
        {
            File.Delete(GetDiffFilePath());
            return;
        }

        File.WriteAllText(GetDiffFilePath(), diff, Encoding.UTF8);
    }
}
