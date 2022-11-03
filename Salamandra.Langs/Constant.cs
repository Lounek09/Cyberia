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

        public static string GetFlareExecutablePath()
        {
            string path = $"{OUTPUT_PATH}/flare";

            if (OperatingSystem.IsWindows())
                return $"{path}/flare.exe";
            else if (OperatingSystem.IsLinux())
            {
                if (Environment.Is64BitOperatingSystem)
                    return $"{path}/flare64";

                return $"{path}/flare32";
            }

            PlatformNotSupportedException exception = new("Flare is only available in Windows or Linux (it's false but fuck mac)");
            DofusLangs.Instance.Logger.Crit(exception);
            throw exception;
        }
    }
}
