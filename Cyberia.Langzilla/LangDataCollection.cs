using Cyberia.Langzilla.Enums;
using Cyberia.Langzilla.JsonConverters;

using System.Collections;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Langzilla;

[JsonConverter(typeof(LangDataCollectionConverter))]
public sealed class LangDataCollection : IReadOnlyCollection<LangData>
{
    private const string DATA_FILE_NAME = "data.json";

    public LangType Type { get; internal set; }

    public LangLanguage Language { get; internal set; }

    public long LastChange { get; set; }

    public ReadOnlyCollection<LangData> Items => ItemsCore.AsReadOnly();

    public int Count => ItemsCore.Count;

    internal List<LangData> ItemsCore { private get; set; }

    public LangDataCollection()
    {
        ItemsCore = [];
    }

    internal LangDataCollection(LangType type, LangLanguage language)
        : this()
    {
        Type = type;
        Language = language;

        Directory.CreateDirectory(LangsWatcher.GetOutputDirectoryPath(type, language));
    }

    public static LangDataCollection Load(LangType type, LangLanguage language)
    {
        var dataFilePath = Path.Join(LangsWatcher.GetOutputDirectoryPath(type, language), DATA_FILE_NAME);
        if (!File.Exists(dataFilePath))
        {
            return new LangDataCollection(type, language);
        }

        var json = File.ReadAllText(dataFilePath);
        return JsonSerializer.Deserialize<LangDataCollection>(json) ?? new(type, language);
    }

    public string GetVersionFileName()
    {
        return $"versions_{Language.ToString().ToLower()}.txt";
    }

    public string GetVersionFileUrl()
    {
        return $"{LangsWatcher.BASE_URL}/{LangsWatcher.GetRoute(Type)}/{GetVersionFileName()}";
    }

    public string GetVersionFilePath()
    {
        return Path.Join(LangsWatcher.GetOutputDirectoryPath(Type, Language), GetVersionFileName());
    }

    public DateTime GetDateTimeSinceLastChange()
    {
        return DateTimeOffset.FromUnixTimeMilliseconds(LastChange).DateTime;
    }

    public LangData? GetLangByName(string name)
    {
        return ItemsCore.Find(x => x.Name.Equals(name));
    }

    public List<LangData> GetLangsByName(string name)
    {
        return ItemsCore.FindAll(x => x.Name.NormalizeCustom().Contains(name.NormalizeCustom()));
    }

    public IEnumerator<LangData> GetEnumerator()
    {
        return ItemsCore.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    internal async Task<List<LangData>> FetchLangsAsync(bool force)
    {
        var versionFile = await FetchVersionFileAsync(force);
        if (string.IsNullOrEmpty(versionFile))
        {
            return [];
        }

        return await ProcessLangsAsync(versionFile);
    }

    private async Task<string> FetchVersionFileAsync(bool force)
    {
        var versionFileUrl = GetVersionFileUrl();

        try
        {
            using var response = await LangsWatcher.HttpRetryPolicy.ExecuteAsync(() => LangsWatcher.HttpClient.GetAsync(versionFileUrl));
            response.EnsureSuccessStatusCode();

            var lastModifiedHeader = response.Content.Headers.LastModified!.Value.ToUnixTimeMilliseconds();
            var isMoreRecent = lastModifiedHeader > LastChange;

            if (isMoreRecent)
            {
                LastChange = lastModifiedHeader;
            }

            if (isMoreRecent || force)
            {
                var versionFile = await response.Content.ReadAsStringAsync();

                Log.Information("New {LangType} langs detected in {LangLanguage} :\n{VersionFileContent}", Type, Language, versionFile);
                File.WriteAllText(GetVersionFilePath(), versionFile);

                return versionFile;
            }
        }
        catch (HttpRequestException e)
        {
            Log.Error(e, "An error occurred while sending Get request to {Url}}", versionFileUrl);
        }

        return string.Empty;
    }

    private async Task<List<LangData>> ProcessLangsAsync(string versionFileContent)
    {
        List<LangData> updatedLangsData = [];

        var langInfos = versionFileContent[3..].Split("|", StringSplitOptions.RemoveEmptyEntries);
        foreach (var langInfo in langInfos)
        {
            var langParameters = langInfo.Split(',');

            LangData langData = new(langParameters[0], int.Parse(langParameters[2]), Type, Language);
            if (!File.Exists(langData.GetFilePath()))
            {
                await langData.DownloadExtractAndDiffAsync();

                updatedLangsData.Add(langData);

                var index = ItemsCore.FindIndex(x => x.Name.Equals(langData.Name));
                if (index == -1)
                {
                    ItemsCore.Add(langData);
                }
                else
                {
                    ItemsCore[index] = langData;
                }
            }
        }

        Save();
        return updatedLangsData;
    }

    private void Save()
    {
        var filePath = Path.Join(LangsWatcher.GetOutputDirectoryPath(Type, Language), DATA_FILE_NAME);
        var json = JsonSerializer.Serialize(this);

        File.WriteAllText(filePath, json);
    }
}
