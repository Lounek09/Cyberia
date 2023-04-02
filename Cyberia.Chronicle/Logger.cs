using Cyberia.Chronicle.Enums;

using System.Drawing;

namespace Cyberia.Chronicle
{
    public sealed class Logger
    {
        private const string OUTPUT = "logs";

        private readonly object _lock;
        private readonly string _name;

        public Logger(string name)
        {
            if (!Directory.Exists(OUTPUT))
                Directory.CreateDirectory(OUTPUT);

            _lock = new();
            _name = name.ToLower();
        }

        private void Log(string message, Level level)
        {
            string dateFormat = $"[{DateTime.Now:yyyy/MM/dd HH:mm:ss:ffff}]";
            string nameFormat = $"[{_name.ToUpper()}]";
            string levelFormat = $"[{level,5}]";

            string file = $"{OUTPUT}/log_{_name}_{DateTime.Now:yyyy-MM}.txt";
            lock (_lock)
            {
                if (!File.Exists(file))
                    File.Create(file).Dispose();

                using (StreamWriter stream = new(file, true))
                    stream.WriteLine(dateFormat + levelFormat + " " + message);
            }

            switch (level)
            {
                case Level.CRIT:
                    levelFormat = levelFormat.SetStyle(Style.BOLD)
                        .SetBgColor(Color.LightGoldenrodYellow)
                        .SetColor(Color.Red);
                    break;
                case Level.ERROR:
                    levelFormat = levelFormat.SetColor(Color.IndianRed);
                    break;
                case Level.INFO:
                    levelFormat = levelFormat.SetColor(Color.SteelBlue);
                    break;
                case Level.DEBUG:
                    levelFormat = levelFormat.SetColor(Color.MediumPurple);
                    break;
            }

            Console.WriteLine(ConsoleFormat.SetColor(dateFormat + nameFormat, Color.LightGray) + levelFormat + " " + message);
        }

        public void Crit(string message)
        {
            Log(message, Level.CRIT);
        }

        public void Crit(Exception exception)
        {
            Log($"{exception.Message}\n{exception.StackTrace}", Level.CRIT);
        }

        public void Crit(string message, Exception exception)
        {
            Log($"{message}\n{exception.Message}\n{exception.StackTrace}", Level.CRIT);
        }

        public void Error(string message)
        {
            Log(message, Level.ERROR);
        }

        public void Error(Exception exception)
        {
            Log($"{exception.Message}\n{exception.StackTrace}", Level.ERROR);
        }

        public void Error(string message, Exception exception)
        {
            Log($"{message}\n{exception.Message}\n{exception.StackTrace}", Level.ERROR);
        }

        public void Info(string message)
        {
            Log(message, Level.INFO);
        }

        public void Debug(string message)
        {
#if DEBUG
            Log(message, Level.DEBUG);
#endif
        }
    }
}
