using System.Diagnostics;

namespace Cyberia.Utils;

public static class ExecuteCmd
{
    public static bool ExecuteCommand(string command, string args)
    {
        try
        {
            using var process = new Process()
            {
                StartInfo = new ProcessStartInfo(command, args)
                {
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();

            var message = process.StandardOutput.ReadToEnd();
            if (!string.IsNullOrEmpty(message))
            {
                Log.Information(message);
            }

            var error = process.StandardError.ReadToEnd();
            if (!string.IsNullOrEmpty(error))
            {
                Log.Error(error);
                return false;
            }

            return true;
        }
        catch (Exception)
        {
            Log.Error("An error occurred while executing {CommandName} with {CommandArguments} arguments", command, args);
            return false;
        }
    }
}
