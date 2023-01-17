namespace Salamandra
{
    public sealed class Config
    {
        public string TrelloUrl { get; set; }
        public bool EnableCheckLang { get; set; }
        public bool EnableCheckBetaLang { get; set; }
        public bool EnableCheckTemporisLang { get; set; }
        public bool EnableCheckCytrus { get; set; }
        public bool EnableAutomaticCytrusManifestDiff { get; set; }

        public Config()
        {
            TrelloUrl = string.Empty;
        }

        public static Config Build()
        {
            if (!File.Exists(Constant.CONFIG_PATH))
            {
                Salamandra.Logger.Crit($"Configuration file not found at {Constant.CONFIG_PATH}");
                Console.ReadKey();
                Environment.Exit(0);
            }

            return Json.LoadFromFile<Config>(Constant.CONFIG_PATH);
        }
    }
}
