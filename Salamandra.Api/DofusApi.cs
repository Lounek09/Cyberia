global using Salamandra.Utils;

global using SQLite;

namespace Salamandra.Api
{
    public sealed class DofusApi
    {
        public SQLiteAsyncConnection Database { get; private set; }

        internal Logger Logger { get; private set; }

        internal static DofusApi Instance {
            get => _instance is null ? throw new NullReferenceException("Build the Api before !") : _instance;
            private set => _instance = value;
        }
        private static DofusApi? _instance;

        internal DofusApi(Logger logger)
        {
            Logger = logger;
            Database = new(Constant.DATABASE_PATH);
        }

        public static DofusApi Build(Logger logger)
        {
            _instance = new(logger);
            return _instance;
        }
    }
}
