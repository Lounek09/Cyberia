using System.Diagnostics;

namespace Cyberia.Utils;

/// <summary>
/// A utility class for executing command line commands.
/// </summary>
public static class ExecuteCmd
{
    /// <summary>
    /// Executes a command line command.
    /// </summary>
    /// <param name="command">The command to execute.</param>
    /// <param name="args">The arguments for the command.</param>
    /// <param name="workingDirectory">The working directory for the command. Default is empty string.</param>
    /// <returns>True if the command executed successfully; otherwise, false.</returns>
    public static bool Execute(string command, string args, string workingDirectory = "")
    {
        ProcessStartInfo startInfo = new()
        {
            FileName = command,
            Arguments = args,
            WorkingDirectory = workingDirectory,
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };

        try
        {
            using var process = new Process
            {
                StartInfo = startInfo
            };

            process.OutputDataReceived += Process_OutputDataReceived;
            process.ErrorDataReceived += Process_ErrorDataReceived;

            process.Start();

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            process.WaitForExit();
            return process.ExitCode == 0;
        }
        catch (Exception e)
        {
            Log.Error(e, "An error occured while execute the command {CommandName}", command);
            return false;
        }
    }

    /// <summary>
    /// Handles the OutputDataReceived event of the Process.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The DataReceivedEventArgs instance containing the event data.</param>
    private static void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
    {
        if (!string.IsNullOrEmpty(e.Data))
        {
            Log.Information(e.Data);
        }
    }

    /// <summary>
    /// Handles the ErrorDataReceived event of the Process.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The DataReceivedEventArgs instance containing the event data.</param>
    private static void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
    {
        if (!string.IsNullOrEmpty(e.Data))
        {
            Log.Error(e.Data);
        }
    }
}
