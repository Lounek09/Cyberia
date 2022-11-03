using System.Diagnostics;

namespace Salamandra.Utils
{
    public static class ExecuteCmd
    {
        public static bool ExecuteCommand(string command, string args, out string message)
        {
            try
            {
                Process process = new()
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

                message = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                if (!string.IsNullOrEmpty(error))
                {
                    message += "\n" + error;
                    return false;
                }

                return true;
            }
            catch (Exception)
            {
                message = $"Error when execute command {command}{(string.IsNullOrEmpty(args) ? "" : " with args " + args)}";
                return false;
            }
        }
    }
}