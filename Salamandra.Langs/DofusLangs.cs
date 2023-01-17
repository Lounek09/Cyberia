global using Salamandra.Langs.Enums;
global using Salamandra.Utils;

using System.Text;

namespace Salamandra.Langs
{
    public sealed class DofusLangs
    {
        public Logger Logger { get; private set; }
        public LangsConfig Config { get; private set; }

        internal HttpClient HttpClient { get; private set; }

        internal static DofusLangs Instance {
            get => _instance is null ? throw new NullReferenceException("Build the Langs before !") : _instance;
        }
        private static DofusLangs? _instance;

        internal DofusLangs(LangsConfig config)
        {
            Logger = new("langs");
            Config = config;
            HttpClient = new()
            {
                BaseAddress = new Uri(Constant.BASE_ADRESS)
            };
        }

        public static DofusLangs Build()
        {
            if (!File.Exists(Constant.CONFIG_PATH))
                Json.Save(new LangsConfig(), Constant.CONFIG_PATH);

            _instance ??= new(Json.LoadFromFile<LangsConfig>(Constant.CONFIG_PATH));
            return _instance;
        }

        public event EventHandler<CheckLangStartedEventArgs>? CheckLangStarted;
        public event EventHandler<NewerLangDetectedEventArgs>? NewerLangDetected;
        public event EventHandler<CheckLangFinishedEventArgs>? CheckLangFinished;

        /// <summary>
        /// Checks if the <see cref="LangType"/> langs in <see cref="Language"/> have been updated.
        /// </summary>
        /// <param name="type">The lang type to check to</param>
        /// <param name="language">The language to check to</param>
        /// <param name="force">Force the check to be made</param>
        public async Task Launch(LangType type, Language language, bool force = false)
        {
            CheckLangStarted?.Invoke(this, new CheckLangStartedEventArgs(type, language));

            string route = Constant.GetRoute(type);

            string[] langsFileName = await GetLangsFileName(type, language, route, force);
            List<Lang> langs = GetLangs(type, language, route, langsFileName);

            CheckLangFinished?.Invoke(this, new CheckLangFinishedEventArgs(type, language, langs));
        }

        private async Task<string[]> GetLangsFileName(LangType type, Language language, string route, bool force = false)
        {
            string? versions = null;
            try
            {
                using (HttpResponseMessage response = await HttpClient.GetAsync($"{route}/versions_{language.ToString().ToLower()}.txt").ConfigureAwait(false))
                {
                    response.EnsureSuccessStatusCode();

                    DateTimeOffset? lastModifiedHeader = response.Content.Headers.LastModified;
                    long lastModified = Config.GetLastModifiedByLangTypeAndLanguage(type, language);

                    bool isMoreRecent = lastModifiedHeader.HasValue && lastModifiedHeader.Value.Ticks > lastModified;
                    if (isMoreRecent)
                    {
                        Config.SetLastModifiedByLangTypeAndLanguage(type, language, lastModifiedHeader!.Value.Ticks);
                        Config.Save();
                    }

                    if (force || isMoreRecent)
                        versions = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                }
            }
            catch (HttpRequestException e)
            {
                Logger.Error($"Unable to find the versions_{language.ToString().ToLower()}.txt file for {type} langs\n{e.Message}");
            }

            if (versions is null)
                return Array.Empty<string>();

            string[] langsFileName = versions[3..].Replace(',', '_').Split("|", StringSplitOptions.RemoveEmptyEntries);
            if (langsFileName.Length == 0)
            {
                Logger.Error($"The format of versions_{language.ToString().ToLower()}.txt file is incorrect");
            }

            return langsFileName;
        }

        private List<Lang> GetLangs(LangType type, Language language, string route, IEnumerable<string> langsFileName)
        {
            List<Lang> langs = new();
            foreach (string fileName in langsFileName)
            {
                string[] args = fileName.Split('_');
                if (args.Length != 3 ||
                    !args[1].Equals(language.ToString().ToLower()) ||
                    !int.TryParse(args[2], out int version))
                {
                    Logger.Error($"The format of versions_{language.ToString().ToLower()}.txt is incorrect");
                    continue;
                }
                string name = args[0];
                string directoryPath = $"{Constant.OUTPUT_PATH}/{type.ToString().ToLower()}/{language.ToString().ToLower()}/{name.ToLower()}";
                string filePath = $"{directoryPath}/{fileName}.swf";
                string fileRoute = $"/{route}/swf/{fileName}.swf";

                if (!File.Exists(filePath))
                {
                    Lang lang = new(name, version, type, language, !Directory.Exists(directoryPath), filePath, directoryPath, fileRoute);
                    langs.Add(lang);
                    NewerLangDetected?.Invoke(this, new NewerLangDetectedEventArgs(type, language, lang));
                }
            }

            return langs;
        }

        /// <summary>
        /// Diff the last extracted lang, see <see cref="ExtractLang"/>, in the directory of <paramref name="lang"/>.
        /// </summary>
        /// <param name="lang">The lang to diff to</param>
        /// <returns>True if the lang is successfully diff</returns>
        /// <exception cref="FileNotFoundException"></exception>
        public bool DiffLastExtractedLang(Lang lang, out string outputPath)
        {
            outputPath = $"{lang.DirectoryPath}/diff.as";

            if (!File.Exists($"{lang.DirectoryPath}/current.as"))
            {
                Logger.Error($"No extracted lang in '{lang.DirectoryPath}'");
                return false;
            }

            List<KeyValuePair<int, string>> diff = new();

            int index = 0;
            Dictionary<int, string> currentRows = File.ReadAllLines($"{lang.DirectoryPath}/current.as").ToDictionary(x => index++);

            index = 0;
            Dictionary<int, string> oldRows = File.Exists($"{lang.DirectoryPath}/old.as") ? File.ReadAllLines($"{lang.DirectoryPath}/old.as").ToDictionary(x => index++) : new();

            foreach (KeyValuePair<int, string> row in currentRows)
            {
                if (!oldRows.RemoveByValue(row.Value, true))
                    diff.Add(new(row.Key, $"+ {row.Value}"));
            }

            foreach (KeyValuePair<int, string> row in oldRows)
                diff.Add(new(row.Key, $"- {row.Value}"));

            if (diff.Count > 0)
                File.WriteAllLines(outputPath, diff.OrderBy(x => x.Key).Select(x => x.Value), Encoding.UTF8);
            else
                File.Delete(outputPath);

            return true;
        }
    }

    public sealed class CheckLangStartedEventArgs : EventArgs
    {
        public LangType Type { get; set; }
        public Language Language { get; set; }

        public CheckLangStartedEventArgs(LangType type, Language language)
        {
            Type = type;
            Language = language;
        }
    }

    public sealed class NewerLangDetectedEventArgs : EventArgs
    {
        public LangType Type { get; set; }
        public Language Language { get; set; }
        public Lang Lang { get; set; }

        public NewerLangDetectedEventArgs(LangType type, Language language, Lang lang)
        {
            Type = type;
            Language = language;
            Lang = lang;
        }
    }

    public sealed class CheckLangFinishedEventArgs : EventArgs
    {
        public LangType Type { get; set; }
        public Language Language { get; set; }
        public List<Lang> Langs { get; set; }

        public CheckLangFinishedEventArgs(LangType type, Language language, List<Lang> langs)
        {
            Type = type;
            Language = language;
            Langs = langs;
        }
    }
}
