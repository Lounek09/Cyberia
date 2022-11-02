namespace Salamandra.Bot
{
    internal static class Constant
    {
        public const string OUTPUT_PATH = "bot";
        public const string CONFIG_PATH = $"{OUTPUT_PATH}/config.json";
        public const string TEMP_PATH = "temp";

        public static string GetClientJsonUrl(string game, string release, string platform, string version)
        {
            return $"https://cytrus.cdn.ankama.com/{game}/releases/{release}/{platform}/6.0_{version}.json";
        }
    }
}
