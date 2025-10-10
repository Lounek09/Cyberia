using System.Diagnostics;

namespace Cyberia.Utils;

/// <summary>
/// Provides methods for executing command lines.
/// </summary>
public static class ExecuteCmd
{
    /// <summary>
    /// Executes a command line command.
    /// </summary>
    /// <param name="command">The command to execute.</param>
    /// <param name="args">The arguments for the command.</param>
    /// <param name="workingDirectory">The working directory for the command. If empty, the current directory is used.</param>
    /// <returns><see langword="true"/> if the command executed successfully; otherwise, <see langword="false"/>.</returns>
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
    /// <param name="args">The DataReceivedEventArgs instance containing the event data.</param>
    private static void Process_OutputDataReceived(object sender, DataReceivedEventArgs args)
    {
        if (!string.IsNullOrEmpty(args.Data))
        {
            Log.Information(args.Data);
        }
    }

    /// <summary>
    /// Handles the ErrorDataReceived event of the Process.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="args">The DataReceivedEventArgs instance containing the event data.</param>
    private static void Process_ErrorDataReceived(object sender, DataReceivedEventArgs args)
    {
        if (!string.IsNullOrEmpty(args.Data))
        {
            Log.Error(args.Data);
        }
    }
}
