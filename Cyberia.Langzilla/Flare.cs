using System.Diagnostics.CodeAnalysis;

namespace Cyberia.Langzilla;

/// <summary>
/// Provides methods for extracting SWF files using Flare.
/// </summary>
internal static class Flare
{
    private static readonly Lock s_lock = new();
    private static readonly string s_flarePath = GetFlarePath();

    /// <summary>
    /// Tries to extract the specified input SWF file.
    /// </summary>
    /// <param name="inputFilePath">The input SWF file path.</param>
    /// <param name="outputFilePath">The output extracted file path.</param>
    /// <returns><see langword="true"/> if the extraction was successful; otherwise, <see langword="false"/>.</returns>
    internal static bool TryExtract(string inputFilePath, [NotNullWhen(true)] out string? outputFilePath)
    {
        if (!inputFilePath.EndsWith(".swf") ||
            !File.Exists(inputFilePath))
        {
            outputFilePath = null;
            return false;
        }

        lock (s_lock)
        {
            if (!ExecuteCmd.Execute(s_flarePath, inputFilePath))
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

    /// <summary>
    /// Gets the path of the flare executable depending of the current OS.
    /// </summary>
    /// <returns>The path of the flare executable.</returns>
    internal static string GetFlarePath()
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
        Log.Fatal(exception, "Flare is only available in Windows, Linux or Mac but only in 32bits which is no longer supported");
        throw exception;
    }
}
