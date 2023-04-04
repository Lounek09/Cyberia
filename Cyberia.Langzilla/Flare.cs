namespace Cyberia.Langzilla
{
    internal static class Flare
    {
        private static readonly object _lock = new();

        internal static bool ExtractSwf(string swfPath, out string warningMessage)
        {
            lock (_lock)
                return ExecuteCmd.ExecuteCommand(GetFlareExecutablePath(), swfPath, out warningMessage);
        }

        private static string GetFlareExecutablePath()
        {
            if (OperatingSystem.IsWindows())
                return "flare/flare.exe";

            if (OperatingSystem.IsLinux())
            {
                if (Environment.Is64BitOperatingSystem)
                    return "flare/flare64";

                return "flare/flare32";
            }

            PlatformNotSupportedException exception = new("Flare is only available in Windows or Linux (it's false but fuck mac)");
            DofusLangs.Instance.Logger.Crit(exception);
            throw exception;
        }
    }
}
