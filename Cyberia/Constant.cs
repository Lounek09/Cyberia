namespace Cyberia
{
    public static class Constant
    {
#if DEBUG
        public const bool DEBUG = true;
        public const string ENV = "debug";
#else
        public const bool DEBUG = false;
        public const string ENV = "main";
#endif
        public const string CONFIG_PATH = "config.json";

        public const string LOGS_PATH = "logs";
        public const string API_PATH = "api";
        public const string CYTRUS_PATH = "cytrus";
        public const string LANGS_PATH = "langs";
        public const string TEMP_PATH = "temp";

        static Constant()
        {
            if (Directory.Exists(TEMP_PATH))
                Directory.CreateDirectory(TEMP_PATH);
        }
    }
}


