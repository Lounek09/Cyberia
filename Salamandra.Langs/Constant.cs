namespace Salamandra.Langs
{
    internal static class Constant
    {
        public const string OUTPUT_PATH = "langs";
        public const string CONFIG_PATH = $"{OUTPUT_PATH}/config.json";

        public const string BASE_ADRESS = "https://dofusretro.cdn.ankama.com";

        public static string GetRoute(LangType type)
        {
            switch (type)
            {
                case LangType.Beta:
                    return "betaenv/lang";
                case LangType.Temporis:
                    return "temporis/lang";
                default:
                    return "lang";
            }
        }
    }
}
