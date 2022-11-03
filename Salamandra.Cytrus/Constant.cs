namespace Salamandra.Cytrus
{
    internal static class Constant
    {
        public const string OUTPUT_PATH = "cytrus";
        public const string CYTRUS_FILE_NAME = "cytrus.json";
        public const string CYTRUS_PATH = $"{OUTPUT_PATH}/{CYTRUS_FILE_NAME}";

        public const string BASE_ADRESS = "https://cytrus.cdn.ankama.com";

        public static string GetGameManifestUrl(string game, string release, string platform, string version)
        {
            return $"{BASE_ADRESS}/{game}/releases/{release}/{platform}/6.0_{version}.json";
        }
    }
}
