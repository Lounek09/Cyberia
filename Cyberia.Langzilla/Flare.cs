using System.Diagnostics.CodeAnalysis;

namespace Cyberia.Langzilla;

internal static class Flare
{
    private static readonly object _lock = new();
    private static readonly string _flarePath = GetFlarePath();

    internal static bool TryExtract(string inputFilePath, [NotNullWhen(true)] out string? outputFilePath)
    {
        if (!File.Exists(inputFilePath))
        {
            outputFilePath = null;
            return false;
        }

        lock (_lock)
        {
            if (!ExecuteCmd.Execute(_flarePath, inputFilePath))
            {
                outputFilePath = null;
                return false;
            }
        }

        outputFilePath = $"{inputFilePath.TrimEnd(".swf")}.flr";
        if (File.Exists(outputFilePath))
        {
            return true;
        }

        outputFilePath = null;
        return false;
    }

    private static string GetFlarePath()
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
