using Cyberia.Langzilla.Enums;

using System.Text.Json.Serialization;

namespace Cyberia.Langzilla
{
    public sealed class LangDataCollection
    {
        private const string DATA_FILE_NAME = "data.json";

        [JsonPropertyName("type")]
        public LangType Type { get; init; }

        [JsonPropertyName("language")]
        public LangLanguage Language { get; init; }

        [JsonPropertyName("lastChange")]
        public long LastChange { get; set; }

        [JsonPropertyName("langs")]
        public List<LangData> Items { get; init; }

        public LangDataCollection()
        {
            Items = [];
        }

        internal LangDataCollection(LangType type, LangLanguage language)
        {
            Type = type;
            Language = language;
            Items = [];

            Directory.CreateDirectory(LangsWatcher.GetOutputDirectoryPath(type, language));
        }

        public static LangDataCollection Load(LangType type, LangLanguage language)
        {
            string dataFilePath = Path.Join(LangsWatcher.GetOutputDirectoryPath(type, language), DATA_FILE_NAME);
            if (File.Exists(dataFilePath))
            {
                return Json.LoadFromFile<LangDataCollection>(dataFilePath);
            }

            return new(type, language);
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
            return Items.Find(x => x.Name.Equals(name));
        }

        public List<LangData> GetLangsByName(string name)
        {
            return Items.FindAll(x => x.Name.NormalizeCustom().Contains(name.NormalizeCustom()));
        }

        internal async Task<List<LangData>> FetchLangsAsync(bool force)
        {
            string versionFile = await FetchVersionFileAsync(force);
            if (string.IsNullOrEmpty(versionFile))
            {
                return [];
            }

            return await ProcessLangsAsync(versionFile);
        }

        private async Task<string> FetchVersionFileAsync(bool force)
        {
            string versionFileUrl = GetVersionFileUrl();

            try
            {
                using HttpResponseMessage response = await LangsWatcher.HttpRetryPolicy.ExecuteAsync(() => LangsWatcher.HttpClient.GetAsync(versionFileUrl));
                response.EnsureSuccessStatusCode();

                long lastModifiedHeader = response.Content.Headers.LastModified!.Value.ToUnixTimeMilliseconds();
                bool isMoreRecent = lastModifiedHeader > LastChange;

                if (isMoreRecent)
                {
                    LastChange = lastModifiedHeader;
                }

                if (isMoreRecent || force)
                {
                    string versionFile = await response.Content.ReadAsStringAsync();

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

            string[] langInfos = versionFileContent[3..].Split("|", StringSplitOptions.RemoveEmptyEntries);
            foreach (string langInfo in langInfos)
            {
                string[] langParameters = langInfo.Split(',');

                LangData langData = new(langParameters[0], int.Parse(langParameters[2]), Type, Language);
                if (!File.Exists(langData.GetFilePath()))
                {
                    await langData.DownloadExtractAndDiffAsync();

                    updatedLangsData.Add(langData);

                    int index = Items.FindIndex(x => x.Name.Equals(langData.Name));
                    if (index == -1)
                    {
                        Items.Add(langData);
                    }
                    else
                    {
                        Items[index] = langData;
                    }
                }
            }

            Save();
            return updatedLangsData;
        }

        private void Save()
        {
            Json.Save(this, Path.Join(LangsWatcher.GetOutputDirectoryPath(Type, Language), DATA_FILE_NAME));
        }
    }
}
