namespace Cyberia
{
    public sealed class Config
    {
        private const string PATH = "config.json";

        public string TrelloUrl { get; init; }
        public bool EnableCheckLang { get; init; }
        public bool EnableCheckBetaLang { get; init; }
        public bool EnableCheckTemporisLang { get; init; }
        public bool EnableCheckCytrus { get; init; }

        public Config()
        {
            TrelloUrl = string.Empty;
        }

        public static Config Build()
        {
            if (!File.Exists(PATH))
            {
                Cyberia.Logger.Crit($"Configuration file not found at {Directory.GetCurrentDirectory()}/{PATH}");
                Console.ReadKey();
                Environment.Exit(0);
            }

            return Json.LoadFromFile<Config>(PATH);
        }
    }
}
