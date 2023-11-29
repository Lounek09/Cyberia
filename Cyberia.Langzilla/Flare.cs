namespace Cyberia.Langzilla;

internal static class Flare
{
    private static readonly object _lock = new();

    internal static bool TryExtractSwf(string swfFilePath, out string outputFilePath)
    {
        lock (_lock)
        {
            outputFilePath = $"{swfFilePath.TrimEnd(".swf")}.flr";
            return ExecuteCmd.ExecuteCommand(GetFlareExecutablePath(), swfFilePath);
        }
    }

    private static string GetFlareExecutablePath()
    {
        if (OperatingSystem.IsWindows())
        {
            return Path.Join("flare", "flare.exe");
        }

        if (OperatingSystem.IsLinux())
        {
            if (Environment.Is64BitOperatingSystem)
            {
                return Path.Join("flare", "flare64");
            }

            return Path.Join("flare", "flare32");
        }

        var exception = new PlatformNotSupportedException();
        Log.Fatal(exception, "Flare is only available in Windows or Linux (it's false but fuck mac)");
        throw exception;
    }
}
