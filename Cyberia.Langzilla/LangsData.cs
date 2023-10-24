using Cyberia.Langzilla.Enums;

namespace Cyberia.Langzilla
{
    public sealed class LangsData
    {
        public const string DATA_FILE_NAME = "data.json";

        public LangType Type { get; init; }
        public Language Language { get; init; }
        public long LastModified { get; set; }
        public List<Lang> Langs { get; init; }

        public LangsData() //Used for deserialization
        {
            Langs = new();
        }

        internal LangsData(LangType type, Language language)
        {
            Type = type;
            Language = language;
            Langs = new();

            Directory.CreateDirectory(LangsWatcher.GetOutputDirectoryPath(type, language));
        }

        public static LangsData Load(LangType type, Language language)
        {
            string dataFilePath = Path.Join(LangsWatcher.GetOutputDirectoryPath(type, language), DATA_FILE_NAME);
            if (File.Exists(dataFilePath))
            {
                return Json.LoadFromFile<LangsData>(dataFilePath);
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

        public DateTime GetDateTimeSinceLastModified()
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(LastModified).DateTime;
        }

        public Lang? GetLangByName(string name)
        {
            return Langs.Find(x => x.Name.Equals(name));
        }

        public List<Lang> GetLangsByName(string name)
        {
            return Langs.FindAll(x => x.Name.NormalizeCustom().Contains(name.NormalizeCustom()));
        }

        internal async Task<List<Lang>> FetchLangsAsync(bool force)
        {
            string versionFile = await FetchVersionFileAsync(force);
            if (string.IsNullOrEmpty(versionFile))
            {
                return new();
            }

            return await ProcessLangsAsync(versionFile);
        }

        private async Task<string> FetchVersionFileAsync(bool force)
        {
            string versionFileUrl = GetVersionFileUrl();

            try
            {
                using HttpResponseMessage response = await LangsWatcher.Instance.HttpRetryPolicy.ExecuteAsync(() => LangsWatcher.Instance.HttpClient.GetAsync(versionFileUrl));
                response.EnsureSuccessStatusCode();

                long lastModifiedHeader = response.Content.Headers.LastModified!.Value.ToUnixTimeMilliseconds();
                bool isMoreRecent = lastModifiedHeader > LastModified;

                if (isMoreRecent)
                {
                    LastModified = lastModifiedHeader;
                }

                if (isMoreRecent || force)
                {
                    string versionFile = await response.Content.ReadAsStringAsync();

                    Log.Information("New {type} langs detected in {language} :\n{versionFile}", Type, Language, versionFile);
                    File.WriteAllText(GetVersionFilePath(), versionFile);

                    return versionFile;
                }
            }
            catch (HttpRequestException e)
            {
                Log.Error(e, "An error occured while sending Get request to {url}}", versionFileUrl);
            }

            return string.Empty;
        }

        private async Task<List<Lang>> ProcessLangsAsync(string versionFileContent)
        {
            List<Lang> updatedLangs = new();

            string[] langInfoArray = versionFileContent[3..].Split("|", StringSplitOptions.RemoveEmptyEntries);
            foreach (string langInfo in langInfoArray)
            {
                string[] langParameters = langInfo.Split(',');

                Lang lang = new(langParameters[0], int.Parse(langParameters[2]), Type, Language);
                if (!File.Exists(lang.GetFilePath()))
                {
                    await lang.DownloadExtractAndDiffAsync();

                    updatedLangs.Add(lang);

                    int index = Langs.FindIndex(x => x.Name.Equals(lang.Name));
                    if (index == -1)
                    {
                        Langs.Add(lang);
                    }
                    else
                    {
                        Langs[index] = lang;
                    }
                }
            }

            Save();
            return updatedLangs;
        }

        private void Save()
        {
            Json.Save(this, Path.Join(LangsWatcher.GetOutputDirectoryPath(Type, Language), DATA_FILE_NAME));
        }
    }
}
