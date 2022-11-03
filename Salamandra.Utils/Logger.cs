using System.Drawing;

namespace Salamandra.Utils
{
    public sealed class Logger
    {
        private enum Level
        {
            CRIT,
            ERROR,
            INFO
        }

        private const string OUTPUT = "logs";

        private readonly object _lock;

        public Logger()
        {
            if (!Directory.Exists(OUTPUT))
                Directory.CreateDirectory(OUTPUT);

            _lock = new();
        }

        private void Log(string message, Level level)
        {
            string dateFormat = $"[{DateTime.Now:dd/MM/yyyy HH:mm:ss:ffff}]";
            string levelFormat = $"[{level,5}]";

            string file = $"{OUTPUT}/log_{DateTime.Now:yyyy-MM-dd}.txt";
            if (!File.Exists(file))
                File.Create(file).Dispose();

            lock (_lock)
            {
                using (StreamWriter stream = new(file, true))
                    stream.WriteLine(dateFormat + levelFormat + " " + message);
            }

            switch (level)
            {
                case Level.CRIT:
                    levelFormat = levelFormat.SetStyle(ConsoleFormat.Style.BOLD).SetBgColor(Color.LightGoldenrodYellow).SetColor(Color.Red);
                    break;
                case Level.ERROR:
                    levelFormat = levelFormat.SetColor(Color.IndianRed);
                    break;
                case Level.INFO:
                    levelFormat = levelFormat.SetColor(Color.SteelBlue);
                    break;
            }
            Console.WriteLine(dateFormat.SetColor(Color.Gray) + levelFormat + " " + message);
        }

        public void Crit(string message)
        {
            Log(message, Level.CRIT);
        }

        public void Crit(Exception exception)
        {
            Log($"{exception.Message}\n{exception.StackTrace}", Level.CRIT);
        }

        public void Error(string message)
        {
            Log(message, Level.ERROR);
        }

        public void Error(Exception exception)
        {
            Log($"{exception.Message}\n{exception.StackTrace}", Level.ERROR);
        }

        public void Info(string message)
        {
            Log(message, Level.INFO);
        }
    }
}
