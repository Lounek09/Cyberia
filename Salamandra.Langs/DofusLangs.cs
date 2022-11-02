using System.Text;

namespace Salamandra.Langs
{
    public sealed class DofusLangs
    {
        public Flare Flare { get; set; }

        private readonly Logger _logger;
        private readonly LangsConfig _config;
        private readonly HttpClient _httpClient;

        internal DofusLangs(Logger logger, LangsConfig config)
        {
            _logger = logger;
            _config = config;
            _httpClient = new()
            {
                BaseAddress = new Uri(Constant.BASE_ADRESS)
            };
            Flare = new(logger);
        }

        public static DofusLangs Build(Logger logger)
        {
            if (!File.Exists(Constant.CONFIG_PATH))
                Json.Save(new LangsConfig(), Constant.CONFIG_PATH);

            return new(logger, Json.Load<LangsConfig>(Constant.CONFIG_PATH));
        }

        public event EventHandler<CheckLangStartedEventArgs>? CheckLangStarted;
        public event EventHandler<NewerLangDetectedEventArgs>? NewerLangDetected;
        public event EventHandler<CheckLangFinishedEventArgs>? CheckLangFinished;

        /// <summary>
        /// Checks if the <see cref="LangType"/> langs in <see cref="Language"/> have been updated.
        /// </summary>
        /// <param name="type">The lang type to check to</param>
        /// <param name="language">The language to check to</param>
        /// <returns>The newer langs</returns>
        /// <exception cref="FormatException"></exception>
        public async Task CheckForNewerLangs(LangType type, Language language)
        {
            CheckLangStarted?.Invoke(this, new CheckLangStartedEventArgs(type, language));

            string route = Constant.GetRoute(type);

            string? versionsFile = null;
            try
            {
                using (HttpResponseMessage response = await _httpClient.GetAsync($"{route}/versions_{language.ToString().ToLower()}.txt").ConfigureAwait(false))
                {
                    response.EnsureSuccessStatusCode();

                    DateTimeOffset? lastModifiedHeader = response.Content.Headers.LastModified;
                    long lastModified = _config.GetLastModifiedByLangTypeAndLanguage(type, language);

                    if (lastModifiedHeader.HasValue && lastModifiedHeader.Value.Ticks > lastModified)
                    {
                        _config.SetLastModifiedByLangTypeAndLanguage(type, language, lastModifiedHeader.Value.Ticks);
                        _config.Save();
                        versionsFile = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    }
                }
            }
            catch (HttpRequestException e)
            {
                _logger.Error($"Unable to find the versions_{language.ToString().ToLower()}.txt file for {type} langs\n{e.Message}");
            }

            if (string.IsNullOrEmpty(versionsFile))
                return;

            string[] versionsFileArgs = versionsFile[3..].Replace(',', '_').Split("|");
            if (versionsFileArgs.Length == 0)
            {
                _logger.Error($"The format of versions_{language.ToString().ToLower()}.txt is incorrect :\n{versionsFile}");
                return;
            }

            List<Lang> langs = new();
            foreach (string versionsFileArg in versionsFileArgs)
            {
                if (string.IsNullOrWhiteSpace(versionsFileArg))
                    continue;

                string[] langArgs = versionsFileArg.Split('_');
                if (langArgs.Length != 3 ||
                    !langArgs[1].Equals(language.ToString().ToLower()) ||
                    !int.TryParse(langArgs[2], out int version))
                {
                    _logger.Error($"The format of version_{language.ToString().ToLower()}.txt is incorrect :\n{versionsFile}");
                    continue;
                }
                string name = langArgs[0];
                string directoryPath = $"{Constant.OUTPUT_PATH}/{type.ToString().ToLower()}/{language.ToString().ToLower()}/{name.ToLower()}";
                string filePath = $"{directoryPath}/{versionsFileArg}.swf";
                string fileRoute = $"/{route}/swf/{versionsFileArg}.swf";

                if (!File.Exists(filePath))
                {
                    Lang lang = new(_logger, _httpClient, name, version, type, language, !Directory.Exists(directoryPath), filePath, directoryPath, fileRoute);
                    langs.Add(lang);
                    NewerLangDetected?.Invoke(this, new NewerLangDetectedEventArgs(type, language, lang));
                }
            }

            CheckLangFinished?.Invoke(this, new CheckLangFinishedEventArgs(type, language, langs));
            return;
        }

        /// <summary>
        /// Diff the last extracted lang, see <see cref="ExtractLang"/>, in the directory of <paramref name="lang"/>.
        /// </summary>
        /// <param name="lang">The lang to diff to</param>
        /// <returns>True if the lang is successfully diff</returns>
        /// <exception cref="FileNotFoundException"></exception>
        public bool DiffLastExtractedLang(Lang lang)
        {
            if (!File.Exists($"{lang.DirectoryPath}/current.as"))
            {
                _logger.Error($"No extracted lang in '{lang.DirectoryPath}'");
                return false;
            }

            int index = 0;
            Dictionary<int, string> currentRows = File.ReadAllLines($"{lang.DirectoryPath}/current.as").ToDictionary(x => index++);

            index = 0;
            Dictionary<int, string> oldRows = File.Exists($"{lang.DirectoryPath}/old.as") ? File.ReadAllLines($"{lang.DirectoryPath}/old.as").ToDictionary(x => index++) : new();

            List<KeyValuePair<int, string>> diffRows = new();
            foreach (KeyValuePair<int, string> row in currentRows)
            {
                if (!oldRows.RemoveByValue(row.Value))
                    diffRows.Add(new(row.Key, $"+ {row.Value}"));
            }
            foreach (KeyValuePair<int, string> row in oldRows)
                diffRows.Add(new(row.Key, $"- {row.Value}"));

            if (diffRows.Count > 0)
                File.WriteAllLines($"{lang.DirectoryPath}/diff.as", diffRows.OrderBy(x => x.Key).Select(x => x.Value), Encoding.UTF8);
            else
                File.Delete($"{lang.DirectoryPath}/diff.as");

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
