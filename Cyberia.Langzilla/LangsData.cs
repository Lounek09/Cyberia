using Cyberia.Langzilla.Enums;

namespace Cyberia.Langzilla
{
    public sealed class LangsData
    {
        public LangType Type { get; init; }
        public Language Language { get; init; }
        public long LastModified { get; set; }
        public List<Lang> Langs { get; init; }

        public LangsData()
        {
            Langs = new();
        }

        internal LangsData(LangType type, Language language)
        {
            Type = type;
            Language = language;
            Langs = new();

            Directory.CreateDirectory(GetDirectoryPath());
        }

        public string GetDirectoryPath()
        {
            return DofusLangs.GetDirectoryPath(Type, Language);
        }

        public string GetDataFilePath()
        {
            return Path.Join(GetDirectoryPath(), "data.json");
        }

        public string GetVersionFileName()
        {
            return $"versions_{Language.ToString().ToLower()}.txt";
        }

        public string GetVersionFileUrl()
        {
            return $"{DofusLangs.BASE_URL}/{DofusLangs.GetRoute(Type)}/{GetVersionFileName()}";
        }

        public string GetVersionFilePath()
        {
            return Path.Join(GetDirectoryPath(), GetVersionFileName());
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
            return Langs.FindAll(x => x.Name.RemoveDiacritics().Contains(name.RemoveDiacritics()));
        }

        internal async Task<string[]> GetLangsInfosFromServerAsync(bool force)
        {
            string versionFileUrl = GetVersionFileUrl();

            try
            {
                using HttpResponseMessage response = await DofusLangs.Instance.HttpClient.GetAsync(versionFileUrl).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();

                long lastModifiedHeader = response.Content.Headers.LastModified!.Value.ToUnixTimeMilliseconds();
                long lastModified = LastModified;

                bool isMoreRecent = lastModifiedHeader > lastModified;
                if (!force && !isMoreRecent)
                    return Array.Empty<string>();

                string versions = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (isMoreRecent)
                {
                    DofusLangs.Instance.Logger.Info($"New {Type} langs detected in {Language} :\n{versions}");

                    LastModified = lastModifiedHeader;
                    File.WriteAllText(GetVersionFilePath(), versions);
                }

                return versions[3..].Split("|", StringSplitOptions.RemoveEmptyEntries);
            }
            catch (HttpRequestException e)
            {
                DofusLangs.Instance.Logger.Error($"Unable to find {versionFileUrl}", e);
            }
            catch (TaskCanceledException e)
            {
                DofusLangs.Instance.Logger.Error($"The request to get {versionFileUrl} has been cancelled", e);
            }

            return Array.Empty<string>();
        }

        internal void UpdateLangs(List<Lang> updatedLangs)
        {
            foreach (Lang updatedLang in updatedLangs)
            {
                int index = Langs.FindIndex(x => x.Name.Equals(updatedLang.Name));
                if (index == -1)
                    Langs.Add(updatedLang);
                else
                    Langs[index] = updatedLang;
            }
        }

        internal void Save()
        {
            Json.Save(this, GetDataFilePath());
        }
    }
}
