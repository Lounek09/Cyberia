using System.Collections.Concurrent;

namespace Salamandra.Langs
{
    public sealed class LangsConfig
    {
        public ConcurrentDictionary<string, ConcurrentDictionary<string, long>> LastModifiedLangs { get; set; }

        public LangsConfig()
        {
            LastModifiedLangs = new();
        }

        public static LangsConfig Build()
        {
            LangsConfig config;

            if (File.Exists(Constant.CONFIG_PATH))
                config = Json.LoadFromFile<LangsConfig>(Constant.CONFIG_PATH);
            else
            {
                config = new LangsConfig();
                Json.Save(config, Constant.CONFIG_PATH);
            }

            return config;
        }

        internal void Save()
        {
            Json.Save(this, Constant.CONFIG_PATH);
        }

        internal long GetLastModifiedByLangTypeAndLanguage(LangType langType, Language language)
        {
            return LastModifiedLangs.GetOrAdd(langType.ToString(), new ConcurrentDictionary<string, long>())
                                    .GetOrAdd(language.ToString(), 0);
        }

        internal void SetLastModifiedByLangTypeAndLanguage(LangType langType, Language language, long value)
        {
            LastModifiedLangs.GetOrAdd(langType.ToString(), new ConcurrentDictionary<string, long>())
                             .AddOrUpdate(language.ToString(), value, (key, oldValue) => oldValue = value);
        }
    }
}
