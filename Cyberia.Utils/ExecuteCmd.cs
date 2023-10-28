using System.Diagnostics;

namespace Cyberia.Utils
{
    public static class ExecuteCmd
    {
        public static bool ExecuteCommand(string command, string args)
        {
            try
            {
                using Process process = new()
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

                if (process.StandardOutput.BaseStream.Length > 0)
                {
                    Log.Information(process.StandardOutput.ReadToEnd());
                }

                if (process.StandardError.BaseStream.Length > 0)
                {
                    Log.Error(process.StandardError.ReadToEnd());
                    return false;
                }

                return true;
            }
            catch (Exception)
            {
                Log.Error("An error occured while executing {command} with {args} arguments", command, args);
                return false;
            }
        }
    }
}
