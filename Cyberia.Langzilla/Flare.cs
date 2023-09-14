namespace Cyberia.Langzilla
{
    internal static class Flare
    {
        private static readonly object _lock = new();

        internal static bool TryExtractSwf(string swfFilePath, out string warningMessage, out string outputFilePath)
        {
            lock (_lock)
            {
                outputFilePath = $"{swfFilePath.TrimEnd(".swf")}.flr";
                return ExecuteCmd.ExecuteCommand(GetFlareExecutablePath(), swfFilePath, out warningMessage);
            }
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

            PlatformNotSupportedException exception = new();
            LangsWatcher.Instance.Log.Fatal(exception, "Flare is only available in Windows or Linux (it's false but fuck mac)");
            throw exception;
        }
    }
}
